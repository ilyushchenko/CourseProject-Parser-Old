namespace ParserLibrary.Types
{
    /// <summary>
    /// Структура символов грамматики
    /// </summary>
    public class Word
    {
        public Word(int number, string value)
        {
            Number = number;
            Value = value;
            Temp = "";
        }

        public Word(Word wordToCopy)
        {
            Number = wordToCopy.Number;
            Value = wordToCopy.Value;
            Temp = wordToCopy.Temp;
        }

        /// <summary>
        /// Номер слова
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Значение слова
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Дополнительная информация о слове
        /// </summary>
        public string Temp { get; set; }
    }
}
