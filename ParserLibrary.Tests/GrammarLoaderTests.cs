using NUnit.Framework;
using ParserLibrary.Parsers.Lr;
using ParserLibrary.Types;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class GrammarLoaderTests
    {
        [Test]
        public void SetRulesTest()
        {
            Rule[] expectedRules =
            {
                new Rule(0, new[] {1, 2, 3}),       // "S -> C if W"
                new Rule(0, new[] {1, 2, 3, 0})     // "S -> C if W S"
            };

            GrammarLoader loader = new GrammarLoader();
            loader = loader.SetRule("S -> C if W").SetRule("S -> C if W S");

            for (int i = 0; i < loader.Rules.Length; i++)
            {
                for (int wordIndex = 0; wordIndex < expectedRules[i].CountOfWords; wordIndex++)
                {
                    Assert.AreEqual(expectedRules[i].RuleList[wordIndex], loader.Rules[i].RuleList[wordIndex]);
                }
            }

            Word[] expectedWords = { new Word(0, "S"), new Word(1, "C"), new Word(2, "if"), new Word(3, "W") };

            for (int ruleIndex = 0; ruleIndex < loader.Rules.Length; ruleIndex++)
            {
                Assert.AreEqual(expectedWords[ruleIndex].Value, loader.Words[ruleIndex].Value);
                Assert.AreEqual(expectedWords[ruleIndex].Number, loader.Words[ruleIndex].Number);
            }
        }

        [Test]
        public void SetRuleTest()
        {
            Rule[] expectedRules =
            {
                new Rule(0, new[] {1, 2, 3}),       // "S -> C if W"
            };

            GrammarLoader loader = new GrammarLoader();
            loader = loader.SetRule("S -> C if W");

            for (int i = 0; i < loader.Rules.Length; i++)
            {
                for (int wordIndex = 0; wordIndex < expectedRules[i].CountOfWords; wordIndex++)
                {
                    Assert.AreEqual(expectedRules[i].RuleList[wordIndex], loader.Rules[i].RuleList[wordIndex]);
                }
            }

            Word[] expectedWords = { new Word(0, "S"), new Word(1, "C"), new Word(2, "if"), new Word(3, "W") };

            for (int ruleIndex = 0; ruleIndex < loader.Rules.Length; ruleIndex++)
            {
                Assert.AreEqual(expectedWords[ruleIndex].Value, loader.Words[ruleIndex].Value);
                Assert.AreEqual(expectedWords[ruleIndex].Number, loader.Words[ruleIndex].Number);
            }
        }

        [Test]
        public void SetWordTest()
        {
            Word[] expectedWords = { new Word(0, "S"), new Word(1, "W") };
            GrammarLoader loader = new GrammarLoader();
            loader = loader.SetWord("S").SetWord("W");

            for (int i = 0; i < loader.Words.Length; i++)
            {
                Assert.AreEqual(expectedWords[i].Value, loader.Words[i].Value);
                Assert.AreEqual(expectedWords[i].Number, loader.Words[i].Number);
            }
        }

        [Test]
        public void SetDuplicatedWordTest()
        {
            Word[] expectedWords = { new Word(0, "S") };
            GrammarLoader loader = new GrammarLoader();
            loader = loader.SetWord("S").SetWord("S");

            for (int i = 0; i < loader.Words.Length; i++)
            {
                Assert.AreEqual(expectedWords[i].Value, loader.Words[i].Value);
                Assert.AreEqual(expectedWords[i].Number, loader.Words[i].Number);
            }
        }

        [Test]
        public void GetNotExistedWordTest()
        {
            GrammarLoader loader = new GrammarLoader();
            loader = loader
                .SetWord("S")
                .SetWord("S")
                .SetWord("C")
                .SetWord("if");

            Word srchWord = loader.GetWord("W");

            Assert.IsNull(srchWord);
        }

        [Test]
        public void CheckCountOfWordsTest()
        {
            GrammarLoader loader = new GrammarLoader();
            loader = loader
                .SetWord("S")
                .SetWord("S")
                .SetWord("C")
                .SetWord("if")
                .SetWord("E")
                .SetWord("if")
                .SetWord("S")
                .SetWord("L");

            Assert.AreEqual(loader.Words.Length, 5);
        }

        [Test]
        public void CheckRulesLoadWithWordsTest()
        {

            Rule[] expectedRules =
            {
                new Rule(0, new[] {1, 7, 2}),         // S-> C if W
                new Rule(0, new[] {1, 7, 2, 0}),      // S-> C if W S
                new Rule(1, new[] {5, 4}),            // C -> L A
                new Rule(2, new[] {3, 4}),            // W -> X A
                new Rule(2, new[] {3, 5}),            // W -> X L
                new Rule(3, new[] {8, 9}),            // X -> := id
                new Rule(3, new[] {8, 9, 10, 4, 11}), // X -> := id [ A ]
                new Rule(4, new[] {9}),               // A -> id
                new Rule(4, new[] {12}),              // A -> const
                new Rule(4, new[] {9, 10, 4, 11}),    // A -> id [ A ]
                new Rule(4, new[] {13, 4}),           // A -> sqr A
                new Rule(4, new[] {6, 4}),            // A -> R A
                new Rule(5, new[] {14, 4}),            // L -> = A
                new Rule(5, new[] {15, 4}),           // L -> ! A
                new Rule(5, new[] {16, 4}),           // L -> > A
                new Rule(5, new[] {17, 4}),           // L -> < A
                new Rule(6, new[] {18, 4}),           // R -> - A
            };

            GrammarLoader loader = new GrammarLoader();
            loader = loader
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

            loader = loader
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

            for (int ruleIndex = 0; ruleIndex < loader.Rules.Length; ruleIndex++)
            {
                Assert.AreEqual(
                    expectedRules[ruleIndex].RuleWordNumber, 
                    loader.Rules[ruleIndex].RuleWordNumber, 
                    $"В правиле {ruleIndex} не сошелся номер правила"
                    );
                for (int i = 0; i < loader.Rules[ruleIndex].RuleList.Length; i++)
                {
                    Assert.AreEqual(
                        loader.Rules[ruleIndex].RuleList[i], 
                        expectedRules[ruleIndex].RuleList[i],
                        $"В правиле {ruleIndex} не сошелся порядок правил по индексу {i}"
                        );
                }
            }
        }

        [Test]
        public void CheckWordsLoadWithRulesTest()
        {
            Word[] expectedWords =
            {
                new Word(0, "S"),
                new Word(1, "C"),
                new Word(2, "W"),
                new Word(3, "X"),
                new Word(4, "A"),
                new Word(5, "L"),
                new Word(6, "R"),
                new Word(7, "if"),
                new Word(8, ":="),
                new Word(9, "id"),
                new Word(10, "["),
                new Word(11, "]"),
                new Word(12, "const"),
                new Word(13, "sqr"),
                new Word(14, "="),
                new Word(15, "!"),
                new Word(16, ">"),
                new Word(17, "<"),
                new Word(18, "-"),
                new Word(19, "$")
            };

            GrammarLoader loader = new GrammarLoader();
            loader = loader
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

            loader = loader
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

            for (int wordIndex = 0; wordIndex < loader.Words.Length; wordIndex++)
            {
                Assert.AreEqual(expectedWords[wordIndex].Value, loader.Words[wordIndex].Value);
                Assert.AreEqual(expectedWords[wordIndex].Number, loader.Words[wordIndex].Number);
                Assert.AreEqual(expectedWords[wordIndex].Temp, loader.Words[wordIndex].Temp);
            }

        }


        [Test]
        public void LoadFromFileTest()
        {
            Word[] expectedWords =
            {
                new Word(0, "S"),
                new Word(1, "C"),
                new Word(2, "W"),
                new Word(3, "X"),
                new Word(4, "A"),
                new Word(5, "L"),
                new Word(6, "R"),
                new Word(7, "if"),
                new Word(8, ":="),
                new Word(9, "id"),
                new Word(10, "["),
                new Word(11, "]"),
                new Word(12, "const"),
                new Word(13, "sqr"),
                new Word(14, "="),
                new Word(15, "!"),
                new Word(16, ">"),
                new Word(17, "<"),
                new Word(18, "-"),
                new Word(19, "$")
            };

            Rule[] expectedRules =
            {
                new Rule(0, new[] {1, 7, 2}),         // S-> C if W
                new Rule(0, new[] {1, 7, 2, 0}),      // S-> C if W S
                new Rule(1, new[] {5, 4}),            // C -> L A
                new Rule(2, new[] {3, 4}),            // W -> X A
                new Rule(2, new[] {3, 5}),            // W -> X L
                new Rule(3, new[] {8, 9}),            // X -> := id
                new Rule(3, new[] {8, 9, 10, 4, 11}), // X -> := id [ A ]
                new Rule(4, new[] {9}),               // A -> id
                new Rule(4, new[] {12}),              // A -> const
                new Rule(4, new[] {9, 10, 4, 11}),    // A -> id [ A ]
                new Rule(4, new[] {13, 4}),           // A -> sqr A
                new Rule(4, new[] {6, 4}),            // A -> R A
                new Rule(5, new[] {14, 4}),            // L -> = A
                new Rule(5, new[] {15, 4}),           // L -> ! A
                new Rule(5, new[] {16, 4}),           // L -> > A
                new Rule(5, new[] {17, 4}),           // L -> < A
                new Rule(6, new[] {18, 4}),           // R -> - A
            };


            GrammarLoader loader = new GrammarLoader("words.txt", "rules.txt");
            

            for (int wordIndex = 0; wordIndex < loader.Words.Length; wordIndex++)
            {
                Assert.AreEqual(expectedWords[wordIndex].Value, loader.Words[wordIndex].Value);
                Assert.AreEqual(expectedWords[wordIndex].Number, loader.Words[wordIndex].Number);
                Assert.AreEqual(expectedWords[wordIndex].Temp, loader.Words[wordIndex].Temp);
            }

            for (int ruleIndex = 0; ruleIndex < loader.Rules.Length; ruleIndex++)
            {
                Assert.AreEqual(
                    expectedRules[ruleIndex].RuleWordNumber,
                    loader.Rules[ruleIndex].RuleWordNumber,
                    $"В правиле {ruleIndex} не сошелся номер правила"
                );
                for (int i = 0; i < loader.Rules[ruleIndex].RuleList.Length; i++)
                {
                    Assert.AreEqual(
                        loader.Rules[ruleIndex].RuleList[i],
                        expectedRules[ruleIndex].RuleList[i],
                        $"В правиле {ruleIndex} не сошелся порядок правил по индексу {i}"
                    );
                }
            }

        }

        [Test]
        public void CheckCountOfRulesWithLoadFromFileTest()
        {
            GrammarLoader loader = new GrammarLoader("words.txt", "rules.txt");

            Assert.AreEqual(loader.CountOfRules, 7);
        }
    }
}