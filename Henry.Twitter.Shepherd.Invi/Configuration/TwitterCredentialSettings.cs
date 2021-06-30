using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;

namespace Henry.Twitter.Shepherd.Invi.Configuration
{
    public class TwitterCredentialSettings : IReadOnlyTwitterCredentials
    {
        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string BearerToken { get; private set; }
        public string AccessToken { get; private set; }
        public string AccessTokenSecret { get; private set; }
    }
}
