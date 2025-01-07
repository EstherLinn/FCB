using Sitecore.Diagnostics;
using Sitecore.Xml.Patch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Foundation.Wealth.Helper
{
    /// <summary>
    /// The configuration roles helper.
    /// </summary>
    public class RoleConfigurationHelper
    {
        private string[] definedRoles;

        private string[] definedEnvironment;

        private string[] definedScheduleAgent;

        public RoleConfigurationHelper()
        {
            LoadAppSetting();
        }

        internal RoleConfigurationHelper(string[] roles)
        {
            definedRoles = roles;
        }

        /// <summary>
        /// List of defined roles.
        /// </summary>
        /// <value>
        /// The defined roles.
        /// </value>
        public IEnumerable<string> DefinedRoles
        {
            get
            {
                return (definedRoles ?? new string[0]).ToArray();
            }
        }

        internal string DefinedRolesSource { get; private set; }

        internal string DefinedRolesErrorSource { get; private set; }

        private string DefinedRolesErrorMessage { get; set; }

        /// <summary>
        /// List of defined environments.
        /// </summary>
        /// <value>
        /// The defined environments.
        /// </value>
        public IEnumerable<string> DefinedEnvironment
        {
            get
            {
                return (definedEnvironment ?? new string[0]).ToArray();
            }
        }

        internal string DefinedEnvironmentSource { get; private set; }

        internal string DefinedEnvironmentErrorSource { get; private set; }

        private string DefinedEnvironmentErrorMessage { get; set; }

        /// <summary>
        /// List of defined environments.
        /// </summary>
        /// <value>
        /// The defined environments.
        /// </value>
        public IEnumerable<string> DefinedScheduleAgent
        {
            get
            {
                return (definedScheduleAgent ?? new string[0]).ToArray();
            }
        }

        internal string DefinedScheduleAgentSource { get; private set; }

        internal string DefinedScheduleAgentErrorSource { get; private set; }

        private string DefinedScheduleAgentErrorMessage { get; set; }

        internal void LoadAppSetting()
        {
            var roleDefine = ConfigurationManager.AppSettings["role:define"];
            if (!string.IsNullOrEmpty(roleDefine))
            {
                DefineRolesOnce(roleDefine, "web.config");
            }
            var envDefine = ConfigurationManager.AppSettings["environment:define"];
            if (!string.IsNullOrEmpty(envDefine))
            {
                DefineEnvironmentOnce(envDefine, "web.config");
            }
            var agentDefine = ConfigurationManager.AppSettings["agent:define"];
            if (!string.IsNullOrEmpty(agentDefine))
            {
                DefineScheduleAgentOnce(agentDefine, "web.config");
            }
        }

        internal void Validate()
        {
            if (string.IsNullOrEmpty(DefinedRolesErrorSource) && string.IsNullOrEmpty(DefinedRolesErrorMessage))
            {
                return;
            }

            throw new ConfigurationErrorsException(DefinedRolesErrorMessage, DefinedRolesErrorSource, 0);
        }

        private void DefineRolesOnce(string value, IXmlNode node)
        {
            Assert.ArgumentNotNull(value, "value");
            Assert.ArgumentNotNull(node, "node");

            var source = (IXmlSource)node;
            var sourceName = source.SourceName;

            DefineRolesOnce(value, sourceName);
        }

        private void DefineRolesOnce(string value, string sourceName)
        {
            Assert.ArgumentNotNull(value, "value");
            Assert.ArgumentNotNull(sourceName, "sourceName");

            if (definedRoles != null && DefinedRolesSource != sourceName)
            {
                DefinedRolesErrorSource = sourceName;
                DefinedRolesErrorMessage = string.Format(
                  "Current set of roles defined in the \"{0}\" file was attempted to be modified in the \"{1}\" file. " +
                  "This is not allowed to prevent unintended configuration changes. " +
                  "If roles from both files are valid, they need to be merged into a single file.",
                  DefinedRolesSource,
                  DefinedRolesErrorSource);

                return;
            }

            var roles = value.Split("|,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
              .Select(x => x.ToLowerInvariant())
              .Distinct()
              .ToList();

            var error = ValidateRoles(roles);
            if (!string.IsNullOrEmpty(error))
            {
                DefinedRolesErrorMessage = error;
                DefinedRolesErrorSource = sourceName;

                return;
            }

            definedRoles = roles.ToArray();
            DefinedRolesSource = sourceName;
        }

        /// <summary>
        /// 定義環境
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sourceName"></param>
        private void DefineEnvironmentOnce(string value, string sourceName)
        {
            Assert.ArgumentNotNull(value, "value");
            Assert.ArgumentNotNull(sourceName, "sourceName");

            if (definedEnvironment != null && DefinedEnvironmentSource != sourceName)
            {
                DefinedEnvironmentErrorSource = sourceName;
                DefinedEnvironmentErrorMessage = string.Format(
                  "Current set of Environment defined in the \"{0}\" file was attempted to be modified in the \"{1}\" file. " +
                  "This is not allowed to prevent unintended configuration changes. " +
                  "If Environment from both files are valid, they need to be merged into a single file.",
                  DefinedEnvironmentSource,
                  DefinedEnvironmentErrorSource);

                return;
            }

            var env = value.Split("|,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
              .Select(x => x.ToUpperInvariant())
              .Distinct()
              .ToList();

            var error = ValidateEnvironment(env);
            if (!string.IsNullOrEmpty(error))
            {
                DefinedEnvironmentErrorMessage = error;
                DefinedEnvironmentErrorSource = sourceName;

                return;
            }

            definedEnvironment = env.ToArray();
            DefinedEnvironmentSource = sourceName;
        }

        /// <summary>
        /// 定義排程
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sourceName"></param>
        private void DefineScheduleAgentOnce(string value, string sourceName)
        {
            Assert.ArgumentNotNull(value, "value");
            Assert.ArgumentNotNull(sourceName, "sourceName");

            if (definedScheduleAgent != null && DefinedScheduleAgentSource != sourceName)
            {
                DefinedScheduleAgentErrorSource = sourceName;
                DefinedScheduleAgentErrorMessage = string.Format(
                  "Current set of ScheduleAgent defined in the \"{0}\" file was attempted to be modified in the \"{1}\" file. " +
                  "This is not allowed to prevent unintended configuration changes. " +
                  "If ScheduleAgent from both files are valid, they need to be merged into a single file.",
                  DefinedScheduleAgentSource,
                  DefinedScheduleAgentErrorSource);

                return;
            }

            var env = value.Split("|,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
              .Select(x => x.ToUpperInvariant())
              .Distinct()
              .ToList();

            var error = ValidateScheduleAgent(env);
            if (!string.IsNullOrEmpty(error))
            {
                DefinedScheduleAgentErrorMessage = error;
                DefinedScheduleAgentErrorSource = sourceName;

                return;
            }

            definedScheduleAgent = env.ToArray();
            DefinedScheduleAgentSource = sourceName;
        }


        internal static string ValidateRoles(ICollection<string> roles)
        {
            return null;
        }

        internal static string ValidateEnvironment(ICollection<string> env)
        {
            string errMsg = string.Empty;
            if (env == null || (env != null && !env.Any()))
            {
                errMsg = "Environment setting in web config is null";
            }

            var _env = env.FirstOrDefault();
            if (_env != "LOCAL" && _env != "DEVELOPMENT" && _env != "STAGING" && _env != "PRODUCTION")
            {
                errMsg = "Environment setting is not correct";
            }

            return errMsg;
        }

        internal static string ValidateScheduleAgent(ICollection<string> roles)
        {
            return null;
        }
    }
}
