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
            if (Assign())
                return true;
            else if (FunCall())
                return true;
            else if (Arr())
                return true;
            else if (OBJ())
                return true;
            else if (OBJCall())
                return true;
            else if (INCDEC())
                return true;

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
            else if (arr[i].clss == "COMMA")
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
            return true;
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
            if (FunCall3())
                return true;
            else if (Arr3())
                return true;
            else if (arr[i - 1].clss == "ID")
                return true;

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

        public bool WithDT()
        {
            if (arr[i].clss == "DT")
            {
                i++;
                if (DTList())
                    return true;
            }
            return false;
        }
        public bool DTList()
        {
            if (arr[i].clss == "ID")
            {
                i++;
                if (AllInit())
                    return true;
            }
            else if (Arr())
                return true;
            return false;
        }

        public bool Static_ST()
        {
            if (arr[i].clss == "STATIC")
            {
                i++;
                return true;
            }
            return true;
        }
        public bool Const_ST()
        {
            if (arr[i].clss == "CONST")
            {
                i++;
                return true;
            }
            return true;
        }
        public bool WithStaticConst_DT()
        {
            if (Static_ST())
                if (Const_ST())
                    if (WithDT())
                        return true;
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
                        return true;
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
                                            if (Body())
                                                return true;
                                        }
                                }
                        }
                }
            }
            return false;
        }
        public bool C1()
        {
            if (OE())
                return true;
            else if (WithDT())
                return true;
            else if (WithID())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool C2()
        {
            if (OE())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool C3()
        {
            if (OE())
                return true;
            else if (WithDT())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
        public bool DoWhile()
        {
            if (arr[i].clss == "DO")
            {
                i++;
                if(Body())
                    if (arr[i].clss == "WHILE")
                    {
                        i++;
                        if (arr[i].clss == "OSB")
                        {
                            i++;
                            if(OE())
                                if (arr[i].clss == "CSB")
                                {
                                    i++;
                                    return true;
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
            return true;
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
            if (!SST())
                return false;
            else if (!MST())
                return false;
            return true;
        }
        public bool Method()
        {
            if(Static_ST())
                if(VO())
                    if(Ret_Type())
                        if(ID_ST())
                            if (arr[i].clss == "OSB")
                            {
                                i++;
                                if(Args_ST())
                                    if (arr[i].clss == "CSB")
                                    {
                                        i++;
                                        if (Super_Class())
                                            if (Body())
                                                return true;
                                    }
                            }
            return false;
        }
        public bool VO()
        {
            if (arr[i].clss == "VIRTUAL")
            {
                i++;
                return true;
            }
            if (arr[i].clss == "OVERRIDE")
            {
                i++;
                return true;
            }
            return true;
        }
        public bool Ret_Type()
        {
            if (arr[i].clss == "VOID" || arr[i].clss == "ID" )
            {
                i++;
                return true;
            }
            else if (arr[i].clss == "DT")
            {
                i++;
                if (DT2List())
                    return true;
            }
            return false;
        }
        public bool DT2List()
        {
            if (arr[i].clss == "OLB")
            {
                i++;
                if (!DT2List2())
                    return false;
            }
            return true;
        }
        public bool DT2List2()
        {
            if (arr[i].clss == "CLB")
            {
                i++;
                return true;
            }
            else if (arr[i].clss == "COMMA")
            {
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
                if (!ID_ST())
                    return false;
            }
            return true;
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
                        if(Params())
                            if (arr[i].clss == "CSB")
                            {
                                i++;
                                return true;
                            }
                    }
                }
                return false;
            }
            return true;
        }
        public bool Args()
        {
            if (WithDT())
            {
                if (Args_ST())
                    return true;
            }
            else if (arr[i].clss == "ID")
            {
                i++;
                if (ID3_ST())
                    if (Args_ST())
                        return true;
            }
            return false;
        }
        public bool Args_ST()
        {
            if (arr[i].clss == "COMMA")
            {
                i++;
                if (!Args())
                    return false;
            }
            return true;
        }
        public bool ID3_ST()
        {
            if (OBJ())
                return true;
            else if (Arr())
                return true;
            return false;
        }
        public bool WithAM()
        {
            if (AM_ST())
                if (AMList())
                    return true;
            return false;
        }
        public bool AM_ST()
        {
            if (arr[i].clss == "AM")
                i++;
            return true;
        }
        public bool AMList()
        {
            if (Method())
                return true;
            else if (WithDT())
            {
                if (arr[i].clss == "TERMINATOR")
                {
                    i++;
                    return true;
                }
            }
            else if (arr[i].clss == "ID")
            {
                i++;
                if (DECList())
                    if (arr[i].clss == "TERMINATOR")
                    {
                        i++;
                        return true;
                    }
            }
            else if (Construct())
                return true;
            return false;
        }
        public bool DECList()
        {
            if (OBJ())
                return true;
            else if (Arr())
                return true;
            return false;
        }
        public bool Construct()
        {
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
                            if (Body())
                                return true;
                        }
                }
            }
            return false;
        }
        public bool Class_ST()
        {
            if (Abstract_ST())
                if (arr[i].clss == "CLASS")
                {
                    i++;
                    if (arr[i].clss == "ID")
                    {
                        i++;
                        if(Inherit_ST())
                            if (arr[i].clss == "OCB")
                            {
                                i++;
                                if(ClassList())
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
        public bool Abstract_ST()
        {
            if (arr[i].clss == "ABSTRACT")
                i++;
            return true;
        }
        public bool Inherit_ST()
        {
            if (arr[i].clss == "COLON")
            {
                i++;
                if (arr[i].clss == "ID")
                {
                    i++;
                    return true;
                }
                return false;
            }
            return true;
        }
        public bool ClassList()
        {
            if (WithAM())
            {
                if (ClassList())
                    return true;
            }
            else if (NBody())
                return true;

            return true;
        }
        public bool INTMethod()
        {
            if(Ret_Type())
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
            return true;
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
                                return true;
                            }
                    }
                }
            }
            return false;
        }
        public bool NamespaceBody()
        {
            if (NBody())
            {
                if (NamespaceBody())
                    return true;
                return false;
            }
            return true;
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
            else if (arr[i].clss == "TERMINATOR")
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
            else if (Assign())
                return true;
            else if (INCDEC())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;

            return false;
        }
        public bool Arr4DOTList()
        {
            if (Assign())
                return true;
            else if (INCDEC())
                return true;
            else if (FunCall())
                return true;
            else if (OBJCall())
                return true;
            else if (Arr4())
                return true;
            else if (arr[i].clss == "TERMINATOR")
                return true;
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
            else if(ID_ST2())
            {
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
            if (T_())
                return true;
            else if (E_())
                return true;
            else if (RE_())
                return true;
            else if (AE_())
                return true;
            else if (OE_())
                return true;
            return false;
        }
        public bool Assign3()
        {
            if (Assign())
                return true;
            else if (Alles())
                return true;
            else if(arr[i].clss == "OSB")
            {
                i++;
                if(Params())
                    if (arr[i].clss == "CSB")
                    {
                        i++;
                        if (Alles())
                            return true;
                    }
                return false;
            }
            else if (arr[i].clss == "INCDEC")
            {
                i++;
                if (Alles())
                    return true;
            }
            if (arr[i].clss == "TERMINATOR")
                return true;
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
            if (FunCall3())
                return true;
            else if (Fun3List())
                return true;
            else if (Arr3())
                return true;
            if (arr[i].clss == "TERMINATOR")
                return true;
            return false;
        }
    }
}
