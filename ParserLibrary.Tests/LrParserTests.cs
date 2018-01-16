using System;
using System.Collections.Generic;
using NUnit.Framework;
using ParserLibrary.Parsers.Lr;
using ParserLibrary.Types;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class LrParserTests
    {
        private LrParser parser;
        private GrammarLoader grammarLoader;
        private int[,] ruleTable;
        TextParser textParser;

        public LrParserTests()
        {
            grammarLoader = new GrammarLoader();
            ParserLoader loader = new ParserLoader("words.txt", "rules.txt", "table.txt");
            ruleTable = loader.LoadTable("table.txt");
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
            
            textParser = new TextParser(grammarLoader);
        }
        
        [Test]
        public void CompileTestReturnStateTest()
        {
            parser = new LrParser(grammarLoader.Words, grammarLoader.Rules, ruleTable, grammarLoader.CountOfRules);

            string textToParse = "= k 12 if := num 10 = 3 3 if := id 6";
            
            Queue<Word> actualWords = textParser.Parse(textToParse);

            Assert.True(parser.TryCompile(actualWords));
        }
        
        [Test]
        public void CompileTest1()
        {
            parser = new LrParser(grammarLoader.Words, grammarLoader.Rules, ruleTable, grammarLoader.CountOfRules);

            string textToParse = "= 13 12 if := num 10 = 3 3 if := id 6";
            
            Queue<Word> actualWords = textParser.Parse(textToParse);

            if (parser.TryCompile(actualWords))
            {
                string expectedString = String.Format(
                    "{0}{1}{2}", "if ( 13 == 12 ) { num = 10; }", 
                    Environment.NewLine, "if ( 3 == 3 ) { id = 6; }");
                
                Assert.AreEqual(expectedString,parser.CompiledText);
            }
            else
            {
                Assert.Fail();
            }
            
        }
        
        [Test]
        public void CompileTest2()
        {
            parser = new LrParser(grammarLoader.Words, grammarLoader.Rules, ruleTable, grammarLoader.CountOfRules);

            string textToParse = "= m 45 if := num 10 = 3 3 if := k 6";
            
            Queue<Word> actualWords = textParser.Parse(textToParse);

            if (parser.TryCompile(actualWords))
            {
                string expectedString = String.Format(
                    "{0}{1}{2}", "if ( m == 45 ) { num = 10; }", 
                    Environment.NewLine, "if ( 3 == 3 ) { k = 6; }");
                
                Assert.AreEqual(expectedString,parser.CompiledText);
            }
            else
            {
                Assert.Fail();
            }
            
        }
    }
}