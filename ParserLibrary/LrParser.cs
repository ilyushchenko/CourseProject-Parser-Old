using System.Collections.Generic;
using System.Linq;
using ParserLibrary.Parsers;
using ParserLibrary.Parsers.Lr;
using ParserLibrary.Types;

namespace ParserLibrary
{
    public class LrParser
    {
        public LrParser(ParserLoader loader)
        {
            Words = loader.Words;
            Rules = loader.Rules;
            RuleTable = loader.ControlTable;
            _firstRule = Rules.First();
            _lastWord = Words.Last();
            _compiler = new Compiler();
            _parser = new TextParser(loader.GrammarLoader);
        }

        public LrParser(Word[] words, Rule[] rules, int[,] ruleTable, int countOfRules)
        {
            Words = words;
            Rules = rules;
            RuleTable = ruleTable;
            _compiler = new Compiler();
            _parser = new TextParser(Words, Rules, countOfRules);
            _firstRule = rules.First();
            _lastWord = words.Last();
        }

        /// <summary>
        /// Делегат ошибки компиляции
        /// </summary>
        /// <param name="sendler">Объект, сгенерировавший вызов метода</param>
        /// <param name="e">Дополнительные параметры</param>
        public delegate void CompileErrorDelegate(object sendler, CompileErrorEventArgs e);

        /// <summary>
        /// Информационный делегат компиляции
        /// </summary>
        /// <param name="sendler">Объект, сгенерировавший вызов метода</param>
        /// <param name="e">Дополнительные параметры</param>
        public delegate void CompileInfoDelegate(object sendler, CompileInfoEventArgs e);

        /// <summary>
        /// Событие оштбки компиляции
        /// </summary>
        public event CompileErrorDelegate OnCompileError = (sendler, e) => { };

        /// <summary>
        /// Событие конца компиляции
        /// </summary>
        public event CompileInfoDelegate OnCompileDone = (sendler, e) => { };

        /// <summary>
        /// Событие конца шага компиляции
        /// </summary>
        public event CompileInfoDelegate OnCompileNextStep = (sendler, e) => { };

        /// <summary>
        /// Коллекция правил, для компиляции
        /// </summary>
        public readonly Rule[] Rules;

        /// <summary>
        /// Коллекция символов грамматики, для компиляции
        /// </summary>
        public readonly Word[] Words;

        /// <summary>
        /// Управляющая таблица восходящего разбора
        /// </summary>
        public readonly int[,] RuleTable;

        /// <summary>
        /// Символ конца цепочки
        /// </summary>
        private readonly Word _lastWord;

        /// <summary>
        /// Правило, в которое нужно свернуть цепочку
        /// </summary>
        private Rule _firstRule;

        /// <summary>
        /// Компилятор кода
        /// </summary>
        private readonly IСompiler _compiler;

        /// <summary>
        /// Парсер входной строки
        /// </summary>
        private readonly ITextParser _parser;
        
        public string CompiledText { get; private set; }

        public bool TryCompile(Queue<Word> splittedWords)
        {
            // Коллекций номеров правил
            List<int> rulesFound = new List<int>();
            // Коллекция элементов в магазине
            LinkedList<Word> stack = new LinkedList<Word>();

            stack.AddLast(new Word(_lastWord));

            OnCompileNextStep.Invoke(this, new CompileInfoEventArgs(splittedWords, stack, rulesFound));

            while (splittedWords.Count > 0)
            {
                CompileActions action = CompileActions.Start;
                
                // Серем слово, для просмотра
                Word word = splittedWords.Peek();

                // Проверяем, является ли последний символ символом конца цепочки
                if (word.Number == _lastWord.Number)
                {
                    if (stack.Count == 2)
                    {
                        // Если последний символ в стеке - символ цепочки S, а первый - символ клнца цепочки
                        // Тогда разбор окончен успешно
                        if ((stack.Last.Value.Number == _firstRule.RuleWordNumber) && (stack.First.Value.Number == _lastWord.Number))
                        {
                            CompiledText = stack.Last.Value.Temp;
                            OnCompileDone.Invoke(this, new CompileInfoEventArgs(splittedWords, stack, rulesFound));
                            return true;
                        }
                    }
                }

                int row = stack.Last.Value.Number;
                int col = word.Number;

                // Правило, для переноса
                if ((RuleTable[row, col] == 1 && action != CompileActions.Error) || (RuleTable[row, col] == 2 && action != CompileActions.Error))
                {
                    // Вытаскиваем слово из входной цепочки и заносим в стек
                    Word tmpWord = splittedWords.Dequeue();
                    stack.AddLast(tmpWord);
                    action = CompileActions.Next;
                }

                // Правило, для свертки
                if ((RuleTable[row, col] == 3) && action != CompileActions.Next && action != CompileActions.Error)
                {
                    // Ищем количество слов, для свертки
                    // Изначально, текуший символ не может иметь правило, для сдвига => cnt = 1
                    int cntWordsInStack = 1;

                    // Начинаем поиск с конца цепочки
                    LinkedListNode<Word> node = stack.Last;
                    
                    // Если правило не равняется сдвигу, то сдвигаем цепочку. Увеличиваем число найденных слов
                    while(RuleTable[node.Previous.Value.Number, node.Value.Number] != 1)
                    {
                        node = node.Previous;
                        cntWordsInStack++;
                    }

                    // Ищем подходящее правило
                    for (var ruleNumber = 0; ruleNumber < Rules.Length; ruleNumber++)
                    {
                        // Ищем подходящие по длене правило из списка правил
                        if (Rules[ruleNumber].CountOfWords == cntWordsInStack)
                        {
                            // Количество символов, которые последовательно совпадают в цепочке и правиле
                            int cntWordsInRule = 0;

                            // Присваиваем начальный символ цепочки, для свертки
                            LinkedListNode<Word> srchNode = node;
                            // Можно изменить способ сверки
                            for (var i = 0; i < cntWordsInStack; i++)
                            {
                                // Сверяет последовательность слов в выбранном правиле с цепочкой правил в стеке
                                if (Rules[ruleNumber].RuleList[i] == srchNode.Value.Number)
                                {
                                    cntWordsInRule++;
                                    srchNode = srchNode.Next;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            
                            // Если количество сошлось, то это то правило
                            if (cntWordsInRule == cntWordsInStack)
                            {
                                List<Word> toCompileList = new List<Word>();
                                // Загружаем последний элемент стека
                                srchNode = stack.Last;
                                toCompileList.Add(srchNode.Value);

                                // СмещаемЮ до первого элемента, в цепочке правил
                                for (int i = 1; i < cntWordsInRule; i++)
                                {
                                    srchNode = srchNode.Previous;
                                    toCompileList.Add(srchNode.Value);
                                }


                                // Присваиваем номер и символ правила, которое использовали
                                int collapsedRuleNumber = Rules[ruleNumber].RuleWordNumber;

                                // Производим покпиляцию по правилу
                                node.Value.Temp = _compiler.Compil(ruleNumber, toCompileList.ToArray());
                                node.Value.Number = collapsedRuleNumber;
                                node.Value.Value = Words[collapsedRuleNumber].Value;

                                // Добаляем использованное правило в коллекцию правил
                                rulesFound.Add(ruleNumber);

                                // Очищаем ненужные элементы до конца списка
                                while (node.Next != null)
                                {
                                    stack.Remove(node.Next);
                                }

                                action = CompileActions.Next;
                                break;
                            }
                        }
                    }
                }
                if (action != CompileActions.Next)
                {
                    OnCompileError.Invoke(this, new CompileErrorEventArgs(splittedWords, stack, rulesFound, RuleTable[row, col]));
                    return false;
                }
                OnCompileNextStep.Invoke(this, new CompileInfoEventArgs(splittedWords, stack, rulesFound));
            }
            return false;
        }

        public bool TryCompile(string text)
        {
            var parsedText = _parser.Parse(text);
            if (parsedText != null)
            {
                return TryCompile(parsedText);
            }
            return false;
        }

        public void Run(object obj)
        {
            if (obj is string str)
            {
                TryCompile(str);
            }
        }
    }
}
