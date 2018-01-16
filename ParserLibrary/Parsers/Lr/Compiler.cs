using System;
using System.Collections.Generic;
using ParserLibrary.Types;

namespace ParserLibrary.Parsers.Lr
{
    public class Compiler : IСompiler
    {
        /// <summary>
        /// Производит компиляцию, на основе цепочки данных в стеке
        /// </summary>
        /// <param name="rule">Испольщуемое правило</param>
        /// <param name="arrSNode">Коллекция символов в стеке</param>
        public string Compil(int ruleNumber, Word[] arrSNode)
        {
            if (ruleNumber == 0)
                return String.Format("{1} ( {2} ) {{ {0}; }}", arrSNode[0].Temp, arrSNode[1].Value, arrSNode[2].Temp);
            if (ruleNumber == 1)
                return String.Format("{2} ( {3} ) {{ {1}; }}{4}{0}", arrSNode[0].Temp, arrSNode[1].Temp, arrSNode[2].Value,arrSNode[3].Temp, Environment.NewLine);
            if (ruleNumber == 2)
                return String.Format("{1} {0}", arrSNode[0].Temp, arrSNode[1].Temp);
            if (ruleNumber == 3)
                return String.Format("{1} {0}", arrSNode[0].Temp, arrSNode[1].Temp);
            //if (ruleNumber == 4)
            //    ArrS[ts1 - 1].Temp = $"{ArrS[ts1 - 1].Temp} {ArrS[ts1].Temp}";
            //if (ruleNumber == 3)
            //ArrS[ts1 - 1].Temp = $"{ArrS[ts1 - 1].Temp} {ArrS[ts1].Temp}";
            if (ruleNumber == 5)
                return String.Format("{0} =", arrSNode[0].Temp);
            //if (ruleNumber == 8)
            //    ArrS[ts1].Temp = ArrS[ts1].Value;
            if (ruleNumber == 7)
                return arrSNode[0].Temp;
            
            if (ruleNumber == 8)
                return arrSNode[0].Temp;
            //if (ruleNumber == 11)
            //    ArrS[ts1 - 1].Temp = "! ( " + ArrS[ts1].Temp + " )";
            //if (ruleNumber == 10)
            //    ArrS[ts1 - 1].Temp = "sqrt ( " + ArrS[ts1].Temp + " )";
            if (ruleNumber == 12)
                return String.Format("{0} ==", arrSNode[0].Temp);

            //if (ruleNumber == 0)
            //    return $"{arrSNode.Next.Value.Value} ( {arrSNode.Value.Temp} ) {{ {arrSNode.Next.Next.Value.Temp}; }}";
            //if (ruleNumber == 1)
            //    return $"{arrSNode.Next.Value.Value} ( {arrSNode.Value.Temp} ) {{ {arrSNode.Next.Next.Value.Temp}; }}{Environment.NewLine}{arrSNode.Next.Next.Next.Value.Temp}";
            //if (ruleNumber == 2)
            //    return $"{arrSNode.Value.Temp} {arrSNode.Next.Value.Temp}";
            //if (ruleNumber == 3)
            //    return $"{arrSNode.Value.Temp} {arrSNode.Next.Value.Temp}";
            //if (ruleNumber == 5)
            //    return $"{arrSNode.Next.Value.Temp} =";
            //if (ruleNumber == 12)
            //    return $"{arrSNode.Value.Temp} ==";
            //if (ruleNumber == 7)
            //    //arrSNode.Value.Temp = $"{arrSNode.Value.Value}";
            //    return arrSNode.Value.Temp;
            //if (ruleNumber == 8)
            //    //arrSNode.Value.Temp = $"{arrSNode.Value.Value}";
            //    return arrSNode.Value.Temp;
            //if (ruleNumber == 9)
            //    return $"{arrSNode.Value.Temp}{arrSNode.Next.Value.Value}{arrSNode.Next.Next.Value.Temp}{arrSNode.Next.Next.Next.Value.Value}";
            //if (ruleNumber == 15)
            //    ArrS[ts1 - 1].Temp = $"{ArrS[ts1].Temp} == ";
            //if (ruleNumber == 16)
            //    ArrS[ts1 - 1].Temp = ArrS[ts1].Temp + " ^ ";
            //if (ruleNumber == 13)
            //    ArrS[ts1 - 1].Temp = ArrS[ts1].Temp + " * ";
            //if (ruleNumber == 14)
            //    ArrS[ts1 - 1].Temp = ArrS[ts1].Temp + " / ";
            return "";
        }
    }
}