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
        string[] DT = { "int", "float", "char", "string" };
        string[] AM = { "public", "private", "protected", "internal" };
        string[] VO = { "virtual", "override"};
        string[] remainingKW = { "abstract", "static", "interface", "base", "const", "namespace", "break", "new", "continue", "do", "return", "else", "this", "if", "while", "class", "for", "void"};



        public string IsKeyword(string word)
        {
            for (int i = 0; i < DT.Length; i++)
                if (DT[i] == word)
                    return "DT";
            for (int i = 0; i < AM.Length; i++)
                if (AM[i] == word)
                    return "AM";
            if (word == "virtual" || word == "override")
                return "VO";
            for (int i = 0; i < remainingKW.Length; i++)
                if (remainingKW[i] == word)
                    return word;
            return "none";
        }

        public string IsOperator(string word)
        {
            if (word == "++" || word == "--")
                return "op1";
            if (word == "!")
                return "op2";
            if (word == "*" || word == "/" || word == "%")
                return "op3";
            if (word == "+" || word == "-")
                return "op4";
            if (word == ">" || word == "<" || word == ">=" || word == "<=" || word == "==" || word == "!=")
                return "op5";
            if (word == "&&")
                return "op6";
            if (word == "||")
                return "op7";
            if (word == "=" || word == "-=" || word == "+=" || word == "/=" || word == "*=" || word == "%=")
                return "ASSIGN-OPT";
            return "none";
        }

        public string IsPunctuator(string word)
        {
            if (word == "{")
                return "CO";
            if (word == "}")
                return "CC";
            if (word == "[")
                return "LO";
            if (word == "]")
                return "LC";
            if (word == "(")
                return "SO";
            if (word == ")")
                return "SC";
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
