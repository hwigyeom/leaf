using Newtonsoft.Json;

namespace Leaf.Authorization
{
    public class AuthenticationRequestModel
    {
        [JsonProperty("uid")] public string UserId { get; set; }

        [JsonProperty("pwd")] public string Password { get; set; }
    }
}