using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CC
{
    class LaxicalComponents
    {
        string[] DT = { "INT", "FLOAT", "CHAR", "STRING" };
        string[] AM = { "PUBLIC", "PRIVATE", "PROTECTED", "INTERNAL" };
        string[] VO = { "VIRTUAL", "OVERRIDE" };
        string[] remainingKW = { "ABSTRACT", "STATIC", "INTERFACE", "BASE", "CONST", "NAMESPACE", "BREAK", "NEW", "CONTINUE", "DO", "RETURN", "ELSE", "THIS", "IF", "WHILE", "CLASS", "FOR", "VOID" };
        //public LaxicalComponents()
        //{
        //    for (int i = 0; i < remainingKW.Length; i++)
        //    {
        //        if (i < DT.Length)
        //            DT[i] = DT[i].ToUpper();
        //        if (i < AM.Length)
        //            AM[i] = AM[i].ToUpper();
        //        if (i < VO.Length)
        //            VO[i] = VO[i].ToUpper();
        //        remainingKW[i] = remainingKW[i].ToUpper();

        //    }
        //}


        public string IsKeyword(string word)
        {
            for (int i = 0; i < DT.Length; i++)
                if (DT[i].ToLower() == word)
                    return "DT";
            for (int i = 0; i < AM.Length; i++)
                if (AM[i].ToLower() == word)
                    return "AM";
            if (word == "virtual" || word == "override")
                return "VO";
            for (int i = 0; i < remainingKW.Length; i++)
                if (remainingKW[i].ToLower() == word)
                    return remainingKW[i];
            return "none";
        }

        public string IsOperator(string word)
        {
            if (word == "++" || word == "--")
                return "INCDEC";
            if (word == "!")
                return "NOT";
            if (word == "*" || word == "/" || word == "%")
                return "MDM";
            if (word == "+" || word == "-")
                return "PM";
            if (word == ">" || word == "<" || word == ">=" || word == "<=" || word == "==" || word == "!=")
                return "RO";
            if (word == "&&")
                return "AND";
            if (word == "||")
                return "OR";
            if (word == "=" || word == "-=" || word == "+=" || word == "/=" || word == "*=" || word == "%=")
                return "ASSIGN-OPT";
            return "none";
        }

        public string IsPunctuator(string word)
        {
            if (word == "{")
                return "OCB";
            if (word == "}")
                return "CCB";
            if (word == "[")
                return "OLB";
            if (word == "]")
                return "CLB";
            if (word == "(")
                return "OSB";
            if (word == ")")
                return "CSB";
            if (word == ".")
                return "DOT";
            if (word == ",")
                return "COMMA";
            if (word == ":")
                return "COLON";
            if (word == ";")
                return "TERMINATOR";
            return "none";
        }

        public bool IsID(string word)
        {
            string pattern = @"^[_@a-zA-Z][_a-zA-Z0-9]*$";
            Regex re = new Regex(pattern);
            if (re.IsMatch(word))
                return true;
            return false;
        }
        public bool IsIntConst(string word)
        {
            string pattern = @"^[+-]?[0-9]+$";
            Regex re = new Regex(pattern);
            if (re.IsMatch(word))
                return true;
            return false;
        }
        public bool IsFloatConst(string word)
        {
            string pattern = @"^[+-]?[0-9]*(?:\.[0-9]+)?$";
            Regex re = new Regex(pattern);
            if (re.IsMatch(word))
                return true;
            return false;
        }
        public bool IsCharConst(string word)
        {
            if (word.Length == 4)
                if (word[0] == '\'' && word[3] == '\'')
                    if (word[1] == '\\')
                        if (word[2] == 'n' || word[2] == 'r' || word[2] == 't' || word[2] == '\'' || word[2] == '\"' || word[2] == '.' || word[2] == 'a' || word[2] == '\\')
                            return true;
            if (word.Length == 3)
                if (word[0] == '\'' && word[2] == '\'')
                    if (word[1] == '\\')
                        return false;
                    else
                        return true;
            return false;
        }
        public bool IsStringConst(string word)
        {
            if (word[0] != '\"' || word[word.Length-1] != '\"')
                return false;
            if (word == "\"")
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == '\\')
                    if (word[i + 1] == 'n' || word[i + 1] == 'r' || word[i + 1] == 't' || (2+i< word.Length && word[i + 1] == '\"') || word[i + 1] == '\'' || word[i + 1] == '.' || word[i + 1] == 'a' || word[i + 1] == '\\')
                    {
                        i++;
                        continue;
                    }
                    else return false;
            }
            return true;
        }

        public string Analyze(string word)
        {
            if (IsKeyword(word) != "none")
            {
                return IsKeyword(word);
            }
            if (IsPunctuator(word) != "none")
            {
                return IsPunctuator(word);
            }
            if (IsOperator(word) != "none")
            {
                return IsOperator(word);
            }
            if (IsID(word))
            {
                return "ID";
            }
            if (IsIntConst(word))
            {
                return "INT_CONST";
            }
            if (IsFloatConst(word))
            {
                return "FLOAT_CONST";
            }
            if (IsCharConst(word))
            {
                return "CHAR_CONST";
            }
            if (IsStringConst(word))
            {
                return "STRING_CONST";
            }
            if (word == "$")
                return "END";
            return "error";
        }
    }
}
