using System;
using System.Collections.Generic;
using System.Text;
using ParserLibrary.Types;

namespace ParserLibrary
{
    public class CompileInfoEventArgs : EventArgs
    {

        public CompileInfoEventArgs(Queue<Word> words, LinkedList<Word> stack, List<int> rules)
        {
            Words = words;
            Rules = rules;
            Stack = stack;
        }

        public readonly Queue<Word> Words;
        public readonly LinkedList<Word> Stack;
        public readonly List<int> Rules;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Входная строка: ");
            foreach (var word in Words)
            {
                sb.Append( $"{word.Value} ");
            }
            sb.AppendLine();
            sb.Append("Магазин: ");
            foreach (var arrVal in Stack)
            {
                sb.Append($"{arrVal.Value} ");
            }
            sb.AppendLine();
            sb.Append("Правила: ");
            foreach (var rule in Rules)
            {
                sb.Append($"{rule} ");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
