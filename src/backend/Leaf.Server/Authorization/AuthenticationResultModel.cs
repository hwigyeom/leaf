using Newtonsoft.Json;

namespace Leaf.Authorization
{
    public class AuthenticationResultModel
    {
        [JsonProperty("result")] public AuthenticationResultType Result { get; set; }

        [JsonProperty("token")] public string Token { get; set; }
    }
}