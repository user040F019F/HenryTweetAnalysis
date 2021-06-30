using Newtonsoft.Json;

namespace Henry.Twitter.Emoji
{
    class Definition
    {
        [JsonProperty]
        public string Unified { get; set; }

        [JsonProperty("non_qualified")]
        public string NonQualified { get; set; }

        [JsonProperty("has_img_twitter")]
        public bool HasTwitterSupport { get; set; }
    }
}
