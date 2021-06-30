using System.Text.RegularExpressions;

namespace Henry.Twitter.Analyzation.Abstractions
{
    public interface IEmojiScanner
    {
        MatchCollection GetMatches(string text);
    }
}
