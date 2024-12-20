using Feature.Wealth.Account.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Analytics;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.Request.RequestBegin;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Sitecore.Xdb.Configuration.XdbSettings;

namespace Feature.Wealth.Account.Pipelines
{
    public class XConnect : RequestBeginProcessor
    {
        public readonly ID XconnectRecord = new ID("{2E5BF640-E13E-4AF4-BA13-2E94C7FF705A}");
        public readonly ID IsTrigger = new ID("{A3E71F76-0069-463E-9F89-1DD50F256241}");

        public override void Process(RequestBeginArgs args)
        {

            try
            {
                if (args.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    return;
                }
                //確認後台開關開啟
                Item configItem = ItemUtils.GetItem(XconnectRecord);
                if (configItem == null)
                {
                    return;
                }
                if (configItem.IsChecked(IsTrigger) && FcbMemberHelper.CheckMemberLogin())
                {
                    var id = FcbMemberHelper.GetMemberPlatFormId();
                    var platForm = FcbMemberHelper.GetMemberPlatForm().ToString();
                    var info = FcbMemberHelper.GetMemberAllInfo();
                    using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
                    {
                        if (Tracker.Current == null && Tracking.Enabled)
                        {
                            Tracker.StartTracking();
                        }
                        if (Tracker.Current != null && Tracker.Current.IsActive)
                        {
                            // 如果聯絡人已識別，return
                            if (Tracker.Current.Session.Contact.Identifiers.Any(x => x.Source == platForm && x.Identifier == id))
                            {
                                return;
                            }
                            var reference = new IdentifiedContactReference(platForm, id);
                            Task.Run(async () =>
                            {
                                var contact = await client.GetAsync<Contact>(reference, new ContactExecutionOptions(
                                    new ContactExpandOptions(PersonalInformation.DefaultFacetKey,EmailAddressList.DefaultFacetKey)));
                                //取得聯絡人
                                if (contact == null)
                                {
                                    //聯絡人不在，建立聯絡人
                                    var newContact = new Contact(new ContactIdentifier(platForm, id, ContactIdentifierType.Known));
                                    var persoalInfoFacet = new PersonalInformation
                                    {
                                        FirstName = info.MemberName,
                                        LastName = string.Empty
                                    };
                                    client.SetFacet<PersonalInformation>(newContact, PersonalInformation.DefaultFacetKey, persoalInfoFacet);
                                    var emailFacet = new EmailAddressList(new EmailAddress(info.MemberEmail, true), platForm);
                                    client.SetFacet<EmailAddressList>(newContact, EmailAddressList.DefaultFacetKey, emailFacet);
                                    client.AddContact(newContact);
                                }
                                else
                                {
                                    //聯絡人存在，更新聯絡人資料
                                    var facet = contact.GetFacet<PersonalInformation>();
                                    var emailFacet =contact.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey);
                                    emailFacet.PreferredEmail = new EmailAddress(info.MemberEmail, true);
                                    emailFacet.PreferredKey = platForm;
                                    facet.FirstName = info.MemberName;
                                    facet.LastName = string.Empty;
                                    client.SetFacet<EmailAddressList>(contact, EmailAddressList.DefaultFacetKey, emailFacet);
                                    client.SetFacet<PersonalInformation>(contact, PersonalInformation.DefaultFacetKey, facet);
                                }
                                await client.SubmitAsync();
                            }).GetAwaiter().GetResult();
                            //識別聯絡人
                            var identificationManager = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetRequiredService<Sitecore.Analytics.Tracking.Identification.IContactIdentificationManager>();
                            Sitecore.Analytics.Tracking.Identification.IdentificationResult result = identificationManager.IdentifyAs(new Sitecore.Analytics.Tracking.Identification.KnownContactIdentifier(platForm, id));
                            if (!result.Success)
                            {
                                Logger.Account.Info(result.ErrorMessage);
                            }
                        }

                    }
                }
            }
            catch (XdbExecutionException ex)
            {
                Logger.Account.Info(ex);
            }
            catch (Exception ex)
            {
                Logger.Account.Info(ex);
            }
        }
    }
}
