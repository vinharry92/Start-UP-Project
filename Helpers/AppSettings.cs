using System;
namespace No_Core_Auth.Helpers
{
    public class AppSettings
    {
        //properties for JWT Token
        public string Site { get; set;}
        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }

        // Sendgrid
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
    }
}
