using Newtonsoft.Json;

namespace Leaf.Authorization
{
    public class AuthenticationAccountModel
    {
        [JsonProperty("uid")] public string UserId { get; set; }

        [JsonIgnore] public string HashedPassword { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [JsonProperty("cls")] public string UserClass { get; set; }

        [JsonProperty("company")] public string CompanyCode { get; set; }

        [JsonProperty("group")] public string GroupCode { get; set; }

        public bool ChangePassword { get; set; }

        public bool Locked { get; set; }
    }
}