namespace Leaf.Authorization
{
    public class TokenAuthenticationOptions
    {
        public string Issuer { get; set; } = "leaf";
        public string Audience { get; set; } = "leaf";
        public string Secret { get; set; } = "__leaf__auth__secret__";
        public int Expires { get; set; } = 120;
    }
}