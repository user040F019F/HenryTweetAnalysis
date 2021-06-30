using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Analyzation.Models;
using Henry.Twitter.Emoji.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Henry.Twitter.Emoji
{
    class Scanner : IEmojiScanner
    {
        readonly Regex Expression;

        public Scanner(
            IOptions<EmojiSettings> options)
        {
            var path = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value.JsonFileLocation;
            var isTwitterAvailableFilter = new Func<JToken, bool>(
                x => x.Value<bool>("has_img_twitter"));
            using var file = File.OpenText(path);
            using var reader = new JsonTextReader(file);
            var definitions = JArray
                .ReadFrom(reader)
                .SelectMany(x => {
                    var results = new[] { x.ToObject<Definition>() }
                        .AsEnumerable();
                    var variations = x["skin_variations"];
                    if (variations != null)
                    {
                        results = results.Concat(variations
                            .ToObject<Dictionary<string, Definition>>()
                            .Values
                            .Where(x => x.HasTwitterSupport));
                    }
                    return results;
                }).ToArray();
            var codes = definitions
                .Where(x => x.HasTwitterSupport)
                .SelectMany(x => new[] { x.Unified, x.NonQualified })
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => $@"\u{x.Replace("-", @"\u")}");
            Expression = new Regex($"[{string.Join('|', codes)}]");
        }

        public MatchCollection GetMatches(string text)
        {
            return Expression.Matches(text);
        }
    }
}
