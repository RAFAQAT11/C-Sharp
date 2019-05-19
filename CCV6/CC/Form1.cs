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
            richTextBox4.Font = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size + 4.019f);
            richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size + 4.019f);
            textBox1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //richTextBox1.SelectionFont = new Font("courier", fontSize, FontStyle.Regular);
            //richTextBox4.SelectionFont = new Font("courier", fontSize, FontStyle.Regular);

            //richTextBox.ScrollBars = textBox1.ScrollBars;
            updateLineNumber();
        }
        int x = 242, y = 0;
        private void updateLineNumber()
        {
            //we get index of first visible char and number of first visible line
            Point pos = new Point(x, y);
            int firstIndex = richTextBox1.GetCharIndexFromPosition(pos);
            int firstLine = richTextBox1.GetLineFromCharIndex(firstIndex);
            //LineNumberTextBox.Height = richTextBox1.Height;

            //now we get index of last visible char and number of last visible line
            pos.X = ClientRectangle.Width;
            pos.Y = ClientRectangle.Height;
            int lastIndex = richTextBox1.GetCharIndexFromPosition(pos);
            int lastLine = richTextBox1.GetLineFromCharIndex(lastIndex);

            //this is point position of last visible char, we'll use its Y value for calculating LineNumberTextBox size
            pos = richTextBox1.GetPositionFromCharIndex(lastIndex);


            //finally, renumber label
            //if (richTextBox4.Text == "" && firstLine == 1)
            //    firstLine = 0;
            richTextBox4.Text = "";
            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                richTextBox4.Text += i + 1 + "\n";
            }

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
            richTextBox5.Text = "";
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
        SyntaxAnalyzer syntax;
        private void button2_Click(object sender, EventArgs e)
        {
            syntax = new SyntaxAnalyzer(arr,richTextBox5,dataGridView1,dataGridView3);
            bool ret = syntax.Namespace_ST();
            bool result = arr[syntax.i].clss == "END" ? true : false;
            if (ret && result)
                richTextBox3.Text = "Successfully Parsed::" + arr[syntax.i]+":" + arr[syntax.i].clss;
            else
                richTextBox3.Text = "Error  " + arr[syntax.i] + " :"+arr[syntax.i].clss;
            //richTextBox1.SelectionFont = new Font("times", ++fontSize, FontStyle.Bold);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //richTextBox1.SelectionFont = new Font("times", --fontSize, FontStyle.Bold);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            updateLineNumber();
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            int d = richTextBox1.GetPositionFromCharIndex(0).Y % (richTextBox1.Font.Height + 1);
            richTextBox4.Location = new Point(x, d);
            updateLineNumber();
        }
        public string[] NewStr(int start, int end)
        {
            string text = richTextBox1.Text;
            string[] str = new string[2];
            if (text.Length < 1)
                return str;
            for (int i = 0; i < start; i++)
                str[0] += text[i];

            for (int i = end + 1; i < text.Length; i++)
                str[1] += text[i];

            return str;
        }
        public string CountTabs(int selection)
        {
            int i = selection;
            int tabs = 0;
            string str = "";
            for (; i < richTextBox1.TextLength; i++)
            {
                if (richTextBox1.Text[i] == '{')
                    tabs--;
                else if (richTextBox1.Text[i] == '}')
                    tabs++;
            }
            for (int j = 0; j < tabs; j++)
                str += "\t";
            return str;
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)'{' || e.KeyChar == (char)'[' || e.KeyChar == (char)'(')
            {
                int i = richTextBox1.SelectionStart;
                string[] text = NewStr(i, i - 1);
                string @Char = "";
                if (e.KeyChar == (char)'(')
                    @Char = ")";
                else if (e.KeyChar == (char)'[')
                    @Char = "]";
                else if (e.KeyChar == (char)'{')
                    @Char = "}";
                richTextBox1.Text = text[0] + @Char + text[1];
                richTextBox1.SelectionStart = i;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                int i = richTextBox1.SelectionStart;
                if (i < richTextBox1.TextLength && richTextBox1.Text[i] == '}')
                {

                    if (i - 2 > 0 && richTextBox1.Text[i - 2] == '{')
                    {
                        string[] text = NewStr(i - 2, i);
                        string tabs = CountTabs(i + 1);
                        string str = "\n" + tabs + "{\n" + tabs + "\t";
                        int cursor = str.Length - 2;
                        string str2 = "\n" + tabs + "}\n";

                        richTextBox1.Text = text[0] + str + str2 + text[1];
                        richTextBox1.SelectionStart = i + cursor;
                    }
                }
                else
                {
                    string[] text = NewStr(i - 1, i - 1);
                    string tabs = CountTabs(i);
                    string str = "\n" + tabs + "\t";
                    int cursor = str.Length - 2;
                    richTextBox1.Text = text[0] + str + text[1];
                    richTextBox1.SelectionStart = i + cursor;
                }

            }
        }
        public int CountCB(int start, int end)
        {
            int op = 0;
            string text = richTextBox1.Text;
            for (; start < end; start++)
            {
                if (text[start] == '{')
                    op++;
                else if (text[start] == '}')
                    op--;
            }
            return op;
        }
        int dataSetI = 0;
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            DataSet ds = syntax.GetDataSet();
            if (dataSetI > 0)
                dataSetI--;
            label4.Text = ds.Tables[dataSetI].TableName.ToString();
            dataGridView2.DataSource = ds.Tables[dataSetI];
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DataSet ds = syntax.GetDataSet();
            if (dataSetI < ds.Tables.Count-1)
                dataSetI++;
            label4.Text = ds.Tables[dataSetI].TableName.ToString();
            dataGridView2.DataSource = ds.Tables[dataSetI];
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
