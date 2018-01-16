using System.Collections.Generic;
using NUnit.Framework;
using ParserLibrary.Parsers.Lr;
using ParserLibrary.Types;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class TextParserTests
    {
        [Test]
        public void StringParseTest()
        {
            GrammarLoader grammarLoader = new GrammarLoader();
            grammarLoader = grammarLoader
                .SetWord("S")
                .SetWord("C")
                .SetWord("W")
                .SetWord("X")
                .SetWord("A")
                .SetWord("L")
                .SetWord("R")
                .SetWord("if")
                .SetWord(":=")
                .SetWord("id")
                .SetWord("[")
                .SetWord("]")
                .SetWord("const")
                .SetWord("sqr")
                .SetWord("=")
                .SetWord("!")
                .SetWord(">")
                .SetWord("<")
                .SetWord("-")
                .SetWord("$");

            grammarLoader = grammarLoader
                .SetRule("S -> C if W")
                .SetRule("S -> C if W S")
                .SetRule("C -> L A")
                .SetRule("W -> X A")
                .SetRule("W -> X L")
                .SetRule("X -> := id")
                .SetRule("X -> := id [ A ]")
                .SetRule("A -> id")
                .SetRule("A -> const")
                .SetRule("A -> id [ A ]")
                .SetRule("A -> sqr A")
                .SetRule("A -> R A")
                .SetRule("L -> = A")
                .SetRule("L -> ! A")
                .SetRule("L -> > A")
                .SetRule("L -> < A")
                .SetRule("R -> - A");
            
            

            ParserLoader loader = new ParserLoader("words.txt", "rules.txt", "table.txt");

            int[,] table = loader.LoadTable("table.txt");
            
            TextParser parser = new TextParser(grammarLoader);

            string textToParse = "= k 12 if := num 10 = 3 3 if := id 6";

            Queue<Word> actualWords = parser.Parse(textToParse);

            //LrParser analysis = new LrParser(rules, words, table, 7);

            //Queue<Word> wordsActual =  analysis.Up("= k 12 if := num 10 = 3 3 if := id 6");

            Queue<Word> expectedwords = new Queue<Word>(
                new[]
                {
                    new Word(14, "="),
                    new Word(9, "id") { Temp = "k"},
                    new Word(12, "const") {Temp = 12.ToString()},
                    new Word(7, "if"),
                    new Word(8, ":="),
                    new Word(9, "id") {Temp = "num"},
                    new Word(12, "const") {Temp = 10.ToString()},
                    new Word(14, "="),
                    new Word(12, "const") {Temp = 3.ToString()},
                    new Word(12, "const") {Temp = 3.ToString()},
                    new Word(7, "if"),
                    new Word(8, ":="),
                    new Word(9, "id") {Temp = "id"},
                    new Word(12, "const") {Temp = 6.ToString()},
                    new Word(19, "$")
                }
            );

            Assert.AreEqual(expectedwords.Count, actualWords.Count, $"Длинна очередей разная.\nОжидается: {expectedwords.Count}\nТекущая: {actualWords.Count}");

            int count = 0;

            while (expectedwords.Count > 0)
            {
                Word expectedWord = expectedwords.Dequeue();
                Word actualWord = actualWords.Dequeue();
                Assert.AreEqual(expectedWord.Value, actualWord.Value, GetErrorMessage(expectedWord, actualWord, count));
                Assert.AreEqual(expectedWord.Number, actualWord.Number, GetErrorMessage(expectedWord, actualWord, count));
                Assert.AreEqual(expectedWord.Temp, actualWord.Temp, GetErrorMessage(expectedWord, actualWord, count));
                count++;
            }
        }

        private string GetErrorMessage(Word expectedWord, Word actualWord, int index)
        {
            string result = $"Индекс слова: {index}\n";
            result += $"Ожидается:  \nValue: {expectedWord.Value}\n Number: {expectedWord.Number}\n Temp: {expectedWord.Temp}\n";
            result += $"Фактически: \nValue: {actualWord.Value}\n   Number: {actualWord.Number}\n   Temp: {actualWord.Temp}";
            return result;
        }
    }
}