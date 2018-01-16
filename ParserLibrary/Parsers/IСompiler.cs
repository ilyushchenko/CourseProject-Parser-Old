using ParserLibrary.Types;

namespace ParserLibrary.Parsers
{
    public interface IСompiler
    {
        string Compil(int ruleNumber, Word[] arrSNode);
    }
}