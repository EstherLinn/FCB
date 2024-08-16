using Microsoft.Extensions.DependencyInjection;
using Sitecore.Configuration;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Xml;

namespace Feature.Wealth.Toolkit.Areas.Tools.Services
{
    public class ShowConfigService
    {
        private readonly IConfigurationRulesContext _configRulesContext;
        public ShowConfigService() => this._configRulesContext = ServiceLocator.ServiceProvider.GetService<IConfigurationRulesContext>();
        public XmlNode Show(HttpRequestBase request)
        {
            var nameValueCollection = new NameValueCollection();
            string empty = string.Empty;
            var configRulesContext = this._configRulesContext;
            var missingRules = (configRulesContext != null ? configRulesContext.GetRuleDefinitionNames().Select(name => name?.ToUpperInvariant()).ToList() : null) ?? new List<string>();
            foreach (string allKey in request.QueryString.AllKeys)
            {
                if (allKey == "layer")
                {
                    empty = request.QueryString[allKey];
                }
                else if (missingRules.Contains(allKey.ToUpperInvariant()))
                {
                    nameValueCollection.Add($"{allKey}:{RuleBasedConfigReader.RuleDefineSuffix}", request.QueryString[allKey]);
                    missingRules.Remove(allKey.ToUpperInvariant());
                }
            }

            XmlDocument xmlDocument;
            if (nameValueCollection.Keys.Count == 0 && string.IsNullOrEmpty(empty))
            {
                xmlDocument = Factory.GetConfiguration();
            }
            else
            {
                PopulateMissingRules(nameValueCollection, missingRules);
                xmlDocument = GetRuleBasedConfiguration(nameValueCollection, empty);
            }

            return xmlDocument;
        }

        private void PopulateMissingRules(NameValueCollection ruleCollections, List<string> missingRules)
        {
            foreach (string missingRule in missingRules)
            {
                string[] ruleDefinitionValue = this._configRulesContext.GetRuleDefinitionValue(missingRule.ToLowerInvariant());
                string str = ruleDefinitionValue != null ? ruleDefinitionValue.FirstOrDefault(ruleValue => !string.IsNullOrEmpty(ruleValue)) : null;
                string name = $"{missingRule.ToLowerInvariant()}:{RuleBasedConfigReader.RuleDefineSuffix}";
                if (ruleCollections[name] == null && str != null)
                {
                    ruleCollections.Add(name, str);
                }
            }
        }

        private XmlDocument GetRuleBasedConfiguration(NameValueCollection ruleCollection, string layers) => new RuleBasedConfigLayerReader(GetIncludeFiles(layers.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries)), ruleCollection).GetConfigurationLayer();

        private IEnumerable<string> GetIncludeFiles(IReadOnlyCollection<string> layers)
        {
            var source = new LayeredConfigurationFiles().ConfigurationLayerProviders.SelectMany(x => x.GetLayers());
            return layers.Count != 0 ? source.Where(x => layers.Contains(x.Name)).SelectMany(x => x.GetConfigurationFiles()).Distinct(StringComparer.OrdinalIgnoreCase) : source.SelectMany(x => x.GetConfigurationFiles()).Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class RuleBasedConfigLayerReader : RuleBasedConfigReader
    {
        public RuleBasedConfigLayerReader(IEnumerable<string> includeFiles, NameValueCollection ruleCollection) : base(includeFiles, ruleCollection) { }
        public XmlDocument GetConfigurationLayer() => base.DoGetConfiguration();
    }
}