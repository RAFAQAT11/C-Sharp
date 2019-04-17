using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CC
{
    public partial class Form1 : Form
    {
        int fontSize = 14;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ScrollBars = ScrollBars.Vertical;
            richTextBox1.SelectionFont = new Font("courier", fontSize, FontStyle.Regular); 
        }


        StorageStructure[] arr;
        int line = 1;
        MemoryStorage list;
        string temp = "";
        string text="";
        private void button1_Click(object sender, EventArgs e)
        {
            line = 1;
            textBox1.Text = "";
            list = new MemoryStorage();
            text = richTextBox1.Text;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\t')
                    continue;
                if (i + 1 < text.Length && (text[i] == '+' || text[i] == '-') && text[i + 1] >= '0' && text[i + 1] <= '9')
                    {
                        //MessageBox.Show(text[i].ToString());
                        if (i - 1 >= 0 && (!(text[i - 1] >= '0' && text[i - 1] <= '9') && !(text[i - 1] >= 'a' && text[i - 1] <= 'z' || text[i - 1] >= 'A' && text[i - 1] <= 'Z')) && (i - 2 >= 0 && ((text[i - 1] != '+' && text[i - 2] != '+') && (text[i - 1] != '-' && text[i - 2] != '-'))))
                        {
                            //MessageBox.Show(text[i].ToString() + "0");

                            if (temp != "")
                            {
                                if (!list.Add(temp, line.ToString()))
                                    textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                                temp = "";
                            }
                            temp += text[i];
                            continue;
                        }
                        //if (i - 2 >= 0 && ((text[i - 1] != '+' && text[i - 2] != '+') && (text[i - 1] != '-' && text[i - 2] != '-')))
                        //{
                        //    MessageBox.Show(text[i].ToString()+"++");

                        //    if (temp != "")
                        //    {
                        //        if (!list.Add(temp, line.ToString()))
                        //            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        //        temp = "";
                        //    }
                        //    temp += text[i];
                        //    continue;
                        //}
                    }









                if (temp != "" && (text[i] == ' ' || text[i] == '\n' || text[i] == '\r' ))
                {
                    if (!list.Add(temp, line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'"+"\n";
                    temp = "";
                }
                if (temp != "" && ( text[i] == '(' || text[i] == ')' || text[i] == '{' || text[i] == '}' || text[i] == '[' || text[i] == ']' || text[i] == ',' || text[i] == ':' || text[i] == ';'))
                {
                    if (!list.Add(temp, line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'"+"\n";
                    temp = "";
                }
                if (text[i] == '(' || text[i] == ')' || text[i] == '{' || text[i] == '}' || text[i] == '[' || text[i] == ']' || text[i] == ',' || text[i] == ':' || text[i] == ';')
                {
                    if (temp != "")
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                    temp = "";
                    if (!list.Add(text[i].ToString(), line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                    continue;
                }
                if ( text[i] == '.')
                {
                    // null
                    if (WhatIs(temp) == 1)
                    {
                        if (i+1<text.Length && (i + 1 < text.Length && text[i + 1] >= '0' && text[i + 1] <= '9'))
                        {
                            temp+=text[i];
                            continue;
                        }
                        if (i+1<text.Length && (i + 1 < text.Length && ((text[i + 1] <= 'z' && text[i + 1] >= 'a') || text[i + 1] <= 'Z' && text[i + 1] >= 'A')))
                        {
                            if (!list.Add(text[i].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            continue;
                        }
                    }
                    // doted num
                    if (WhatIs(temp) == 3)
                    {
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        temp = "";
                        if (i+1<text.Length && (text[i + 1] >= '0' && text[i + 1] <= '9'))
                        {
                            temp += text[i];
                            continue;
                        }
                        if (i+1<text.Length &&((text[i + 1] <= 'z' && text[i + 1] >= 'a') || text[i + 1] <= 'Z' && text[i + 1] >= 'A'))
                        {
                            if (!list.Add(text[i].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            continue;
                        }
                        continue;
                    }
                    // num
                    if (WhatIs(temp) == 2)
                    {
                        temp += text[i];
                        continue;
                    }
                    // word
                    if (WhatIs(temp) == 4)
                    {
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        temp = "";
                        if (i+1<text.Length && (text[i + 1] >= '0' && text[i + 1] <= '9'))
                        {
                            temp += text[i];
                            continue;
                        }
                        if (i+1<text.Length && ((text[i + 1] <= 'z' && text[i + 1] >= 'a') || text[i + 1] <= 'Z' && text[i + 1] >= 'A'))
                        {
                            if (!list.Add(text[i].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            continue;
                        }
                        else
                        {
                            if (!list.Add(text[i].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        }
                        continue;
                    }
                    // unexpected

                    if (!list.Add(text[i].ToString(), line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                    if (temp != "")
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                    temp = "";
                    continue;
                    //if(temp!="")
                    //    if (!list.Add(temp, line.ToString()))
                    //        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                    //temp = "";
                    //if (text[i + 1] >= '0' && text[i + 1] <= '9')
                    //{
                    //    temp += text[i];
                    //    continue;
                    //}
                    //if ((text[i + 1] <= 'z' && text[i + 1] >= 'a') || text[i + 1] <= 'Z' && text[i + 1] >= 'A')
                    //{
                    //    if (!list.Add(text[i].ToString(), line.ToString()))
                    //        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                    //    continue;
                    //}
                    //continue;
                }
                // char
                if (text[i] == '\'')
                {
                    if (temp != "")
                    {
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        temp = "";
                    }
                    if (i + 1 < text.Length && text[i + 1] == '\\')
                    {
                        if (!list.Add(text[i].ToString() + text[i+1].ToString() + text[i+2].ToString() + text[i+3].ToString(), line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + text[i].ToString() + text[i + 1].ToString() + text[i + 2].ToString() + text[i + 3].ToString() + "\'" + "\n";
                        i += 3;
                        continue;
                    }
                    else if (i + 1 < text.Length && text[i + 1] != '\\')
                    {
                        //if (text[i + 1] == '\'')
                        //{
                        //    if (!list.Add(text[i].ToString() + text[i + 1].ToString(), line.ToString()))
                        //    {
                        //        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        //        i += 2;
                        //    }
                        //}
                        if (i + 2 < text.Length )
                        {
                            if (!list.Add(text[i].ToString() + text[i + 1].ToString() + text[i + 2].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + text[i].ToString() + text[i + 1].ToString() + text[i + 2].ToString() + "\'" + "\n";
                                i += 2;
                                continue;
                            
                        }
                        else
                        {
                            if (!list.Add(text[i].ToString() + text[i + 1].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                                i += 2;
                                continue;
                            
                        }
                    }
                    else
                    {
                        if (!list.Add(text[i].ToString(), line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        continue;
                    }
                }
                // string
                if (i<text.Length && text[i] == '\"')
                {
                    if (temp != "")
                    {
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        temp = "";
                    }
                    temp+=text[i++];
                    for ( ; i < text.Length; i++)
                    {
                        if (text[i] != '\n')
                            temp += text[i];
                        if (i + 1 < text.Length && text[i] == '\\' && (text[i + 1] == '\\' || text[i + 1] == '\"'))
                        {
                            temp += text[i + 1];
                            i++;
                            continue;
                        }
                        if (text[i] == '\"' || text[i] == '\n')
                            break;
                    }
                    if (i<text.Length && text[i]=='\"')
                        i++;
                    if (!list.Add(temp, line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                    temp = "";
                }
                if( i<text.Length && (text[i] == '+' || text[i] == '-' || text[i] == '/' || text[i] == '*' || text[i] == '%' || text[i] == '&' || text[i] == '|' || text[i] == '<' || text[i] == '>' || text[i] == '!' || text[i] == '='))
                {
                    // & | && || + - ++ -- += -= / % * /= %= *= > < = ! <= >= != ==
                    //comments
                    
                    if (i + 1 < text.Length && text[i] == '/' && (text[i + 1] == '/' || text[i + 1] == '*'))
                    {
                        if (text[i] == '/' && text[i + 1] == '/')
                        {
                            i += 2;
                            int start = i;
                            for (; i < text.Length; i++)
                                if (text[i] == '\n' && i < text.Length)
                                    break;
                            richTextBox1.Select(start-2, i);
                            richTextBox1.SelectionColor = Color.Green;
                        }
                        else
                        {
                            i += 2;
                            int start = i;
                            for (; i < text.Length; i++)
                            {
                                if (text[i] == '\n' && i < text.Length)
                                    line++;
                                if (i+1 < text.Length && text[i] == '*' && text[i + 1] == '/')
                                    break;
                            }
                            int j = ++i + 1;
                            richTextBox1.Select(start - 2, j);
                            richTextBox1.SelectionColor = Color.Green;
                            continue;
                            //if (!list.Add(text[i].ToString(), line.ToString()))
                            //{
                            //    textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            //    continue;
                            //}
                            //continue;
                        }
                    }
                    else if ( i+1<text.Length && ((text[i] == '&' && text[i + 1] == '&') || (text[i] == '|' && text[i + 1] == '|') || (text[i] == '+' && text[i + 1] == '+') || (text[i] == '+' && text[i + 1] == '=') || (text[i] == '-' && text[i + 1] == '-') || (text[i] == '-' && text[i + 1] == '=') || (text[i] == '/' && text[i + 1] == '=') || (text[i] == '%' && text[i + 1] == '=') || (text[i] == '*' && text[i + 1] == '=') || (text[i] == '<' && text[i + 1] == '=') || (text[i] == '>' && text[i + 1] == '=') || (text[i] == '=' && text[i + 1] == '=') || (text[i] == '!' && text[i + 1] == '=')))
                    {
                        if (temp != "")
                        {
                            if (!list.Add(temp, line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            temp = "";
                        }
                        if (!list.Add(text[i].ToString()+text[i+1].ToString(), line.ToString()))
                        {
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            i++;
                            continue;
                        }
                        i++;
                        continue;
                    }
                    else
                    {
                        if (temp != "")
                        {
                            if (!list.Add(temp, line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            temp = "";
                        }
                        if (!list.Add(text[i].ToString(), line.ToString()))
                        {
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                            continue;
                        }
                        continue;
                    }
                }
                
                if (i<text.Length && (text[i] != ' ' && text[i] != '\n' && text[i] != '\r'))
                    temp += text[i];
                if (i < text.Length && text[i] == '\n')
                    line++;
                if (i == text.Length - 1)
                    if (temp != "")
                    {
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\r\n";
                        temp = "";
                    }
            }
            richTextBox2.Text = "";
            list.Add("$",line.ToString());
            list.GetList().ForEach(data => richTextBox2.Text += data + "\n");

            arr = new StorageStructure[list.GetList().Count];
            int ii=0;
            list.GetList().ForEach(data => arr[ii++]=data );
            richTextBox3.Text = "";
            //richTextBox2.Text = arr[0].word;
            //list.ForEach(data => richTextBox1.Text += data + "\n");
            
        }

        int WhatIs(string word)
        {
            if (word == "")
                return 1;
            int num=0, dot=0, alpha=0;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] <= '9' && word[i] >= '0')
                    num++;
                if (word[i] == '.')
                    dot++;
                if ((word[i] <= 'z' && word[i] >= 'a') || word[i] <= 'Z' && word[i] >= 'A')
                    alpha++;
            }
            
            if (num > 0 && dot > 0)
                return 3;
            else if (alpha > 0)
                return 4;
            else if (num > 0)
                return 2;
            else
                return 10;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SyntaxAnalyzer syntax = new SyntaxAnalyzer(arr);
            syntax.Namespace_ST();
            bool result = arr[syntax.i].clss == "END" ? true : false;
            if (result)
                richTextBox3.Text = "Successfully Parsed::" + arr[syntax.i]+":" + arr[syntax.i].clss;
            else
                richTextBox3.Text = "Error  " + arr[syntax.i] + " :"+arr[syntax.i].clss;
            //richTextBox1.SelectionFont = new Font("times", ++fontSize, FontStyle.Bold);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionFont = new Font("times", --fontSize, FontStyle.Bold);
        }
        //string NextWord()
        //{
        //    string temp = "";
        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        if (temp != "" && (text[i] == ' ' || text[i] == '\n' || text[i] == '\r'))
        //        {
        //            return (temp);
        //            temp = "";
        //        }
        //        if (temp != "" && (text[i] == '(' || text[i] == ')' || text[i] == '{' || text[i] == '}' || text[i] == '[' || text[i] == ']' || text[i] == ',' || text[i] == '.' || text[i] == ':' || text[i] == ';'))
        //        {
        //            list.Add(temp);
        //            list.Add(text[i].ToString());
        //            temp = "";
        //            continue;
        //        }
        //        if (text[i] != ' ' && text[i] != '\n' && text[i] != '\r')
        //            temp += text[i];
        //    }
        //}
    }
}
