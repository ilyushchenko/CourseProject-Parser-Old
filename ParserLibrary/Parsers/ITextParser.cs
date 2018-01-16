using System.Collections.Generic;
using ParserLibrary.Types;

namespace ParserLibrary.Parsers
{
    public interface ITextParser
    {
        Word[] Words { get; }
        Rule[] Rules { get; }
        int CountOfRules { get; }
        Queue<Word> Parse(string text);
    }
}