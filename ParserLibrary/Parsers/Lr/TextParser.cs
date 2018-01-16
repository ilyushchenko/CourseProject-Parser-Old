using System;
using System.Collections.Generic;
using System.Linq;
using ParserLibrary.Types;

namespace ParserLibrary.Parsers.Lr
{
    public class TextParser : ITextParser
    {
        public TextParser(Word[] words, Rule[] rules, int countOfRules)
        {
            Words = words;
            Rules = rules;
            CountOfRules = countOfRules;
        }
        public TextParser(GrammarLoader loader)
        {
            Words = loader.Words;
            Rules = loader.Rules;
            CountOfRules = loader.CountOfRules;
        }


        public Word[] Words { get; }
        public Rule[] Rules { get; }
        public int CountOfRules { get; }

        /// <summary>
        /// Парсит входную строку
        /// </summary>
        /// <param name="text">Строка, для парсинга</param>
        /// <returns>Очередь распарщенных слов</returns>
        public Queue<Word> Parse(string text)
        {
            string[] list = text.Split(' ');



            Queue<string> words = new Queue<string>(list);

            //if (words.Contains("const"))
            //{
            //    throw new Exception("const - служебное слово");
            //}

            // Коллекция строк, которые были получены в результате парсинга строки
            Queue<Word> splittedWords = new Queue<Word>();

            while (words.Count > 0)
            {
                string word = words.Dequeue();

                Word foundWord = Words.FirstOrDefault(item =>
                    item.Value == word && item.Number >= CountOfRules && 
                    item.Value != "const" && item.Value != "id");
                
                if (foundWord != null)
                {
                    splittedWords.Enqueue(new Word(foundWord.Number, foundWord.Value));
                }
                else
                {
                    int nuber;
                    //TODO: true или false в качестве const
                    if (int.TryParse(word, out nuber))
                    {
                        //nuber = Convert.ToInt32(word, 8);
                        splittedWords.Enqueue(new Word(12, "const") {Temp = nuber.ToString()});
                    }
                    else
                    {
                        splittedWords.Enqueue(new Word(9, "id") { Temp = word });
                    }
                }      

            }
            
            //TODO: Не забыть поставить символ конца цепочки
            Word endWord = Words.FirstOrDefault(item => item.Value == "$");
            if (endWord != null)
            {
                splittedWords.Enqueue(new Word(endWord.Number, endWord.Value));
            }
            else
            {
                throw new Exception("Символ конца строки не найден");
            }
            
            
            return splittedWords;
        }
    }
}