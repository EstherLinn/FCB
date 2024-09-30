using Feature.Wealth.Account.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Analytics;
using Sitecore.Mvc.Pipelines.Request.RequestBegin;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Logging;
using static Sitecore.Xdb.Configuration.XdbSettings;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace Feature.Wealth.Account.Pipelines
{
    public class XConnect : RequestBeginProcessor
    {
        public readonly ID XconnectRecord  =  new ID("{2E5BF640-E13E-4AF4-BA13-2E94C7FF705A}");
        public readonly ID IsTrigger = new ID("{A3E71F76-0069-463E-9F89-1DD50F256241}");

        public override void Process(RequestBeginArgs args)
        {
            try
            {
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
                        if (Tracker.Current == null)
                        {
                            Tracker.StartTracking();
                        }
                        if (Tracker.Current != null && Tracking.Enabled)
                        {
                            if (Tracker.Current.Session.Contact.Identifiers.Any(x => x.Source == platForm && x.Identifier == id))
                            {
                                return;
                            }
                            var reference = new IdentifiedContactReference(platForm, id);
                            //取得聯絡人
                            Contact contact = client.Get<Contact>(reference, new ContactExecutionOptions(new ContactExpandOptions() { }));
                            if (contact == null)
                            {
                                //聯絡人不在，建立聯絡人
                                var newContact = new Contact(new ContactIdentifier(platForm, id, ContactIdentifierType.Known));
                                var persoalInfoFacet = new PersonalInformation
                                {
                                    LastName = info.MemberName
                                };
                                client.SetFacet<PersonalInformation>(newContact, PersonalInformation.DefaultFacetKey, persoalInfoFacet);
                                var emailFacet = new EmailAddressList(new EmailAddress(info.MemberEmail, true), platForm);
                                client.SetFacet<EmailAddressList>(newContact, EmailAddressList.DefaultFacetKey, emailFacet);
                                client.AddContact(newContact);
                                client.AddContactIdentifier(newContact, new ContactIdentifier(platForm, id, ContactIdentifierType.Known));
                                client.Submit();
                            }
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
