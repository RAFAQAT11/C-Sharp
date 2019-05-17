﻿using System;
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
        string args = "";

        public SyntaxAnalyzer(StorageStructure[] arr,RichTextBox rtb5,DataGridView dgv)
        {
            this.dgv = dgv;
            this.rtb5 = rtb5;
            this.arr = arr;
            this.i = 0;
        }

        public bool WithID()
        {
            if (arr[i].clss == "ID")
            {
                i++;
                if (List())
                    return true;
            }
            return false;
        }
        // -----------------------------------------------LIST START-----------------------------------------------

        public bool List()
        {
            if (arr[i].clss == "ASSIGN-OPT")
            {
                if (Assign())
                    return true;
            }
            else if (arr[i].clss == "OSB")
            {
                if (FunCall())
                    return true;
            }
            else if (arr[i].clss == "OLB")
            {
                if (Arr())
                    return true;
            }
            else if (arr[i].clss == "ID")
            {
                if (OBJ())
                return true;
            }
            else if (arr[i].clss == "DOT")
            {
                if (OBJCall())
                return true;
            }
            else if (arr[i].clss == "INCDEC")
            {
                if (INCDEC())
                    return true;
            }

            return false;
        }
        public bool Assign()
        {
            if (arr[i].clss == "ASSIGN-OPT")
            {
                i++;
                if (Values())
                    return true;
            }
            return false;
        }
        public bool FunCall()
        {
            if (arr[i].clss == "OSB")
            {
                i++;
                if (Params())
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        if (FunList())
                            return true;
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
            if (FunCall())
                return true;
            else if (Assign())
                return true;
            else if (OBJCall())
                return true;
            else if (Arr4())
                return true;
            else if (INCDEC())
                return true;

            return false;
        }
        public bool Arr()
        {
            if (arr[i].clss == "OLB")
            {
                i++;
                if (ArrList())
                    return true;
            }
            return false;
        }
        public bool ArrList()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (AllInit())
                        return true;
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
                        i++;
                        if (AllInit())
                            return true;
                    }
                }
            }
            else if (OE())
                if (ArrFunList())
                    return true;
            return false;
        }
        public bool ArrFunList()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                if (ARLF()) 
                    return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if(OE())
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        if (ARLF())
                            return true;
                    }

            }
            return false;
        }
        public bool ARLF()
        {
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
            else if (Assign())
                return true;
            else if (INCDEC())
                return true;
            return false;
        }
        public bool DOTList()
        {
            if (FunCall())
                return true;
            else if (OBJCall())
                return true;
            else if (Arr4())
                return true;
            else if (Assign())
                return true;
            else if (INCDEC())
                return true;
            return false;
        }
        public bool Arr2()
        {
            if (arr[i].clss == "OLB")
            {
                i++;
                if(OE())
                    if (Arr2List())
                        return true;
            }
            return false;
        }
        public bool Arr2List()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                if (Arr2CL())
                    return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if(OE())
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
            if (FunCall())
                return true;
            else if (Arr())
                return true;
            else if (OBJCall())
                return true;
            if (INCDEC())
                return true;
            return false;
        }
        public bool Assign2()
        {
            if (Assign())
                return true;
            if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool AllInit()
        {
            if (arr[i].clss == "ASSIGN-OPT")
            {
                if (Assign())
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
        public bool OBJ()
        {
            if (arr[i].clss == "ID")
            {
                i++;
                if (AllInit())
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
            if (arr[i].clss == "DOT")
            {
                if (OBJCall())
                    return true;
            }
            else if (arr[i].clss == "OSB")
            {
                if (FunCall())
                    return true;
            }
            else if(arr[i].clss == "ASSIGN-OPT")
            {
                if (Assign())
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
        
        public bool Values()
        {
            if (arr[i].clss == "ID" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                if (Init())
                    return true;
            } 
            else if (arr[i].clss == "NEW")
            {
                i++;
                if (ASSList())
                    return true;
            }
            else if (arr[i].clss == "OCB")
            {
                if (BInit())
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
        
        public bool OE()
        {
            if (AE())
                if (OE_())
                    return true;
            return false;
        }
        public bool OE_()
        {
            if (arr[i].clss == "OR" )
            {
                i++;
                if (AE())
                    if (OE_())
                        return true;
                return false;
            }
            else if (arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool AE()
        {
            if (RE())
                if (AE_())
                    return true;
            return false;
        }
        public bool AE_()
        {
            if (arr[i].clss == "AND")
            {
                i++;
                if (RE())
                    if (AE_())
                        return true;
                return false;
            }

            else if (arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool RE()
        {
            if (E())
                if (RE_())
                    return true;
            return false;
        }
        public bool RE_()
        {
            if (arr[i].clss == "RO")
            {
                i++;
                if (E())
                    if (RE_())
                        return true;
                return false;
            }
            else if (arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool E()
        {
            if (T())
                if (E_())
                    return true;
            return false;
        }
        public bool E_()
        {
            if (arr[i].clss == "PM")
            {
                i++;
                if (T())
                    if (E_())
                        return true;
                return false;
            }
            else if (arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool T()
        {
            if (F())
                if (T_())
                    return true;
            return false;
        }
        public bool T_()
        {
            if (arr[i].clss == "MDM")
            {
                i++;
                if (F())
                    if (T_())
                        return true;
                return false;
            }
            else if (arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR" || arr[i].clss == "CCB")
                return true;
            return false;
        }
        public bool F()
        {
            if (arr[i].clss == "INCDEC")
            {
                i++;
                if (INCDECList())
                    return true;
            }
            else if (Const())
                return true;
            else if (arr[i].clss == "OSB")
            {
                i++;
                if (OE())
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        return true;
                    }
            }
            else if (arr[i].clss == "NOT")
            {
                i++;
                if (F())
                    return true;
            }
            else if (arr[i].clss == "ID")
            {
                i++;
                if (OEList())
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
                        if (OEList())
                            return true;
                    }
                }

            }

            return false;
        }
        public bool INCDECList()
        {
            if (arr[i].clss == "ID")
            {
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
                    if (arr[i].clss == "OLB")
                    {
                        if (Arr3())
                            return true;
                    }
                }
            }
            else
            {
                if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
                    return true;
            }
            return false;
        }
        public bool OEList()
        {
            if (arr[i].clss == "DOT" || arr[i].clss == "OLB" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                if (arr[i].clss == "OSB")
                {
                    if (FunCallOE())
                        return true;
                }
                else if (arr[i].clss == "DOT")
                {
                    if (OBJCallOE())
                        return true;
                }
                else if (arr[i].clss == "OLB")
                {
                    if (ArrOE())
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
                if (arr[i].clss == "MDM" || arr[i].clss == "CCB" || arr[i].clss == "PM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR" || arr[i].clss == "CSB" || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR")
                    return true;
            }
            return false;
        }
        public bool FunCallOE()
        {
            if (arr[i].clss == "OSB")
            {
                i++;
                if (Params())
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        if (FunListOE())
                            return true;
                    }
            }
            return false;
        }
        public bool FunListOE()
        {
            if (arr[i].clss == "OLB" || arr[i].clss == "DOT")
            {
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (OEList())
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
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (OEList())
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
        public bool ArrOE()
        {
            if (arr[i].clss == "OLB")
            {
                i++;
                //if (arr[i].clss == "CLB") return false;
                if(OE())
                    if (ArrOEList())
                        return true;
            }
            return false;
        }
        public bool ArrOEList()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                if (ArrOEVal())
                    return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if(OE())
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        if (ArrOEVal())
                            return true;
                    }
            }
            return false;
        }
        public bool ArrOEVal()
        {
            if (arr[i].clss == "DOT" || arr[i].clss == "INCDEC")
            {
                if (arr[i].clss == "DOT")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (OEList())
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



        public bool Init()
        {
            
            if (OE2())
                if (Init3())
                    return true;
            return false;
        }
        
        public bool Init3()
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (Assign())
                        return true;
                    return false;
                }
                return false;
            }
            else if(arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }


        public bool Const()
        {
            if (arr[i].clss == "INT_CONST")
            {
                i++;
                return true;
            }
            if (arr[i].clss == "FLOAT_CONST")
            {
                i++;
                return true;
            }
            if (arr[i].clss == "CHAR_CONST")
            {
                i++;
                return true;
            }
            if (arr[i].clss == "STRING_CONST")
            {
                i++;
                return true;
            }
            return false;
        }
        
        public bool ASSList()
        {
            if (arr[i].clss == "ID" || arr[i].clss == "DT")
            {
                i++;
                if (ASSList2())
                    return true;
            }
            return false;
        }
        public bool ASSList2()
        {
            if (arr[i].clss == "OLB")
            {
                i++;
                if (OE())
                    if (ASSList3())
                        return true;
            }
            else if (arr[i].clss == "OSB")
            {
                i++;
                if (Params())
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        return true;
                    }
            }

            return false;
        }
        public bool Params()
        {
            //if (!(arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "TERMINATOR"  || arr[i].clss == "COMMA" || arr[i].clss == "CLB" || arr[i].clss == "OR"))
            //    return true;
            if (arr[i].clss == "OCB" || arr[i].clss == "ID" || arr[i].clss == "NEW" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
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
            if (OE())
            {
                if (Param3())
                    return true;
            }
            else if (arr[i].clss == "NEW")
            {
                i++;
                if (ASSList())
                    if (Param3())
                        return true;
            }
            else if (BInit())
            {
                if (Param3())
                    return true;
            }
            return false;
        }
        public bool Param3()
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (Param2())
                    return true;
                return false;
            }
            else if (arr[i].clss == "CSB")
                return true;
            return false;
        }
        public bool ASSList3()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if (OE())
                    if (arr[i].clss == "CLB")
                    {
                        i++;
                        return true;
                    }
            }

            return false;
        }

        public bool BInit()
        {
            if(arr[i].clss == "OCB")
            {
                i++;
                if (BInitList())
                    return true;
            }

            return false;
        }
        public bool BInitList()
        {
            if (arr[i].clss == "ID" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                if (ArrConst0())
                {
                    if (arr[i].clss == "CCB")
                    {
                        i++;
                        return true;
                    }
                }
            }

            else
            {
                if (ArrConst2())
                    if (arr[i].clss == "CCB")
                    {
                        i++;
                        return true;
                    }
            }
            return false;
        }
        public bool ArrConst0()
        {
            if (arr[i].clss == "ID" || arr[i].clss == "INT_CONST" || arr[i].clss == "FLOAT_CONST" || arr[i].clss == "CHAR_CONST" || arr[i].clss == "STRING_CONST" || arr[i].clss == "NOT" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC")
            {
                if (ArrConst())
                    return true;
            } 
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        public bool ArrConst()
        {
            if (OE())
                if(ACL())
                    return true;
            return false;
        }
        public bool ACL()
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (ArrConst())
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
        
        public bool ArrConst2()
        {
            if (arr[i].clss == "OCB")
            {
                if (ArrConst3())
                    return true;
            }
            else
            {
                if (arr[i].clss == "CCB")
                    return true;
            }
            return false;
        }
        public bool ArrConst3()
        {
            if (arr[i].clss == "OCB")
            {
                i++;
                if(ArrConst0())
                    if (arr[i].clss == "CCB")
                    {
                        i++;
                        if (ArrConst4())
                            return true;
                    }
            }
            return false;
        }
        public bool ArrConst4()
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (ArrConst3())
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
            if (Assign())
                return true;
            else if (FunCall())
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
                if (DTList(out isdim))
                {
                    arg += isdim;
                    return true;
                }
            }
            return false;
        }
        //public static virtual int abc() { return 1; }
        public bool DTList(out string isdim)
        {
            isdim = "";
            if (arr[i].clss == "ID")
            {
                i++;
                if (AllInit())
                    return true;
            }
            else if (Arr5(out isdim))
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
                        if(AllInit())
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
                                                return true;
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
            string dt;
            if (OE2())
                return true;
            else if (WithDT(out dt))
                return true;
            //else if (WithID())
            //    return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool C2()
        {
            if (OE2())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool C3()
        {
            if (OE2())
                return true;
            //else if (WithDT())
            //    return true;
            else if (arr[i].clss == "CSB")
                return true;
            return false;
        }
        public bool DoWhile()
        {
            if (arr[i].clss == "DO")
            {
                i++;
                if (arr[i].clss == "OCB")
                {
                    i++;
                    if (MST())
                        if (arr[i].clss == "CCB")
                        {
                            i++;
                            if (arr[i].clss == "WHILE")
                            {
                                i++;
                                if (arr[i].clss == "OSB")
                                {
                                    i++;
                                    if (OE())
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
            return false;
        }
        public bool Return_ST()
        {
            if (OE())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool If_Else()
        {
            if(arr[i].clss == "IF")
            {
                i++;
                if (arr[i].clss == "OSB")
                {
                    i++;
                    if(OE())
                        if (arr[i].clss == "CSB")
                        {
                            i++;
                            if (Body())
                                if (If_List())
                                    return true;
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
                if (Body())
                    return true;
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
                i++;
                if (Args())
                    if (arr[i].clss == "CSB")
                    {
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
                    IsConstructNameRight(name,"None",AM,TM,false,CT);
                    if (Construct())
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
                isDimentional = "1D";
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                isDimentional = "2D";
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
        public bool Args()
        {
            if (arr[i].clss == "DT" || arr[i].clss == "ID")
            {
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
            if (WithDT(out arg))
            {
                args += arg;
                if (Args_ST())
                    return true;
            }
            else if (arr[i].clss == "ID")
            {
                arg = arr[i].word;
                i++;
                string isdim="";
                if (ID3_ST(out isdim))
                {
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
        public bool ID3_ST(out string isdim)
        {
            isdim = "";
            if (OBJ())
                return true;
            else if (Arr5(out isdim))
                return true;
            return false;
        }
        public bool Arr5(out string isdim)
        {
            isdim = "";
            if (arr[i].clss == "OLB")
            {
                i++;
                if (Arr5List(out isdim))
                    return true;
            }
            return false;
        }
        public bool Arr5List(out string isdim)
        {
            isdim = "";
            if (arr[i].clss == "CLB")
            {
                isdim = "1D";
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (AllInit())
                        return true;
                }
            }
            else if (arr[i].clss == "COMMA")
            {
                isdim = "2D";
                i++;
                if (arr[i].clss == "CLB")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if (AllInit())
                            return true;
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
            if (arr[i].clss == "ASSIGN-OPT" || arr[i].clss == "TERMINATOR")
            {
                this.TypeIfExistThenInsert(name, DT, AM, TM, isConst, CT);
                if (AllInit())
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
            string isdim;
            if (OBJ())
                return true;
            else if (Arr5(out isdim))
                return true;
            return false;
        }
        public bool Construct()
        {
            if (arr[i].clss == "OSB")
            {
                i++;
                if (Args())
                    if (arr[i].clss == "CSB")
                    {
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
            DataTable CT = symentic.CreateCT();

            if (Abstract_ST(out cat))
                if (arr[i].clss == "CLASS")
                {
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
        public bool INTMethod()
        {
            string DT;
            if(arr[i].clss == "VOID" || DT_Types("","",out DT,new DataTable()))
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (arr[i].clss == "OSB")
                    {
                        i++;
                        if(Args())
                            if (arr[i].clss == "CSB")
                            {
                                i++;
                                return true;
                            }
                    }
                }
            return false;
        }
        public bool Interface_ST()
        {
            if (arr[i].clss == "INTERFACE")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (arr[i].clss == "OCB")
                    {
                        i++;
                        if(INTMethod2())
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
        public bool INTMethod2()
        {
            if (INTMethod())
            {
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    if(INTMethod2())
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
            if (arr[i].clss == "OLB")
            {
                i++;
                if (OE())
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
                i++;
                if (OE())
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
                    if (Assign())
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
            if (arr[i].clss == "DOT" || arr[i].clss == "ASSIGN-OPT" || arr[i].clss == "INCDEC" || arr[i].clss == "OSB" || arr[i].clss == "OLB")
            {
                if (arr[i].clss == "ASSIGN-OPT")
                {
                    if (Assign())
                        return true;
                }
                else if (arr[i].clss == "INCDEC")
                {
                    if (INCDEC())
                        return true;
                }
                else if (arr[i].clss == "OSB")
                {
                    if (FunCall())
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

        public bool OE2()
        {
            if (FWID())
            {
                if (Alles())
                    return true;
            }
            else if(arr[i].clss == "ID")
            {
                i++;
                if (Assign3())
                    return true;
            }
            return false;
        }
        public bool FWID()
        {
            if (Const())
                return true;
            else if (arr[i].clss == "NOT")
            {
                i++;
                if (F())
                    return true;
            }
            else if (arr[i].clss == "OSB")
            {
                i++;
                if(OE2())
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        return true;
                    }
            }
            else if (arr[i].clss == "INCDEC")
            {
                i++;
                if (ID_ST2())
                    return true;
            }
            return false;
        }
        public bool Alles()
        {
            if (OE_())
                return true;
            else if (AE_())
                return true;
            else if (RE_())
                return true;
            else if (E_())
                return true;
            else if (T_())
                return true;
            return false;
        }
        public bool Assign3()
        {
            if (arr[i].clss == "DOT" || arr[i].clss == "OLB" || arr[i].clss == "OSB" || arr[i].clss == "INCDEC" || arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "OR" || arr[i].clss == "ASSIGN-OPT")
            {
                if (arr[i].clss == "ASSIGN-OPT")
                {
                    if (Assign())
                        return true;
                }
                else if (arr[i].clss == "MDM" || arr[i].clss == "PM" || arr[i].clss == "RO" || arr[i].clss == "AND" || arr[i].clss == "OR")
                {
                    if (Alles())
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
                    if (OEList())
                    {
                        if (Alles())
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
        public bool ID_ST2()
        {
            if (arr[i].clss == "ID")
            {
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
            if (arr[i].clss == "OLB")
            {
                i++;
                if (OE())
                    if (FunArrList())
                        return true;
            }
            return false;
        }
        public bool FunArrList()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
                i++;
                if(OE())
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
            {
                InsertError("Already Decleared");
            }
        }
        public void InsertCT(string name, string DT, string AM, string TM,bool isConst, DataTable CT)
        {
            if (!symentic.Insert_CT(name, DT, AM, TM, isConst, CT))
            {
                InsertError("Already Decleared");
            }
        }
        public void InheritTypeCheck(string name)
        {
            string type, catagory, parent;
            symentic.Lookup(name, out type, out catagory, out parent);
            if (type != "CLASS")
                InsertError(name+" is not decleared");
        }
        public void TypeIfExistThenInsert(string name,string type,string AM,string TM,bool isConst, DataTable dt)
        {
            if (symentic.IsExistClass(type) || type == "int" || type == "float" || type == "char" || type == "string" || type == "int1D" || type == "float1D" || type == "char1D" || type == "string1D" || type == "int2D" || type == "float2D" || type == "char2D" || type == "string2D")
                symentic.Insert_CT(name, type, AM, TM, isConst, dt);
            else InsertError("Type Does not exist");
        }
        public void IsConstructNameRight(string name,string type,string AM,string TM,bool isConst, DataTable dt)
        {
            if (name == symentic.WhatIsClassName(dt.TableName.ToString()))
                symentic.Insert_CT(name, type, AM, TM, isConst, dt);
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
    }
}
