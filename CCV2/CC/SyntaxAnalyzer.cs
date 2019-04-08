using System;
using System.Collections.Generic;
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
        public SyntaxAnalyzer(StorageStructure[] arr)
        {
            this.arr = arr;
            this.i = 0;
        }

        public bool WithID()
        {
            if (arr[i++].clss == "ID")
                if (List())
                    return true;
            return false;
        }
        // -----------------------------------------------LIST START-----------------------------------------------

        public bool List()
        {
            if (Assign())
                return true;

            return false;
        }
        public bool Assign()
        {
            if (arr[i++].clss == "ASSIGN-OPT")
                if (Values())
                    return true;
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
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (FunList2())
                        return true;
                }
                return false;
            }
            return true;
        }
        public bool FunList2()
        {
            if (FunCall())
                return true;
            else if (Assign())
                return true;
            else if (OBJCall())
                return true;
            else if (Arr())
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
            else if (Arr())
                return true;
            else if (INCDEC())
                return true;
            return true;
        }
        public bool DOTList()
        {
            if (FunCall())
                return true;
            else if (OBJCall())
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
            if (Assign())
                return true;
            if (arr[i].clss == "TERMINATOR") // <========================= POSSIBLE PUNGA
                return true;
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
            if (OBJCList2())
                return true;
            else if (FunCall())
                return true;
            else if (Assign())
                return true;
            else if (Arr())
                return true;
            else if (INCDEC())
                return true;

            return false;
        }
        public bool OBJCList2()
        {
            if (OBJCall())
                return true;
            else if (arr[i].clss == "TERMINATOR") //<----------------------------- POSSIBLE PUNGA
            {
                return true;
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

            if (Init())
                return true;
            else if (arr[i].clss == "NEW")
            {
                i++;
                if (ASSList())
                    return true;
            }
            else if (BInit())
                return true;
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
                return true;
            else if (OE_())
                return true;

            return false;
        }
        public bool OE_()
        {
            if (arr[i].clss == "OR")
            {
                i++;
                if (AE())
                    return true;
                else if (OE_())
                    return true;
                else return false;
            }
            return true;
        }
        public bool AE()
        {
            if (RE())
                return true;
            else if (AE_())
                return true;

            return false;
        }
        public bool AE_()
        {
            if (arr[i].clss == "AND")
            {
                i++;
                if (RE())
                    return true;
                else if (AE_())
                    return true;
                else return false;
            }

            return true;
        }
        public bool RE()
        {
            if (E())
                return true;
            else if (RE_())
                return true;

            return false;
        }
        public bool RE_()
        {
            if (arr[i].clss == "ROP")
            {
                i++;
                if (E())
                    return true;
                else if (RE_())
                    return true;
                else return false;
            }
            return true;
        }
        public bool E()
        {
            if (T())
                return true;
            else if (E_())
                return true;

            return false;
        }
        public bool E_()
        {
            if (arr[i].clss == "PM")
            {
                i++;
                if (T())
                    return true;
                else if (E_())
                    return true;
                else return false;
            }
            return true;
        }
        public bool T()
        {
            if (F())
                return true;
            else if (T_())
                return true;

            return false;
        }
        public bool T_()
        {
            if (arr[i].clss == "MDM")
            {
                i++;
                if (F())
                    return true;
                else if (T_())
                    return true;
                return false;
            }
            return true;
        }
        public bool F()
        {
            if (INCDEC())
            {
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
                if (OE())
                    return true;
            }

            return false;
        }
        public bool INCDECList()
        {
            if (arr[i].clss == "ID")
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

        public bool OEList()
        {
            if(FunCallOE())
                return true;
            else if(OBJCallOE())
                return true;
            else if(ArrOE())
                return true;
            else if(INCDEC())
                return true;
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
                        if (FunListOE())
                            return true;
                    }
            }
            return false;
        }
        public bool FunListOE()
        {
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (FunListOE2())
                        return true;
                }
                return false;
            }
            return true;
        }
        public bool FunListOE2()
        {
            if (FunCall())
                return true;
            else if (OBJCall())
                return true;
            else if (Arr())
                return true;
            else if (INCDEC())
                return true;

            return false;
        }
        public bool OBJCallOE()
        {
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (OBJOEList())
                        return true;
                }
            }
            return false;
        }
        public bool OBJOEList()
        {
            if (ArrOE())
                return true;
            else if (OBJCallOE())
                return true;
            else if (FunCallOE())
                return true;
            else if (INCDEC())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool ArrOE()
        {
            if (arr[i].clss == "OLB")
            {
                i++;
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
            if (arr[i].clss == "DOT")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    if (ArrOEVal2())
                        return true;
                }
            }
            if (INCDEC())
                return true;
            if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }

        public bool ArrOEVal2()
        {
            if (ArrOE())
                return true;
            else if (OBJCallOE())
                return true;
            else if (FunCallOE())
                return true;
            else if (INCDEC())
                return true;
            return false;
        }

        // -----------------------------------------------OE END-------------------------------------------------



        public bool Init()
        {
            
            if (Const())
                if (Init3())
                    return true;
            else if (OE())
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
                    else return false;
                }
                else return false;
            }

            return true;
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
                        return true;
            }

            return false;
        }
        public bool Params()
        {
            if (OE())
                if(ParamST())
                    return true;
            else if (arr[i].clss == "ID")
            {
                i++;
                if (ParamIDList())
                    if(ParamST())
                        return true;
            }
            return false;
        }
        public bool ParamIDList()
        {
            if (Arr())
                return true;
            else if (FunCall())
                return true;
            else if (OBJCall())
                return true;
            else if (Assign())
                return true;

            return false;
        }
        public bool ParamST()
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (Params())
                    return true;
                return false;
            }
            return true;
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
                        return true;
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
            if (ArrConst0())
            {
                if (arr[i].clss == "CCB")
                {
                    i++;
                    return true;
                }
            }
            else if (ArrConst2())
                if (arr[i].clss == "CCB")
                {
                    i++;
                    return true;
                }
            return false;
        }
        public bool ArrConst0()
        {
            if (ArrConst())
                return true;
            else if (arr[i].clss == "CCB")
            {
                i++;
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
            return true;
        }
        
        public bool ArrConst2()
        {
            if (ArrConst3())
                return true;
            else if (arr[i].clss == "CCB")
            {
                i++;
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
            if (arr[i].clss == "CCB")
            {
                i++;
                return true;
            }
            return false;
        }
        public bool Init2()
        {
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
        
    }
}
