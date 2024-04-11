using Newtonsoft.Json;

namespace Feature.Wealth.Account.Models.OAuth
{
    public class LineTokenResponse
    {

        [JsonProperty("access_token")]
        public string AccessToken
        {
            get;
            set;
        }

        [JsonProperty("expires_in")]
        public long ExpiresIn
        {
            get;
            set;
        }

        [JsonProperty("id_token")]
        public string IdToken
        {
            get;
            set;
        }

        [JsonProperty("refresh_token")]
        public string RefreshToken
        {
            get;
            set;
        }

        [JsonProperty("scope")]
        public string Scope
        {
            get;
            set;
        }

        [JsonProperty("token_type")]
        public string TokenType
        {
            get;
            set;
        }

        [JsonProperty("error")]
        public string Error
        {
            get;
            set;
        }

        [JsonProperty("error_description")]
        public string ErrorDescription
        {
            get;
            set;
        }
    }

    public class LineUser
    {

        [JsonProperty("userId")]
        public string UserId
        {
            get;
            set;
        }

        [JsonProperty("displayName")]
        public string DisplayName
        {
            get;
            set;
        }

        [JsonProperty("pictureUrl")]
        public string PictureUrl
        {
            get;
            set;
        }

        [JsonProperty("statusMessage")]
        public string StatusMessage
        {
            get;
            set;
        }
    }

    public class LineVerify
    {

        [JsonProperty("iss")]
        public string Iss { get; set; }
        [JsonProperty("sub")]
        public string Sub { get; set; }
        [JsonProperty("aud")]
        public string Aud { get; set; }
        [JsonProperty("exp")]
        public string Exp { get; set; }
        [JsonProperty("iat")]
        public string Iat { get; set; }
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
        [JsonProperty("amr")]
        public string[] Amr { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("picture")]
        public string Picture { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
