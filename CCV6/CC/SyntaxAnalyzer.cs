using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CC
{
    class SyntaxAnalyzer
    {
        StorageStructure[] arr;
        public int i;
        public Symentic symentic = new Symentic();
        RichTextBox rtb5;
        DataGridView dgv;
        DataGridView dgv2;
        string args = "";
        string param = "";
        DataTable GCT;
        StackX stack;
        string retTypeCheck = "";        

        public SyntaxAnalyzer(StorageStructure[] arr,RichTextBox rtb5,DataGridView dgv,DataGridView dgv2)
        {
            this.dgv = dgv;
            this.rtb5 = rtb5;
            this.arr = arr;
            this.i = 0;
            this.dgv2 = dgv2;
            stack = new StackX();
        }

        public bool WithID()
        {
            string type = "";
            if (arr[i].clss == "ID")
            {
                type = GETReturnType(arr[i].word);
                //MessageBox.Show("id="+type);
                i++;
                if (List(type))
                {
                    return true;
                }
            }
            return false;
        }
        // -----------------------------------------------LIST START-----------------------------------------------

        public bool List(string type)
        {
            string type2="";
            if (arr[i].clss == "ASSIGN-OPT")
            {
                if (Assign(out type2,type))
                    return true;
            }
            else if (arr[i].clss == "OSB")
            {
                if (FunCall(out type2))
                {
                    //MessageBox.Show("funcall="+type2);
                    if(!IsMatched(type2, type.Substring(0,type.IndexOf(">")))) InsertError(type2+" and "+type.Substring(0,type.IndexOf(">"))+" :Type Mismatched");
                    return true;
                }
            }
            else if (arr[i].clss == "OLB")
            {
                if (Arr(type))
                    return true;
            }
            else if (arr[i].clss == "ID")
            {
                string name = arr[i].word;
                string CN = arr[i-1].word;
                if (OBJ(out type2))
                {
                    if(!IsMatched(CN, type2)) InsertError(type2+" and "+CN+" :Type Mismatched");
                    //MessageBox.Show(type+" "+type2);
                    InsertFT(name,type2);
                    return true;
                }
            }
            else if (arr[i].clss == "DOT")
            {
                if (OBJCall())
                return true;
            }
            else if (arr[i].clss == "INCDEC")
            {
                Compatibility(type,"INCDEC");
                if (INCDEC())
                    return true;
            }

            return false;
        }
        public bool Assign(out string type,string typeO)
        {
            type = "";
            if (arr[i].clss == "ASSIGN-OPT")
            {
                i++;
                if (Values(out type,typeO))
                    return true;
            }
            return false;
        }
        public bool FunCall(out string type)
        {
            type = "";
            if (arr[i].clss == "OSB")
            {
                i++;
                if (Params())
                {
                    type = param;
                    param = "";
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        if (FunList())
                            return true;
                    }
                }
            }
            return false;
        }
        public bool FunList()
        {
            if (arr[i].clss == "OLB" || arr[i].clss == "DOT")
            {
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (FunList2())
                            return true;
                    }
                }
                else if(FunArr())
                {
                    if (Arr4List())
                        return true;
                        
                }
                return false;
            }
            else
            {
                if (arr[i].clss == "TERMINATOR")
                    return true;
            }
            return false;
        }
        public bool FunList2()
        {
            string type = "";
            if (FunCall(out type))
                return true;
            else if (Assign(out type,""))
                return true;
            else if (OBJCall())
                return true;
            else if (Arr4())
                return true;
            else if (INCDEC())
                return true;

            return false;
        }
        public bool Arr(string type)
        {
            //type = ""; 
            if (arr[i].clss == "OLB")
            {
                i++;
                if (ArrList(type))
                    return true;
            }
            return false;
        }
        public bool ArrList(string type)
        {
            string type2 = "";
            if (arr[i].clss == "CLB")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    string name = arr[i].word;
                    i++;
                    if (AllInit(out type2,type))
                    {
                        InsertFT(name, type + "[]", type2);
                        return true;
                    }
                }
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if (arr[i].clss == "CLB")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        string name = arr[i].word;
                        i++;
                        if (AllInit(out type2,type))
                        {
                            InsertFT(name, type + "[,]", type2);
                            return true;
                        }
                    }
                }
            }
            else if (OE(out type2))
            {
                if (!IsMatched(type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2, type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2)) InsertError((type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2) + " and " + (type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2) + " :Type Mismatched");
                if (ArrFunList(type))
                    return true;
            }
            return false;
        }
        public bool ArrFunList(string type)
        {
            string type2 = "";
            if (arr[i].clss == "CLB")
            {
                i++;
                if (ARLF(type)) 
                    return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if(OE(out type))
                {
                    if (!IsMatched(type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2, type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2)) InsertError((type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2) + " and " + (type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")).ToLower() : type2) + " :Type Mismatched");
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        if (ARLF(type))
                            return true;
                    }
                }
            }
            return false;
        }
        public bool ARLF(string type)
        {
            string type2 = "";
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (DOTList())
                        return true;
                }
            }
            else if (Assign(out type2,type))
            {
                if (!IsMatched(type.Contains("[") ? type.Substring(0, type.IndexOf("[")) : type, type2)) InsertError((type.Contains("[") ? type.Substring(0, type.IndexOf("[")) : type) + " and " + type2 + ":Type Mismatched");
                return true;
            }
            else if (INCDEC())
            {
                Compatibility(type.Contains("[") ? type.Substring(0, type.IndexOf("[")) : (type.Contains("_")?type.Substring(0,type.IndexOf("_")).ToLower():type), "INCDEC");
                return true;
            }
            return false;
        }
        public bool DOTList()
        {
            string type = "";
            if (FunCall(out type))
                return true;
            else if (OBJCall())
                return true;
            else if (Arr4())
                return true;
            else if (Assign(out type,""))
                return true;
            else if (INCDEC())
                return true;
            return false;
        }
        public bool Arr2()
        {
            string type = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if(OE(out type))
                    if (Arr2List())
                        return true;
            }
            return false;
        }
        public bool Arr2List()
        {
            string type = "";
            if (arr[i].clss == "CLB")
            {
                i++;
                if (Arr2CL())
                    return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if(OE(out type))
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        if (Arr2CL())
                            return true;
                    }
            }
                
            return false;
        }
        public bool Arr2CL()
        {
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (Arr2CLList())
                        return true;
                }
                return false;
            }
            if (INCDEC())
                return true;
            if (Assign2())
                return true;
            if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool Arr2CLList()
        {
            string type = "";
            if (FunCall(out type))
                return true;
            else if (Arr(type))
                return true;
            else if (OBJCall())
                return true;
            if (INCDEC())
                return true;
            return false;
        }
        public bool Assign2()
        {
            string type = "";
            if (Assign(out type,""))
                return true;
            if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool AllInit(out string type,string typeO)
        {
            type = "";
            if (arr[i].clss == "ASSIGN-OPT")
            {
                if (Assign(out type,typeO))
                    return true;
                i--;
            }
            else
            {
                if (arr[i].clss == "TERMINATOR" || arr[i].clss == "COMMA" || arr[i].clss == "CSB") // <========================= POSSIBLE PUNGA
                    return true;
            }
            return false;
        }
        public bool OBJ(out string type)
        {
            type = "";
            if (arr[i].clss == "ID")
            {
                i++;
                if (AllInit(out type,""))
                    return true;
            }
            return false;
        }
        public bool OBJCall()
        {
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (OBJCList())
                        return true;
                }
            }
            return false;
        }
        public bool OBJCList()
        {
            string type = "";
            if (arr[i].clss == "DOT")
            {
                if (OBJCall())
                    return true;
            }
            else if (arr[i].clss == "OSB")
            {
                if (FunCall(out type))
                    return true;
            }
            else if(arr[i].clss == "ASSIGN-OPT")
            {
                if (Assign(out type,""))
                    return true;
            }
            else if (arr[i].clss == "OLB")
            {
                if (Arr4())
                    return true;
            }
            else if (arr[i].clss == "INCDEC")
            {
                if (INCDEC())
                    return true;
            }

            return false;
        }
        public bool OBJCList2()
        {
            if (arr[i].clss == "DOT")
            {
                if (OBJCall())
                    return true;
            }
            else
            {
                if (arr[i].clss == "TERMINATOR") //<----------------------------- POSSIBLE PUNGA
                {
                    return true;
                }
            }
            return false ;
        }
        public bool INCDEC()
        {
            if (arr[i].clss == "INCDEC")
            {
                i++;
                return true;
            }
            return false;
        }

        // -----------------------------------------------LIST END-------------------------------------------------

        // -----------------------------------------------VALUES START-------------------------------------------------
        
        public bool Values(out string type, string typeO)
        {
            type = "";
            if (arr[i].clss == "ID" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                if (Init(out type))
                    return true;
            } 
            else if (arr[i].clss == "NEW")
            {
                i++;
                if (ASSList(out type))
                    return true;
            }
            else if (arr[i].clss == "OCB")
            {
                if (BInit(out type,typeO))
                    return true;
            }
            else if (arr[i].clss == "ID")
            {
                i++;
                if (Init2())
                    return true;
            }

            return false;
        }

        // -----------------------------------------------VALUES END-------------------------------------------------

        // -----------------------------------------------OE START-------------------------------------------------
        
        public bool OE(out string type)
        {
            if (AE(out type))
                if (OE_(ref type))
                    return true;
            return false;
        }
        public bool OE_(ref string type)
        {
           
            string type2="";
            if (arr[i].clss == "OR" )
            {
                string opt = arr[i].word;
                i++;
                if (AE(out type2))
                {
                    type = Compatibility(type.Contains("_") ? type.Substring(0, type.IndexOf("_")) : (type.Contains(">") ? type.Substring(type.IndexOf(">") + 1) : type), type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")) : (type2.Contains(">") ? type2.Substring(type2.IndexOf(">") + 1) : type2), opt);
                    if (OE_(ref type))
                        return true;
                }
                return false;
            }
            else if (arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool AE(out string type)
        {
            if (RE(out type))
                if (AE_(ref type))
                    return true;
            return false;
        }
        public bool AE_(ref string type)
        {
            string type2 = "";
            if (arr[i].clss == "AND")
            {
                string opt = arr[i].word;
                i++;
                if (RE(out type2))
                {
                    type = Compatibility(type.Contains("_") ? type.Substring(0, type.IndexOf("_")) : (type.Contains(">") ? type.Substring(type.IndexOf(">") + 1) : type), type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")) : (type2.Contains(">") ? type2.Substring(type2.IndexOf(">") + 1) : type2), opt);
                    if (AE_(ref type))
                        return true;
                }
                return false;
            }
            else if (arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool RE(out string type)
        {
            if (E(out type))
                if (RE_(ref type))
                    return true;
            return false;
        }
        public bool RE_(ref string type)
        {
            string type2 = "";
            if (arr[i].clss == "RO")
            {
                string opt = arr[i].word;
                i++;
                if (E(out type2))
                {
                    type = Compatibility(type.Contains("_") ? type.Substring(0, type.IndexOf("_")) : (type.Contains(">") ? type.Substring(type.IndexOf(">") + 1) : type), type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")) : (type2.Contains(">") ? type2.Substring(type2.IndexOf(">") + 1) : type2), opt);
                    if (RE_(ref type))
                        return true;
                }
                return false;
            }
            else if (arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool E(out string type)
        {
            if (T(out type))
                if (E_(ref type))
                    return true;
            return false;
        }
        public bool E_(ref string type)
        {
            string type2 = "";
            if (arr[i].clss == "PM")
            {
                string opt = arr[i].word;
                i++;
                if (T(out type2))
                {
                    type = Compatibility(type.Contains("_") ? type.Substring(0, type.IndexOf("_")) : (type.Contains(">") ? type.Substring(type.IndexOf(">") + 1) : type), type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")) : (type2.Contains(">") ? type2.Substring(type2.IndexOf(">") + 1) : type2), opt);
                    if (E_(ref type))
                        return true;
                }
                return false;
            }
            else if (arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool T(out string type)
        {
            if (F(out type))
                if (T_(ref type))
                    return true;
            return false;
        }
        public bool T_(ref string type)
        {
            string type2 = "";
            if (arr[i].clss == "MDM")
            {
                string opt = arr[i].word;
                i++;
                if (F(out type2))
                {
                    type = Compatibility(type.Contains("_") ? type.Substring(0, type.IndexOf("_")) : (type.Contains(">") ? type.Substring(type.IndexOf(">") + 1) : type), type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")) : (type2.Contains(">") ? type2.Substring(type2.IndexOf(">") + 1) : type2), opt);
                    if (T_(ref type))
                        return true;
                }
                return false;
            }
            else if (arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool F(out string type)
        {
            string type2 = "";
            type = "";
            if (arr[i].clss == "INCDEC")
            {
                i++;
                if (INCDECList(out type2))
                {
                    type = Compatibility(type2, "INCDEC");
                    return true;
                }
            }
            else if (Const(out type2))
            {
                type = type2;
                return true;
            }
            else if (arr[i].clss == "OSB")
            {
                i++;
                if (OE(out type))
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        return true;
                    }
            }
            else if (arr[i].clss == "NOT")
            {
                i++;
                if (F(out type2))
                {
                    type = Compatibility("NOT", type2);
                    return true;
                }
            }
            else if (arr[i].clss == "ID")
            {
                string name = arr[i].word;
                //GETReturnType(name);
                i++;
                if (OEList(out type2, name))
                {
                    type = type2;
                    return true;
                }
            }
            else if (arr[i].clss == "THIS")
            {
                i++;
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (OEList(out type2, type))
                            return true;
                    }
                }

            }

            return false;
        }
        public bool INCDECList(out string type)
        {
            type = "";
            if (arr[i].clss == "ID")
            {
                type = GETReturnType(arr[i].word);
                i++;
                if (INCCall())
                    return true;
            }
            else if (arr[i].clss == "THIS")
            {
                i++;
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (INCCall())
                            return true;
                    }
                }
            }
            return false;
        }

        public bool INCCall()
        {
            if (arr[i].clss == "OSB" || arr[i].clss == "OLB")
            {
                if (arr[i].clss == "OSB")
                {
                    if (FunCall3())
                        return true;
                }
                else if (arr[i].clss == "OLB")
                {
                    if (Arr3())
                        return true;
                }
            }
            else
            {
                if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
                    return true;
            }
            return false;
        }
        public bool OEList(out string type,string name)
        {
            type = "";
            if (arr[i].clss == "DOT" || arr[i].clss == "OLB" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                if (arr[i].clss == "OSB")
                {
                    if (FunCallOE(out type))
                    {
                        string typeO = GETFuncReturnType(name);
                        if(!IsMatched(typeO.Substring(0,typeO.IndexOf(">")),type)) InsertError((typeO.Substring(0,typeO.IndexOf(">"))+" and "+type+":Type Mismatched"));
                        type = typeO.Substring(typeO.IndexOf(">")+1);
                        return true;
                    }
                }
                else if (arr[i].clss == "DOT")
                {
                    if (OBJCallOE())
                        return true;
                }
                else if (arr[i].clss == "OLB")
                {
                    string typeO = GETReturnType(name);
                    if (ArrOE(out type))
                    {
                        string type2 = typeO.Contains("[")?typeO.Substring(0,typeO.IndexOf("[")):typeO;
                        type = type2 + type;
                        if (!IsMatched(type, typeO)) InsertError(type + " and "+ typeO +"Type Mismatched");
                        return true;
                    }
                }
                else if (arr[i].clss == "INCDEC")
                {
                    string typeO = GETReturnType(name);
                    type = Compatibility(typeO,"INCDEC");
                    if (INCDEC())
                       return true;
                }
            }
            else
            {
                type = GETReturnType(name);
                if (arr[i].clss == "MDM" || arr[i].clss == "CCB" || arr[i].clss == "PM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
                    return true;
            }
            return false;
        }
        public bool FunCallOE(out string type)
        {
            type = "";
            if (arr[i].clss == "OSB")
            {
                i++;
                if (Params())
                    if (arr[i].clss == "CSB")
                    {
                        type = param;
                        param = "";
                        i++;
                        if (FunListOE())
                            return true;
                    }
            }
            return false;
        }
        public bool FunListOE()
        {
            string type = "";
            if (arr[i].clss == "OLB" || arr[i].clss == "DOT")
            {
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (OEList(out type,""))
                            return true;
                    }
                }
                else if (FunArr())
                {
                    if (ArrOEVal())
                        return true;
                        
                }
                return false;
            }
            else
            {
                if (arr[i].clss == "MDM" || arr[i].clss == "CCB" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
                    return true;
            }
            return false;
        }
        //public bool FunListOE2()
        //{
        //    if (FunCallOE())
        //        return true;
        //    else if (OBJCallOE())
        //        return true;
        //    else if (ArrOE())
        //        return true;
        //    else if (INCDEC())
        //        return true;
        //    else if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
        //        return true;
        //    return false;
        //}
        public bool OBJCallOE()
        {
            string type = "";
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (OEList(out type,""))
                        return true;
                }
            }
            return false;
        }
        //public bool OBJOEList()
        //{
        //    if (ArrOE())
        //        return true;
        //    else if (OBJCallOE())
        //        return true;
        //    else if (FunCallOE())
        //        return true;
        //    else if (INCDEC())
        //        return true;
        //    else if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
        //        return true;
        //    return false;
        //}
        public bool ArrOE(out string type)
        {
            type = "";
            string type2 = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                //if (arr[i].clss == "CLB") return false;
                if(OE(out type2))
                {
                    Compatibility(type2,"INCDEC");
                    if (ArrOEList(out type))
                        return true;
                }
            }
            return false;
        }
        public bool ArrOEList(out string type)
        {
            string type2 = "";
            type = "";
            if (arr[i].clss == "CLB")
            {
                type = "[]";
                i++;
                if (ArrOEVal())
                    return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                
                i++;
                if(OE(out type2))
                    if (arr[i].clss == "CLB")
                    {
                        type = "[,]";
                        Compatibility(type2, "INCDEC");
                        i++;
                        if (ArrOEVal())
                            return true;
                    }
            }
            return false;
        }
        public bool ArrOEVal()
        {
            string type = "";
            if (arr[i].clss == "DOT" || arr[i].clss == "INCDEC")
            {
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (OEList(out type,""))
                            return true;
                    }
                }
                else if (INCDEC())
                    return true;
            }
            else
            {
                if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
                    return true;
            }
            return false;
        }

        //public bool ArrOEVal2()
        //{
        //    if (ArrOE())
        //        return true;
        //    else if (OBJCallOE())
        //        return true;
        //    else if (FunCallOE())
        //        return true;
        //    else if (INCDEC())
        //        return true;
        //    else if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
        //        return true;
        //    return false;
        //}

        // -----------------------------------------------OE END-------------------------------------------------



        public bool Init(out string type)
        {
            type = "";
            if (OE2(out type))
                if (Init3())
                    return true;
            return false;
        }
        
        public bool Init3()
        {
            string type = "";
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (Assign(out type,""))
                        return true;
                    return false;
                }
                return false;
            }
            else if(arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }


        public bool Const(out string type)
        {
            type = "";
            if (arr[i].clss == "INT_CONST")
            {
                type = arr[i].clss;
                i++;
                return true;
            }
            if (arr[i].clss == "FLOAT_CONST")
            {
                type = arr[i].clss;
                i++;
                return true;
            }
            if (arr[i].clss == "CHAR_CONST")
            {
                type = arr[i].clss;
                i++;
                return true;
            }
            if (arr[i].clss == "STRING_CONST")
            {
                type = arr[i].clss;
                i++;
                return true;
            }
            return false;
        }
        
        public bool ASSList(out string valType)
        {
            valType = "";
            string isArrOrOBJ = "";
            if (arr[i].clss == "ID" || arr[i].clss == "DT")
            {
                valType = arr[i].word;
                i++;
                if (ASSList2(out isArrOrOBJ,valType))
                {
                    //ConstructValidate(valType,param);
                    //MessageBox.Show("construct = "+valType+" "+param);
                    DataTypeCheck(valType);
                    valType += isArrOrOBJ;
                    return true;
                }
            }
            return false;
        }
        public bool ASSList2(out string isArrOrOBJ,string valType)
        {
            string type = "";
            isArrOrOBJ = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if (OE(out type))
                    if (ASSList3(out isArrOrOBJ))
                        return true;
            }
            else if (arr[i].clss == "OSB")
            {
                isArrOrOBJ = "";
                i++;
                if (Params())
                {
                    ConstructValidate(valType, param);
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        return true;
                    }
                }
            }

            return false;
        }
        public bool Params()
        {
            param = "void";
            //if (!(arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR"  || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR"))
            //    return true;
            if (arr[i].clss == "OCB" || arr[i].clss == "ID" || arr[i].clss == "NEW" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                param = "";
                if (Param2())
                    return true;
            }
            else
            {
                if (arr[i].clss == "CSB")
                    return true;
            }
            return false;
        }
        public bool Param2()
        {
            string type = "";
            string valType = "";
            if (OE(out type))
            {
                if(type.Contains("_"))
                    type = type.Substring(0,type.IndexOf("_")).ToLower();
                param += type;
                if (Param3())
                    return true;
            }
            else if (arr[i].clss == "NEW")
            {
                i++;
                if (ASSList(out valType))
                {
                    param += valType;
                    if (Param3())
                        return true;
                }
            }
            //else if (BInit(out valType))
            //{
            //    if (Param3())
            //        return true;
            //}
            return false;
        }
        public bool Param3()
        {
            if (arr[i].clss == "COMMA")
            {
                param += ",";
                i++;
                if (Param2())
                    return true;
                return false;
            }
            else if (arr[i].clss == "CSB")
                return true;
            return false;
        }
        public bool ASSList3(out string isArrOrOBJ)
        {
            string type = "";
            isArrOrOBJ = "";
            if (arr[i].clss == "CLB")
            {
                isArrOrOBJ = "[]";
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                isArrOrOBJ = "[,]";
                i++;
                if (OE(out type))
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        return true;
                    }
            }

            return false;
        }

        public bool BInit(out string valType,string typeO)
        {
            valType = "";
            if(arr[i].clss == "OCB")
            {
                i++;
                if (BInitList(out valType, typeO))
                {
                    typeO+=valType;
                    valType = typeO;
                    return true;
                }
            }

            return false;
        }
        public bool BInitList(out string valType,string typeO)
        {
            valType = "";
            if (arr[i].clss == "ID" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "CCB" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                valType = "[]";
                if (ArrConst0(typeO))
                {
                    if (arr[i].clss == "CCB")
                    {
                        i++;
                        return true;
                    }
                }
            }
            else if (ArrConst2(typeO))
            {
                valType = "[,]";
                if (arr[i].clss == "CCB")
                {
                    i++;
                    return true;
                }
            }
            
            return false;
        }
        public bool ArrConst0(string typeO)
        {
            if (arr[i].clss == "ID" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                if (ArrConst(typeO))
                    return true;
            } 
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        public bool ArrConst(string typeO)
        {
            string type = "";
            if (OE(out type))
            {
                if(!IsMatched(type,typeO)) InsertError(type+" and "+typeO+":Type Mismatched");;
                //MessageBox.Show(type);
                if (ACL(typeO))
                    return true;
            }
            return false;
        }
        public bool ACL(string typeO)
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (ArrConst(typeO))
                    return true;
                return false;
            }
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        
        public bool ArrConst2(string typeO)
        {
            if (arr[i].clss == "OCB")
            {
                if (ArrConst3(typeO))
                    return true;
            }
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        public bool ArrConst3(string typeO)
        {
            if (arr[i].clss == "OCB")
            {
                i++;
                if(ArrConst0(typeO))
                    if (arr[i].clss == "CCB")
                    {
                        i++;
                        if (ArrConst4(typeO))
                            return true;
                    }
            }
            return false;
        }
        public bool ArrConst4(string typeO)
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (ArrConst3(typeO))
                    return true;
            }
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        public bool Init2()
        {
            string type = "";
            if (Assign(out type,""))
                return true;
            else if (FunCall(out type))
                return true;
            else if (OBJCall())
                return true;
            else if (Arr4())
                return true;
            else if (Init3())
                return true;
            else if (INCDEC())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }

        //public bool FunCall()
        //{
        //    return false;
        //}
        //public bool OBJCall()
        //{
        //    return false;
        //}

        public bool WithDT(out string arg)
        {
            arg = "";
            string isdim = "";
            if (arr[i].clss == "DT")
            {
                arg = arr[i].word;
                i++;
                if (DTList(out isdim,arg))
                {
                    arg += isdim;
                    return true;
                }
            }
            return false;
        }
        //public static virtual int abc() { return 1; }
        public bool DTList(out string isdim,string typeO)
        {
            isdim = "";
            string type = "";
            if (arr[i].clss == "ID")
            {
                string name = arr[i].word;
                i++;
                if (AllInit(out type,typeO))
                {
                    if(!IsMatched(type == "" ? typeO : type, typeO)) InsertError((type == "" ? typeO : type)+" and "+typeO+" :Type Mismatched");
                    InsertFT(name,typeO);
                    return true;
                }
            }
            else if (Arr5(out isdim,typeO))
                return true;
            return false;
        }

        public bool Static_ST(out string TM)
        {
            TM = "None";
            if (arr[i].clss == "STATIC")
            {
                TM = arr[i].word;
                i++;
                return true;
            }
            else if (arr[i].clss == "VO" || arr[i].clss == "DT" || arr[i].clss == "VOID" || arr[i].clss == "ID" || arr[i].clss=="CONST")
                return true;
            return false;
        }
        public bool Const_ST(string AM,string TM,DataTable CT)
        {
            bool isConst = false;
            string DT,name="";
            string type = "";
            if (arr[i].clss == "CONST")
            {
                isConst = true;
                i++;
                if(DT_TypesForConst(out DT))
                    if (arr[i].clss == "ID")
                    {
                        name = arr[i].word;
                        this.TypeIfExistThenInsert(name, DT, AM, TM, isConst, CT);
                        i++;
                        if(AllInit(out type,""))
                            if (arr[i].clss == "TERMINATOR")
                            {
                                i++;
                                return true;
                            }
                    }
            }
            //else if (arr[i].clss == "DT")
            //    return true;
            return false;
        }
        public bool WithStaticConst_DT()
        {
            string DT;
            string TM;
            if (Static_ST(out TM))
                if (Const_ST2())
                {
                    if (WithDT(out DT))
                        return true;
                }
            return false;
        }
        public bool Const_ST2()
        {
            if (arr[i].clss == "CONST")
            {
                    i++;
                    return true;
            }
            else
            {
                if (arr[i].clss == "DT" || arr[i].clss == "ID")
                    return true;
            }
            return false;
        }
        public bool SST()
        {
            if (WithID())
            {
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    return true;
                }
            }
            else if (WithStaticConst_DT())
            {
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    return true;
                }
            }
            else if (For_ST())
                return true;
            else if (DoWhile())
            {
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    return true;
                }
            }
            else if (If_Else())
                return true;
            else if(arr[i].clss == "CONTINUE")
            {
                i++;
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    return true;
                }
            }
            else if (arr[i].clss == "BREAK")
            {
                i++;
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    return true;
                }
            }
            else if (arr[i].clss == "RETURN")
            {
                i++;
                if(Return_ST())
                    if (arr[i].clss == "TERMINATOR")
                    {
                        i++;
                        return true;
                    }
            }
            else if (arr[i].clss == "THIS")
            {
                i++;
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if(WithID())
                        if (arr[i].clss == "TERMINATOR")
                        {
                            i++;
                            return true;
                        }
                }
            }
            
            return false;
        }
        public bool For_ST()
        {
            if (arr[i].clss == "FOR")
            {
                i++;
                if (arr[i].clss == "OSB")
                {
                    stack.Push();
                    i++;
                    if(C1())
                        if (arr[i].clss == "TERMINATOR")
                        {
                            i++;
                            if(C2())
                                if (arr[i].clss == "TERMINATOR")
                                {
                                    i++;
                                    if(C3())
                                        if (arr[i].clss == "CSB")
                                        {
                                            i++;
                                            if ( CheckTerminator() || Body())
                                            {
                                                stack.Pop();
                                                return true;
                                            }
                                        }
                                }
                        }
                }
            }
            return false;
        }

        public bool CheckTerminator()
        {
            if (arr[i].clss == "TERMINATOR")
            {
                i++;
                return true;
            }
            return false;
        }
        public bool C1()
        {
            string type="";
            if (OE2(out type))
            {
                Compatibility(type,"INCDEC");
                return true;
            }
            else if (WithDT(out type))
                return true;
            //else if (WithID())
            //    return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool C2()
        {
            string type = "";
            if (OE(out type))
            {
                if (type.ToLower() != "bool") InsertError("Condition is't bool");
                return true;
            }
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool C3()
        {
            string type = "";
            if (OE2(out type))
            {
                Compatibility(type,"INCDEC");
                return true;
            }
            //else if (WithDT())
            //    return true;
            else if (arr[i].clss == "CSB")
                return true;
            return false;
        }
        public bool DoWhile()
        {
            string type = "";
            if (arr[i].clss == "DO")
            {
                i++;
                if (arr[i].clss == "OCB")
                {
                    stack.Push();
                    i++;
                    if (MST())
                        if (arr[i].clss == "CCB")
                        {
                            stack.Pop();
                            i++;
                            if (arr[i].clss == "WHILE")
                            {
                                i++;
                                if (arr[i].clss == "OSB")
                                {
                                    i++;
                                    if (OE(out type))
                                    {
                                        if (type.ToLower() != "bool") InsertError(type+":type is't bool");
                                        if (arr[i].clss == "CSB")
                                        {
                                            i++;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                }
            }
            return false;
        }
        public bool Return_ST()
        {
            string type = "";
            if (OE(out type))
            {
                if(!IsMatched(type, retTypeCheck)) InsertError(type+" and "+retTypeCheck+"Type Mismatched");
                if (retTypeCheck == "void")
                    InsertError("function does't contain return type (void).");
                retTypeCheck = "";
                return true;
            }
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool If_Else()
        {
            string type = "";
            if(arr[i].clss == "IF")
            {
                i++;
                if (arr[i].clss == "OSB")
                {
                    i++;
                    if(OE(out type))
                    {
                        if (type.ToLower() != "bool") InsertError(type+": Condition is not bool");
                        if (arr[i].clss == "CSB")
                        {
                            i++;
                            stack.Push();
                            if (Body())
                            {
                                stack.Pop();
                                if (If_List())
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool If_List()
        {
            if (arr[i].clss == "ELSE")
            {
                i++;
                stack.Push();
                if (Body())
                {
                    stack.Pop();
                    return true;
                }
                return false;
            }
            else if(arr[i].clss == "CCB" || arr[i].clss == "CONST" || arr[i].clss == "STATIC" || arr[i].clss == "ID" || arr[i].clss == "DT" || arr[i].clss == "FOR" || arr[i].clss == "DO" || arr[i].clss == "CONTINUE" || arr[i].clss == "BREAK" || arr[i].clss == "RETURN" || arr[i].clss == "IF" || arr[i].clss == "THIS")
                return true;
            return false;
        }
        public bool Body()
        {
            if (SST())
                return true;
            else if (arr[i].clss == "OCB")
            {
                i++;
                if(MST())
                    if (arr[i].clss == "CCB")
                    {
                        i++;
                        return true;
                    }
            }
            return false;
        }
        public bool MST()
        {
            if (arr[i].clss == "CONST" || arr[i].clss == "STATIC" || arr[i].clss == "ID" || arr[i].clss == "DT" || arr[i].clss == "FOR" || arr[i].clss == "DO" || arr[i].clss == "CONTINUE" || arr[i].clss == "BREAK" || arr[i].clss == "RETURN" || arr[i].clss == "IF" || arr[i].clss == "THIS")
            {
                if (SST())
                    if (MST())
                        return true;
            }
            else if(arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool Method(string name,string AM,string TM,string DT,DataTable CT)
        {
            if (arr[i].clss == "OSB")
            {
                DataTypeCheck(DT);
                stack.Push();
                i++;
                if (Args())
                    if (arr[i].clss == "CSB")
                    {
                        retTypeCheck = DT;
                        i++;
                        InsertCT(name, (args==""?"void":args) + ">" + DT, AM, TM, false, CT);
                        args = "";
                        if (Super_Class())
                            if (arr[i].clss == "OCB")
                            {
                                i++;
                                if (MST())
                                    if (arr[i].clss == "CCB")
                                    {
                                        if (retTypeCheck != "" && retTypeCheck!="void") InsertError(name+": Function does't contain return statement.");
                                        i++;
                                        return true;
                                    }
                            }
                    }
            }               
            return false;
        }
        public bool VO(string AM,string TM,DataTable CT)
        {
            if (arr[i].clss == "VO")
            {
                i++;
                if(VOList(AM,TM,CT))
                    return true;
            }
            //else if (arr[i].clss == "VOID" || arr[i].clss == "ID" || arr[i].clss == "DT")
            //    return true;
            return false;
        }
        public bool VOList(string AM, string TM, DataTable CT)
        {
            string DT="None";
            if (arr[i].clss == "DT" || arr[i].clss == "VOID")
            {
                if(DT_Types(AM,TM,out DT,CT))
                {
                    if (arr[i].clss == "ID")
                    {
                        string name = arr[i].word;
                        i++;
                        if (Method(name,AM,TM,DT,CT))
                            return true;
                    }
                }
                else if (arr[i].clss == "VOID")
                {
                    DT = arr[i].word;
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        string name = arr[i].word;
                        i++;
                        if (Method(name,AM,TM,DT,CT))
                            return true;
                    }
                }
            }
            return false;
        }
        public bool DT_TypesForConst(out string DT)
        {
            string isDimentinal;
            DT = "";
            if (arr[i].clss == "DT")
            {
                DT = arr[i].word;
                i++;
                if (DT2List(out isDimentinal))
                {
                    DT += isDimentinal;
                    return true;
                }
            }
            if (arr[i].clss == "ID")
            {
                DT = arr[i].word;
                i++;
                if (DT2List(out isDimentinal))
                {
                    DT += isDimentinal;
                    return true;
                }
            }
            return false;
        }
        public bool DT_Types(string AM,string TM, out string DT,DataTable CT)
        {
            string isDimentinal="";
            DT = "";
            if (arr[i].clss == "DT")
            {
                DT = arr[i].word;
                i++;
                if (DT2List(out isDimentinal))
                {
                    DT += isDimentinal;
                    return true;
                }
            }
            if (arr[i].clss == "ID")
            {
                string name = arr[i].word;
                DT = arr[i].word;
                i++;
                if (arr[i].clss == "OSB")
                {
                    if (Construct(name, AM, TM, false, CT))
                        return true;
                }
                else if (arr[i].clss == "OLB")
                {
                    if (DT2List(out isDimentinal))
                    {
                        DT += isDimentinal;
                        return true;
                    }
                }
                else if (arr[i].clss == "ID")
                {
                        return true;
                }
            }
            return false;
        }
        public bool DT2List(out string isDimentional)
        {
            isDimentional = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if (DT2List2(out isDimentional))
                    return true;
            }
            else
            {
                if (arr[i].clss == "ID")
                    return true;
            }
            return false;
        }
        public bool DT2List2(out string isDimentional)
        {
            isDimentional = "";
            if (arr[i].clss == "CLB")
            {
                isDimentional = "[]";
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                isDimentional = "[,]";
                i++;
                if (arr[i].clss == "CLB")
                {
                    i++;
                    return true;
                }
            }
            return false;
        }
        public bool ID_ST()
        {
            if (arr[i].clss == "ID")
            {
                i++;
                if (ID_STList())
                    return true;
            }
            return false;
        }
        public bool ID_STList()
        {
            if (arr[i].clss == "DOT")
            {
                i++;
                if (ID_ST())
                    return true;
            }
            else if(arr[i].clss == "OSB")
                return true;
            return false;
        }
        public bool Super_Class()
        {
            if (arr[i].clss == "COLON")
            {
                i++;
                if (arr[i].clss == "BASE")
                {
                    i++;
                    if (arr[i].clss == "OSB")
                    {
                        i++;
                        if (Params())
                            if (arr[i].clss == "CSB")
                            {
                                i++;
                                return true;
                            }
                    }
                }
                return false;
            }
            else if (arr[i].clss == "OCB")
                return true;
            return false;
        }

        // ========================================================================================   new Arguments
        public bool ArgsDT(out string arg)
        {
            arg = "";
            string isdim = "";
            if (arr[i].clss == "DT")
            {
                arg = arr[i].word;
                i++;
                if (ArgsDTList(out isdim,arg))
                {
                    arg += isdim;
                    return true;
                }
            }
            return false;
        }

        public bool ArgsDTList(out string isdim,string DT)
        {
            isdim = "";
            string name = "";
            if (arr[i].clss == "ID")
            {
                name = arr[i].word;
                i++;
                if (ArgInit(DT,name))
                    return true;
            }
            else if (ArgsArr5(DT,out isdim))
                return true;
            return false;
        }

        public bool ArgInit(string DT,string name)
        {
            string valType="";
            if (arr[i].clss == "ASSIGN-OPT")
            {
                CheckAssign();
                i++;
                if (arr[i].clss == "ID" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
                {
                    if (OE2(out valType))
                        return true;
                }
                else if (arr[i].clss == "NEW")
                {
                    i++;
                    if (ASSList(out valType))
                    {
                        InsertFT(name, DT, valType);
                        return true;
                    }
                }
                else if (arr[i].clss == "OCB")
                {
                    if (BInit(out valType,DT))
                    {
                        if(DT.Contains("["))
                            InsertFT(name, DT, DT.Substring(0,DT.IndexOf('['))+valType);
                        else InsertFT(name, DT, DT + valType);
                        return true;
                    }
                }
            }
            else
            {
                if (arr[i].clss == "CSB" || arr[i].clss == "COMMA")
                {
                    InsertFT(name, DT);
                    return true;
                }
            }
            return false;
        }
        public bool Args()
        {
            //args = "void";
            if (arr[i].clss == "DT" || arr[i].clss == "ID")
            {
                //args = "";
                if (ArgsList())
                return true;
            }
            else 
            {
                if (arr[i].clss == "CSB")
                return true;
            }
            return false;
        }
        public bool ArgsList()
        {
            string arg = "";
            if (ArgsDT(out arg))
            {
                DataTypeCheck(arg);
                args += arg;
                if (Args_ST())
                    return true;
            }
            else if (arr[i].clss == "ID")
            {
                arg = arr[i].word;
                i++;
                string isdim="";
                if (ID3_ST(arg,out isdim))
                {
                    DataTypeCheck(arg + isdim);
                    args += arg + isdim;
                    if (Args_ST())
                        return true;
                }
            }
            return false;
        }
        public bool Args_ST()
        {
            if (arr[i].clss == "COMMA")
            {
                args += ",";
                i++;
                if (Args())
                    return true;
            }
            else if (arr[i].clss == "CSB")
                return true;
            return false;
        }
        public bool ID3_ST(string DT,out string isdim)
        {
            isdim = "";
            if (ArgsOBJ(DT))
                return true;
            else if (ArgsArr5(DT,out isdim))
                return true;
            return false;
        }
        public bool ArgsOBJ(string DT)
        {
            string name="";
            if (arr[i].clss == "ID")
            {
                name = arr[i].word;
                i++;
                if (ArgInit(DT,name))
                    return true;
            }
            return false;
        }
        public bool ArgsArr5(string DT,out string isdim)
        {
            isdim = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if (ArgsArr5List(DT,out isdim))
                    return true;
            }
            return false;
        }
        public bool ArgsArr5List(string DT, out string isdim)
        {
            isdim = "";
            string name = "";
            if (arr[i].clss == "CLB")
            {
                isdim = "[]";
                i++;
                if (arr[i].clss == "ID")
                {
                    name = arr[i].word;
                    i++;
                    if (ArgInit(DT+isdim,name))
                    {

                        return true;
                    }
                }
            }
            else if (arr[i].clss == "COMMA")
            {
                isdim = "[,]";
                i++;
                if (arr[i].clss == "CLB")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        name = arr[i].word;
                        i++;
                        if (ArgInit(DT + isdim, name))
                            return true;
                    }
                }
            }
            return false;
        }

        // ========================================================================================   new Arguments upward

        public bool Arr5(out string isdim,string type)
        {
            isdim = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if (Arr5List(out isdim,type))
                    return true;
            }
            return false;
        }
        public bool Arr5List(out string isdim,string typeO)
        {
            string type = "";
            isdim = "";
            if (arr[i].clss == "CLB")
            {
                isdim = "[]";
                i++;
                if (arr[i].clss == "ID")
                {
                    string name = arr[i].word;
                    i++;
                    if (AllInit(out type,typeO))
                    {
                        if(!IsMatched(typeO + isdim, type == "" ? typeO + isdim : type)) InsertError(typeO + isdim+" and "+(type == "" ? typeO + isdim : type)+":Type Mismatched");
                        InsertFT(name,type);
                        return true;
                    }
                }
            }
            else if (arr[i].clss == "COMMA")
            {
                isdim = "[,]";
                i++;
                if (arr[i].clss == "CLB")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        string name = arr[i].word;
                        i++;
                        if (AllInit(out type,typeO))
                        {
                            if (!IsMatched(typeO + isdim, type == "" ? typeO + isdim : type)) InsertError(typeO + isdim + " and " + (type == "" ? typeO + isdim : type) + ":Type Mismatched"); ;
                            InsertFT(name,type);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool WithAM(DataTable CT)
        {
            string AM;
            if (AM_ST(out AM))
                if (StaticOrVO(AM, CT))
                    return true;
                //if(Static_ST(out TM))
                //    if (AMList(AM,TM,CT))
                //        return true;
            return false;
        }
        public bool StaticOrVO(string AM,DataTable CT)
        {
            string TM = "None";
            if (arr[i].clss == "STATIC")
            {
                TM = arr[i].word;
                i++;
                if (AMList(AM, TM, CT))
                    return true;
            }
            else if (arr[i].clss == "VO")
            {
                TM = arr[i].word;
                if (VO(AM,TM,CT))
                    return true;
            }
            else if (arr[i].clss == "DT" || arr[i].clss == "VOID" || arr[i].clss == "ID" || arr[i].clss == "CONST")
            {
                if (AMList(AM, TM, CT))
                    return true;
            }
            return false;
        }
        public bool AM_ST(out string AM)
        {
            AM = "PRIVATE";
            if (arr[i].clss == "AM")
            {
                AM = arr[i].word;
                i++;
                return true;
            }
            else if (arr[i].clss == "CONST" || arr[i].clss == "STATIC" || arr[i].clss == "VO" || arr[i].clss == "DT" || arr[i].clss == "VOID" || arr[i].clss == "ID" || arr[i].clss == "OLB" || arr[i].clss == "ABSTRACT" || arr[i].clss == "CLASS" || arr[i].clss == "INTERFACE")
                return true;
            return false;
        }
        public bool AMList(string AM, string TM, DataTable CT)
        {
            if (AMList2(AM,TM,CT))
                return true;
            //if (Method())
            //    return true;
            //else if (WithDT())
            //{
            //    if (arr[i].clss == "TERMINATOR")
            //    {
            //        i++;
            //        return true;
            //    }
            //}
            //else if (arr[i].clss == "ID")
            //{
            //    i++;
            //    if (DECList())
            //        if (arr[i].clss == "TERMINATOR")
            //        {
            //            i++;
            //            return true;
            //        }
            //}
            //else if (Construct())
            //    return true;
            else if (NBody())
                return true;
            return false;
        }
        public bool AMList2(string AM, string TM,DataTable CT)
        {
            string DT;
            //if (VO())
            //    return true;
            if (Const_ST(AM,TM,CT))
                return true;
            else if(DT_Types(AM,TM,out DT,CT))
            {
                if (MF(AM,TM,DT,CT))
                    return true;
            }
            else if (arr[i].clss == "VOID")
            {
                DT = arr[i].word;
                i++;
                if (arr[i].clss == "ID")
                {
                    string name = arr[i].word;
                    i++;
                    if (Method(name,AM,TM,DT,CT))
                        return true;
                }
            }
            return false;
        }
        public bool MF(string AM,string TM,string DT,DataTable CT)
        {
            if (arr[i].clss == "ID")
            {
                string name = arr[i].word;
                i++;
                if (AMList3(name, DT, AM, TM, false, CT))
                    return true;
            }
            else
            {
                if (arr[i].clss == "CCB" || arr[i].clss == "AM" || arr[i].clss == "STATIC" || arr[i].clss == "VO" || arr[i].clss == "DT" || arr[i].clss == "VOID" || arr[i].clss == "ID" || arr[i].clss == "ABSTRACT" || arr[i].clss == "CLASS" || arr[i].clss == "INTERFACE")
                    return true;
            }
            return false;
        }
        public bool AMList3(string name,string DT,string AM,string TM,bool isConst,DataTable CT)
        {
            string type = "";
            if (arr[i].clss == "ASSIGN-OPT" || arr[i].clss == "TERMINATOR")
            {
                this.TypeIfExistThenInsert(name, DT, AM, TM, isConst, CT);
                if (AllInit(out type,DT))
                {
                    if (arr[i].clss == "TERMINATOR")
                    {
                        i++;
                        return true;
                    }
                }
            }
            else if (arr[i].clss == "OSB")
            {
                if (Method(name, AM, TM,DT,CT))
                    return true;
            }
            return false;
        }
        public bool DECList()
        {
            string type = "";
            string isdim;
            if (OBJ(out type))
                return true;
            else if (Arr5(out isdim,type))
                return true;
            return false;
        }
        public bool Construct(string name,string AM, string TM,bool isConst,DataTable CT)
        {
            if (arr[i].clss == "OSB")
            {
                i++;
                if (Args())
                    if (arr[i].clss == "CSB")
                    {
                        IfConstructNameRightThenInsert(name, (args == "" ? "void" : args) + ">" + "void", AM, TM, isConst, CT);
                        args = "";
                        i++;
                        if (arr[i].clss == "OCB")
                        {
                            i++;
                            if(MST())
                                if (arr[i].clss == "CCB")
                                {
                                    i++;
                                    return true;
                                }
                        }
                        
                    }
            }
            return false;
        }
        public bool Class_ST()
        {
            string type="",cat,parent="",name="";
            if (Abstract_ST(out cat))
                if (arr[i].clss == "CLASS")
                {
                    DataTable CT = symentic.CreateCT();
                    GCT = CT;
                    type = "CLASS";
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        name = arr[i].word;
                        i++;
                        if (Inherit_ST(out parent))
                            Insert(name, type, cat, parent, CT);
                            if (arr[i].clss == "OCB")
                            {
                                i++;
                                if (ClassList(CT))
                                    if (arr[i].clss == "CCB")
                                    {
                                        i++;
                                        return true;
                                    }
                            }
                    }
                }    
            return false;
        }
        public bool Abstract_ST(out string cat)
        {
            cat = "GENERAL";
            if (arr[i].clss == "ABSTRACT")
            {
                cat = "ABSTRACT";
                i++;
                return true;
            }
            else if (arr[i].clss == "CLASS")
                return true;
            return false;
        }
        public bool Inherit_ST(out string parent)
        {
            parent = "None";
            if (arr[i].clss == "COLON")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    InheritTypeCheck(arr[i].word);
                    parent = arr[i].word;
                    i++;
                    return true;
                }
                return false;
            }
            else if (arr[i].clss == "OCB")
                return true;
            return false;

        }
        public bool ClassList(DataTable CT)
        {
            if(arr[i].clss == "CONST" || arr[i].clss == "AM" || arr[i].clss == "STATIC" || arr[i].clss == "VO" || arr[i].clss == "DT" || arr[i].clss == "VOID" || arr[i].clss == "ID" || arr[i].clss == "ABSTRACT" || arr[i].clss == "CLASS" || arr[i].clss == "INTERFACE")
            {
                if (WithAM(CT))
                {
                    if (ClassList(CT))
                        return true;
                }
            }
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        public bool INTMethod(DataTable CT)
        {
            string DT="",name="";
            if (arr[i].clss == "VOID" || DT_TypesForConst(out DT))
            {
                if (arr[i].clss == "VOID")
                {
                    DT = arr[i].word;
                    i++;
                }
                if (arr[i].clss == "ID")
                {
                    name = arr[i].word;
                    i++;
                    if (arr[i].clss == "OSB")
                    {
                        i++;
                        if(Args())
                            if (arr[i].clss == "CSB")
                            {
                                InsertCT(name, (args==""?"void>"+DT:args+">"+DT), "public", "None", false, CT);
                                i++;
                                return true;
                            }
                    }
                }
            }
            return false;
        }
        public bool Interface_ST()
        {
            string type = "", cat="GENERAL", parent = "None", name = "";
            if (arr[i].clss == "INTERFACE")
            {
                DataTable CT = symentic.CreateCT();
                type = "INTERFACE";
                i++;
                if (arr[i].clss == "ID")
                {
                    name = arr[i].word;
                    Insert(name, type, cat, parent, CT);
                    i++;
                    if (arr[i].clss == "OCB")
                    {
                        i++;
                        if(INTMethod2(CT))
                            if (arr[i].clss == "CCB")
                            {
                                i++;
                                return true;
                            }
                    }
                }
            }
            return false;
        }
        public bool INTMethod2(DataTable CT)
        {
            if (INTMethod(CT))
            {
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    if(INTMethod2(CT))
                        return true;
                }
                return false;
            }
            else if(arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool NBody()
        {
            if (Class_ST())
                return true;
            else if (Interface_ST())
                return true;    
            return false;
        }
        public bool Namespace_ST()
        {
            if (arr[i].clss == "NAMESPACE")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (arr[i].clss == "OCB")
                    {
                        i++;
                        if(NamespaceBody())
                            if (arr[i].clss == "CCB")
                            {
                                i++;
                                dgv.DataSource = symentic.GetTableData1();
                                dgv2.DataSource = symentic.GetFT();
                                return true;
                            }
                    }
                }
            }
            return false;
        }
        public bool NamespaceBody()
        {
            if (arr[i].clss == "AM" || arr[i].clss == "ABSTRACT" || arr[i].clss == "CLASS" || arr[i].clss == "INTERFACE")
            {
                if (WithAMNBody())
                {
                    if (NamespaceBody())
                        return true;
                    return false;
                }
            }
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        public bool WithAMNBody()
        {
            string AM;
            if (AM_ST(out AM))
                if (NBody())
                    return true;
            return false;
        }
        public bool Start()
        {
            if (Namespace_ST())
                return true;
            return false;
        }
        public bool FunCall3()
        {
            if (arr[i].clss == "OSB")
            {
                i++;
                if (Params())
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        if (Fun3List())
                            return true;
                    }
            }
            return false;
        }
        public bool Fun3List()
        {
            if (arr[i].clss == "OLB" || arr[i].clss == "DOT")
            {
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (Chain())
                            return true;
                    }
                }
                else if (FunArr())
                {
                    if (Chain())
                        return true;
                }
                return false;
            }
            return false;
        }
        //public bool Fun3List2()
        //{
        //    if (RList())
        //        return true;
        //    else if (arr[i].clss == "TERMINATOR")
        //        return true;
        //    return false;
        //}
        //public bool RList()
        //{
        //    if (FunCall3())
        //        return true;
        //    else if (Fun3List())
        //        return true;
        //    else if (Arr3())
        //        return true;
        //    return false;
        //}
        public bool Arr3()
        {
            if (Arr8())
                if (Arr3List())
                    return true;
            return false;
        }
        public bool Arr3List()
        {
            if (Fun3List())
                return true;
            else if (arr[i].clss == "TERMINATOR" || arr[i].clss == "COMMA")
                return true;
            return false;
        }
        public bool Arr8()
        {
            string type = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if (OE(out type))
                    if (Arr8List())
                        return true;
            }
            return false;
        }
        public bool Arr8List()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                string type = "";
                i++;
                if (OE(out type))
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        return true;
                    }
            }

            return false;
        }
        public bool Arr4()
        {
            if (Arr8())
                if (Arr4List())
                    return true;
            return false;
        }
        public bool Arr4List()
        {
            string type = "";
            if (arr[i].clss == "DOT" || arr[i].clss == "ASSIGN-OPT" || arr[i].clss == "INCDEC")
            {
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (Arr4DOTList())
                            return true;
                    }
                }
                else if (arr[i].clss == "ASSIGN-OPT")
                {
                    if (Assign(out type,""))
                        return true;
                }
                else if (arr[i].clss == "INCDEC")
                {
                    if (INCDEC())
                        return true;
                }
            }
            else
            {
                if (arr[i].clss == "TERMINATOR")
                    return true;
            }

            return false;
        }
        public bool Arr4DOTList()
        {
            string type = "";
            if (arr[i].clss == "DOT" || arr[i].clss == "ASSIGN-OPT" || arr[i].clss == "INCDEC" || arr[i].clss == "OSB" || arr[i].clss == "OLB")
            {
                if (arr[i].clss == "ASSIGN-OPT")
                {
                    if (Assign(out type,""))
                        return true;
                }
                else if (arr[i].clss == "INCDEC")
                {
                    if (INCDEC())
                        return true;
                }
                else if (arr[i].clss == "OSB")
                {
                    if (FunCall(out type))
                        return true;
                }
                else if (arr[i].clss == "DOT")
                {
                    if (OBJCall())
                        return true;
                }
                else if (arr[i].clss == "OLB")
                {
                    if (Arr4())
                        return true;
                }
            }
            else
            {
                if (arr[i].clss == "TERMINATOR")
                    return true;
            }
            return false;
        }

        //..............................................................OE NEW ......................................................................

        public bool OE2(out string type)
        {
            type = "";
            string type2 = "";
            if (FWID(out type))
            {
                if (Alles(ref type))
                    return true;
            }
            else if(arr[i].clss == "ID")
            {
                type = arr[i].word;
                i++;
                if (Assign3(out type2, type))
                {
                    type = type2;
                    return true;
                }
            }
            return false;
        }
        public bool FWID(out string type)
        {
            type="";
            if (Const(out type))
                return true;
            else if (arr[i].clss == "NOT")
            {
                i++;
                if (F(out type))
                    return true;
            }
            else if (arr[i].clss == "OSB")
            {
                i++;
                if(OE2(out type))
                {
                    if (Alles(ref type))
                    {
                        if (arr[i].clss == "CSB")
                        {
                            i++;
                            return true;
                        }
                    }
                }
            }
            else if (arr[i].clss == "INCDEC")
            {
                i++;
                if (ID_ST2(out type))
                    return true;
            }
            return false;
        }
        public bool Alles(ref string type)
        {
            if (OE_(ref type))
                return true;
            else if (AE_(ref type))
                return true;
            else if (RE_(ref type))
                return true;
            else if (E_(ref type))
                return true;
            else if (T_(ref type))
                return true;
            return false;
        }
        public bool Assign3(out string type2,string name)
        {
            type2 = "";
            if (arr[i].clss == "DOT" || arr[i].clss == "OLB" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC" || arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "OR" || arr[i].clss == "ASSIGN-OPT")
            {
                if (arr[i].clss == "ASSIGN-OPT")
                {
                    string type = GETReturnType(name);
                    if (Assign(out type2,type))
                    {
                        if (!IsMatched(type, type2))
                            InsertError(type + " and " + type2 + ": Mismatched");
                        else type2 = type;
                        return true;
                    }
                }
                else if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "OR")
                {
                    string type = GETReturnType(name);
                    if (Alles(ref type))
                        return true;
                }
                //else if(arr[i].clss == "OSB")
                //{
                //    i++;
                //    if(Params())
                //        if (arr[i].clss == "CSB")
                //        {
                //            i++;
                //            if (Alles())
                //                return true;
                //        }
                //    return false;
                //}
                else if (arr[i].clss == "DOT" || arr[i].clss == "OLB" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
                {
                    if (OEList(out type2,name))
                    {
                        if (Alles(ref type2))
                            return true;
                    }
                }
            }
            else
            {
                if (arr[i].clss == "TERMINATOR" || arr[i].clss == "COMMA")
                    return true;
            }
            return false;
        }
        public bool ID_ST2(out string type)
        {
            type = "";
            if (arr[i].clss == "ID")
            {
                type = Compatibility(arr[i].word,"INCDEC");
                i++;
                if (Chain())
                    return true;
            }
            else if (arr[i].clss == "THIS")
            {
                i++;
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (Chain())
                            return true;
                    }
                }
            }
            return false;
        }
        public bool Chain()
        {
            if (arr[i].clss == "OSB" || arr[i].clss == "DOT" || arr[i].clss == "OLB")
            {
                if (arr[i].clss == "OSB")
                {
                    if (FunCall3())
                        return true;
                }
                else if (arr[i].clss == "DOT")
                {
                    if (OBJCall3())
                        return true;
                }
                else if (arr[i].clss == "OLB")
                {
                    if (Arr3())
                        return true;
                }
            }
            else
            {
                if (arr[i].clss == "TERMINATOR" || arr[i].clss == "COMMA")
                    return true;
            }
            return false;
        }
        public bool OBJCall3()
        {
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (Chain())
                        return true;
                }
            }
            return false;
        }
        public bool FunArr()
        {
            string type = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if (OE(out type))
                    if (FunArrList())
                        return true;
            }
            return false;
        }
        public bool FunArrList()
        {
            string type = "";
            if (arr[i].clss == "CLB")
            {
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if(OE(out type))
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        return true;
                    }
            }
            return false;
        }

        //===================================================================================================================================


        public void Insert(string name, string type, string cat, string parent, DataTable CT)
        {
            if (!symentic.Insert(name, type, cat, parent, CT))
                InsertError("Already Decleared");
        }
        public void InsertCT(string name, string DT, string AM, string TM,bool isConst, DataTable CT)
        {
            if (!symentic.Insert_CT(name, DT, AM, TM, isConst, CT))
                InsertError("Already Decleared");   
        }
        public void InheritTypeCheck(string name)
        {
            string type, catagory, parent,table;
            symentic.Lookup(name, out type, out catagory, out parent,out table);
            if (type != "CLASS")
                InsertError(name+" is not decleared");
        }
        public bool DataTypeCheck(string type)
        {
            if (type.Contains("["))
                type = type.Substring(0, type.IndexOf('['));
            if (symentic.IsExistClass(type) || type == "void" || type == "int" || type == "float" || type == "char" || type == "string" || type == "int1D" || type == "float1D" || type == "char1D" || type == "string1D" || type == "int2D" || type == "float2D" || type == "char2D" || type == "string2D")
                return true;
            InsertError("Type '"+type+"' Does't exist");
            return false;
        }
        public void TypeIfExistThenInsert(string name,string type,string AM,string TM,bool isConst, DataTable dt)
        {
            if (DataTypeCheck(type))
                InsertCT(name, type, AM, TM, isConst, dt);
        }
        public void IfConstructNameRightThenInsert(string name,string type,string AM,string TM,bool isConst, DataTable dt)
        {
            if (name == symentic.WhatIsClassName(dt.TableName.ToString()))
                InsertCT(name, type, AM, TM, isConst, dt);
            else InsertError("Constructor Name does not Match");
        }
        public void InsertError(string Err)
        {
            rtb5.Text += Err+": Error at line no" + arr[i].line + "\n";
        }
        public DataSet GetDataSet()
        {
            return symentic.GetDataSet();
        }

        public void InsertFT(string name, string type1, string type2)
        {
            if(IsMatched(type1,type2))
            {
                if (!symentic.Insert_FT(name, type1, stack.Peek()))
                    InsertError("'" + name + "' Already decleared in this scope");
            }
            else InsertError("Type ("+type1+" and "+type2+") dismatched");
        }
        public bool IsMatched(string type1, string type2)
        {
            type1 = (type1.Contains("_") ? type1.Substring(0, type1.IndexOf("_")) : type1).ToLower();
            type2 = (type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")) : type2).ToLower();
            return type1 == type2;
        }
        public void InsertFT(string name, string type)
        {
                if (!symentic.Insert_FT(name, type, stack.Peek()))
                    InsertError("'" + name + "' Already decleared in this scope");
        }
        public void CheckInFT(string name, string type, DataTable CT)
        {
            string outType = "";
            if (symentic.Lookup_FT(name, stack.GetArr(), CT, out outType))
            {
                if (outType != type)
                    InsertError("'" + name + "' type Mismatched");
            }
            else InsertError("'" + name + "' not decleared.");
        }

        public string Compatibility(string type1, string type2, string opt)
        {
            type1 = (type1.Contains("_") ? type1.Substring(0, type1.IndexOf("_")) : (type1.Contains("[") ? type1.Substring(0, type1.IndexOf("[")) : type1)).ToLower(); //(type1.Contains("[")?type1.Substring(0,type1.IndexOf("[")):type1)
            type2 = (type2.Contains("_") ? type2.Substring(0, type2.IndexOf("_")) : (type2.Contains("[") ? type2.Substring(0, type2.IndexOf("[")) : type2)).ToLower();
            if (opt == "*" || opt == "/" || opt == "%" || opt == "+" || opt == "-")
            {
                if (type1 == "INT_CONST" && type2 == "INT_CONST" || type1 == "FLOAT_CONST" && type2 == "FLOAT_CONST" || type1 == "int" && type2 == "int" || type1 == "float" && type2 == "float")
                    return type1;
                else if (type1 == "INT_CONST" && type2 == "FLOAT_CONST" || type1 == "FLOAT_CONST" && type2 == "INT_CONST" || type1 == "int" && type2 == "float" || type1 == "float" && type2 == "int")
                    return "FLOAT_CONST";
            }
            else if (opt == ">" || opt == "<" || opt == ">=" || opt == "<=")
            {
                if (type1 == "INT_CONST" && type2 == "INT_CONST" || type1 == "FLOAT_CONST" && type2 == "FLOAT_CONST" || type1 == "int" && type2 == "int" || type1 == "float" && type2 == "float" || type1 == "INT_CONST" && type2 == "FLOAT_CONST" || type1 == "FLOAT_CONST" && type2 == "INT_CONST" || type1 == "int" && type2 == "float" || type1 == "float" && type2 == "int")
                    return "BOOL";
            }
            else if (opt == "&&" || opt == "||")
            {
                if (type1 == "BOOL" && type2 == "BOOL")
                    return "BOOL";
            }
            else if (opt == "==" || opt == "!=")
            {
                if (type1 == "BOOL" && type2 == "BOOL" || type1 == "INT_CONST" && type2 == "INT_CONST" || type1 == "FLOAT_CONST" && type2 == "FLOAT_CONST" || type1 == "int" && type2 == "int" || type1 == "float" && type2 == "float" || type1 == "INT_CONST" && type2 == "FLOAT_CONST" || type1 == "FLOAT_CONST" && type2 == "INT_CONST" || type1 == "int" && type2 == "float" || type1 == "float" && type2 == "int")
                    return "BOOL";
            }
            else if (opt == "=")
            {
                if (type1 == "float" || type1 == "int" && type2 == "int")
                    return type1;
                else if (type1 == type2)
                    return type1;
                //MessageBox.Show(type1+"--"+type2);
                InsertError(type1+" and "+type2+": Type Mismatched");
            }
            return "ERROR";
        }
        public string Compatibility(string type, string opt)
        {
            type = (type.Contains("_") ? type.Substring(0, type.IndexOf("_")) : (type.Contains("[") ? type.Substring(0, type.IndexOf("[")) : type)).ToLower();
            if (opt == "INCDEC")
            {
                if (type == "int" || type == "float" || type=="char")
                    return type;
            }
            else if (opt == "NOT")
            {
                if (type == "BOOL")
                    return "BOOL";
            }
            InsertError(type+": type invalid");
            return "ERROR";
        }
        public void CheckAssign()
        {
            if (arr[i].word != "=")
                InsertError(arr[i].word+"Invalid Assignment operator");
        }

        public void ConstructValidate(string name, string param)
        {
            //MessageBox.Show(param);
            string type,cat,parent,table;
            if (name == "int" || name == "float" || name == "char" || name == "string")
                return;
            symentic.Lookup(name, out type, out cat, out parent, out table);
            if (table!="")
            {
                for (int i = 0; i < symentic.GetDataSet().Tables.Count; i++)
                {
                    for (int j = 0; j < symentic.GetDataSet().Tables[i].Rows.Count; j++)
                    {
                        if (symentic.GetDataSet().Tables[i].Rows[j]["Name"].ToString() == name)
                        {
                            string typo = symentic.GetDataSet().Tables[i].Rows[j]["Type"].ToString();
                            if (typo.Contains(">"))
                            {
                                if (typo.Substring(0, typo.IndexOf(">")) == param)
                                    return;
                            }
                        }
                    }
                }
                InsertError(name + " Constructor not found.");
            }
            else InsertError(name+" Class not found.");
        }
        //public void CreateScope()
        //{
        //    stack.Push();
        //}
        //public void DestroyScope()
        //{
        //    stack.Pop();
        //}

        public string GETReturnType(string ID)
        {
            string type,cat,parent,table;
            if (symentic.Lookup(ID, out type, out cat, out parent,out table))
            {
                return ID;
            }
            if(symentic.Lookup_FT(ID, stack.GetArr(), GCT, out type))
            {
                return type;
            }
            InsertError(ID+" is't exist");
            return "ERROR";

        }
        public string GETFuncReturnType(string ID)
        {
            for (int ii = 0; ii < GCT.Rows.Count; ii++)
            {
                if (GCT.Rows[ii]["Name"].ToString() == ID)
                {
                    return GCT.Rows[ii]["Type"].ToString();
                }
            }
            return "ERROR";
        }
        //public string GETOBJReturnType(string name)
        //{
            
        //}
    }
}
