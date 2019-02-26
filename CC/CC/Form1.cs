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
            richTextBox1.SelectionFont = new Font("courier", fontSize, FontStyle.Regular); 
        }


        int line = 1;
        MemoryStorage list;
        string temp = "";
        string text="";
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            list = new MemoryStorage();
            text = richTextBox1.Text;
            for (int i = 0; i < text.Length; i++)
            {
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
                    if (!list.Add(text[i].ToString(), line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                    continue;
                }
                if (i+1<text.Length && text[i] == '.')
                {
                    // null
                    if (WhatIs(temp) == 1)
                    {
                        if (text[i + 1] >= '0' && text[i + 1] <= '9')
                        {
                            temp+=text[i];
                            continue;
                        }
                        if ((text[i+1] <= 'z' && text[i+1] >= 'a') || text[i+1] <= 'Z' && text[i+1] >= 'A')
                        {
                            if (!list.Add(text[i].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                            continue;
                        }
                    }
                    // doted num
                    if (WhatIs(temp) == 3)
                    {
                        MessageBox.Show(temp.ToString());
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        temp = "";
                        if (text[i + 1] >= '0' && text[i + 1] <= '9')
                        {
                            temp += text[i];
                            continue;
                        }
                        if ((text[i + 1] <= 'z' && text[i + 1] >= 'a') || text[i + 1] <= 'Z' && text[i + 1] >= 'A')
                        {
                            if (!list.Add(text[i].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
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
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        temp = "";
                        if (text[i + 1] >= '0' && text[i + 1] <= '9')
                        {
                            temp += text[i];
                            continue;
                        }
                        if ((text[i + 1] <= 'z' && text[i + 1] >= 'a') || text[i + 1] <= 'Z' && text[i + 1] >= 'A')
                        {
                            if (!list.Add(text[i].ToString(), line.ToString()))
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                            continue;
                        }
                        continue;
                    }
                    // unexpected

                    if (!list.Add(temp, line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                    temp = "";
                    if (text[i + 1] >= '0' && text[i + 1] <= '9')
                    {
                        temp += text[i];
                        continue;
                    }
                    if ((text[i + 1] <= 'z' && text[i + 1] >= 'a') || text[i + 1] <= 'Z' && text[i + 1] >= 'A')
                    {
                        if (!list.Add(text[i].ToString(), line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        continue;
                    }
                    continue;
                }
                // char
                if (text[i] == '\'')
                {
                    if (temp != "")
                    {
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        temp = "";
                    }
                    if (text[i + 1] == '\\')
                    {
                        if (!list.Add(text[i].ToString() + text[i+1].ToString() + text[i+2].ToString() + text[i+3].ToString(), line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        i += 4;
                    }
                    else
                    {
                        if (!list.Add(text[i].ToString() + text[i + 1].ToString() + text[i + 2].ToString(), line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        i += 3;
                    }
                }
                // string
                if (text[i] == '\"')
                {
                    if (temp != "")
                    {
                        if (!list.Add(temp, line.ToString()))
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        temp = "";
                    }
                    temp+=text[i++];
                    for ( ; i < text.Length; i++)
                    {
                        if (text[i] != '\n')
                            temp += text[i];
                        if(text[i-1]!='\\' && text[i]=='\"' || text[i]=='\n')
                            break;
                    }
                    i++;
                    if (!list.Add(temp, line.ToString()))
                        textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                    temp = "";
                }
                if( i<text.Length && (text[i] == '+' || text[i] == '-' || text[i] == '/' || text[i] == '*' || text[i] == '%' || text[i] == '&' || text[i] == '|' || text[i] == '<' || text[i] == '>' || text[i] == '!' || text[i] == '='))
                {
                    // & | && || + - ++ -- += -= / % * /= %= *= > < = ! <= >= != ==
                    //comments
                    if (text[i] == '/' && text[i + 1] == '/' || text[i + 1] == '*')
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
                                else if (i < text.Length && text[i] == '*' && text[i + 1] == '/')
                                    break;
                            }
                            richTextBox1.Select(start - 2, ++i+1);
                            richTextBox1.SelectionColor = Color.Green;
                            continue;
                            //if (!list.Add(text[i].ToString(), line.ToString()))
                            //{
                            //    textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
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
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                            temp = "";
                        }
                        if (!list.Add(text[i].ToString()+text[i+1].ToString(), line.ToString()))
                        {
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
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
                                textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                            temp = "";
                        }
                        if (!list.Add(text[i].ToString(), line.ToString()))
                        {
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
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
                            textBox1.Text += "error at line no " + line + " with \'" + temp + "\'" + "\n";
                        temp = "";
                    }
            }
            richTextBox2.Text = "";
            list.GetList().ForEach(data => richTextBox2.Text += data + "\n");
            //richTextBox1.Text = "";
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
            else if (num > 0)
                return 2;
            else if (alpha > 0)
                return 4;
            else
                return 10;
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
