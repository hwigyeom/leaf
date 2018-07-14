using Newtonsoft.Json;

namespace Leaf.Authorization
{
    public class PasswordRequestModel
    {
        [JsonProperty("uid")] public string UserId { get; set; }

        [JsonProperty("pwd")] public string Password { get; set; }

        [JsonProperty("opwd")] public string OldPassword { get; set; }
    }
}