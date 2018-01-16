using System.Collections.Generic;
using System.Text;
using ParserLibrary.Types;

namespace ParserLibrary
{
    public class CompileErrorEventArgs : CompileInfoEventArgs
    {
        public CompileErrorEventArgs(Queue<Word> words, LinkedList<Word> stack, List<int> rules, int compileAction) : 
            base(words, stack, rules)
        {
            CompileAction = compileAction;
            Message = "Ошибка, при выполнении восходящего разбора.";
        }

        public CompileErrorEventArgs(Queue<Word> words, LinkedList<Word> stack, List<int> rules, int compileAction, string message) :
            base(words, stack, rules)
        {
            Message = message;
            CompileAction = compileAction;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Message);
            sb.AppendLine(base.ToString());
            sb.AppendLine($"Управляющая таблица: {CompileAction} [{Stack.Last.Value.Number},{Words.Peek().Number}]");
            return sb.ToString();
        }


        public int CompileAction { get; }
        public string Message { get; }
    }
}
