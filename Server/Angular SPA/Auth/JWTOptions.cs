namespace AngularSPA.Auth
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        
        public JwtIssuerOptions IssuerOptions { get; set; }
    }
}
