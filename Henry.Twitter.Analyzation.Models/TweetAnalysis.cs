using System;
using System.Collections.Generic;
using System.Linq;

namespace Henry.Twitter.Analyzation.Models
{
    public class TweetAnalysis
    {
        public string Id { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public bool HasUrl { get; private set; }

        public bool HasPhotoUrl { get; private set; }

        public IReadOnlyCollection<string> Domains { get; private set; }

        public IReadOnlyCollection<string> Tags { get; private set; }

        public IReadOnlyDictionary<string, int> Emojis { get; }

        public TweetAnalysis(
            string id,
            DateTimeOffset createdAt,
            IEnumerable<string> domains,
            IEnumerable<string> tags,
            IReadOnlyDictionary<string, int> emojis)
        {
            Id = !string.IsNullOrWhiteSpace(id) ? id :
                throw new ArgumentNullException(nameof(id));
            CreatedAt = createdAt;
            Domains = new HashSet<string>(domains);
            Tags = new HashSet<string>(tags);
            HasUrl = Domains.Any();
            HasPhotoUrl = Domains.Contains("pic.twitter.com") || domains.Any(x => x.EndsWith("instagram.com", true, null));
            Emojis = emojis ?? throw new ArgumentNullException(nameof(emojis));
        }
    }
}
