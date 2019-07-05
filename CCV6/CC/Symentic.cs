using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace CC
{
    class Symentic
    {
        DataSet CCTs = new DataSet();
        DataTable classTable;
        DataTable funcTable;
        int tNo = 0;

        public Symentic()
        {
            Init();
        }
 
        //funContentTable.Rows.Add("ID", "int", 1);

        void Init()
        {
            classTable = new DataTable();

            classTable.Columns.Add("Name", typeof(string));
            classTable.Columns.Add("Type", typeof(string));
            classTable.Columns.Add("Catagory", typeof(string));
            classTable.Columns.Add("Parent", typeof(string));
            classTable.Columns.Add("Ref", typeof(string));
            //classTable.Rows.Add("Test", "class", "general", "-", classContentTable);

            funcTable = new DataTable();
            funcTable.Columns.Add("Name", typeof(string));
            funcTable.Columns.Add("Type", typeof(string));
            funcTable.Columns.Add("Scope", typeof(int));
            

        }
        public int GetTableNumber()
        {
            return tNo++;
        }
        public DataTable CreateCT()
        {
            DataTable classContentTable = new DataTable("Table-"+GetTableNumber());

            classContentTable.Columns.Add("Name", typeof(string));
            classContentTable.Columns.Add("Type", typeof(string));
            classContentTable.Columns.Add("AM", typeof(string));
            classContentTable.Columns.Add("TM", typeof(string));
            classContentTable.Columns.Add("Const", typeof(bool));

            CCTs.Tables.Add(classContentTable);
            //classContentTable.Rows.Add("Fun", "void->void", "Public", "-", false);

            return classContentTable;
        }
        public bool Insert(string name,string type, string catagory, string parent, DataTable dt)
        {
            string type2, catagory2, parent2,table;
            Lookup(name,out type2,out catagory2,out parent2,out table);
            if (type2 == "")
            {
                classTable.Rows.Add(name,type,catagory,parent,dt.TableName.ToString());
                return true;
            } return false;

        }
        public bool Lookup(string name, out string type, out string catagory,out string parent, out string table)
        {
            type = "";
            catagory = "";
            parent = "";
            table = "";
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (classTable.Rows[i]["Name"].ToString() == name)
                {
                    type = classTable.Rows[i]["Type"].ToString();
                    catagory = classTable.Rows[i]["Catagory"].ToString();
                    parent = classTable.Rows[i]["Parent"].ToString();
                    table = classTable.Rows[i]["Ref"].ToString();
                    return true;
                }
            }
            return false;
        }
        public bool Insert_CT(string name, string type, string AM, string TM,bool isConst, DataTable dt)
        {
            if(Lookup_CT(name,type,dt))
            {
                dt.Rows.Add(name, type, AM, TM, isConst);
                return true;
            } return false;
            //string type2, AM2, TM2;
            //Lookup_CT(name, out type2, out AM2, out TM2,dt);
            //if (type2 == "" || type2 != type)
            //{
            //    dt.Rows.Add(name, type, AM, TM,isConst);
            //    return true;
            //} return false;

        }
        public void Lookup_CT(string name, out string type, out string AM, out string TM,DataTable dt)
        {
            type = AM = TM = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Name"].ToString() == name)
                {
                    type = dt.Rows[i]["Type"].ToString();
                    AM = dt.Rows[i]["AM"].ToString();
                    TM = dt.Rows[i]["TM"].ToString();
                    break;
                }
            }
        }
        public bool Insert_FT(string name, string type, int scope)
        {
            if (Lookup_FT(name,scope))
            {
                funcTable.Rows.Add(name, type, scope);
                return true;
            } return false;
        }
        public bool Lookup_FT(string name, int currScope)
        {
            for (int i = 0; i < funcTable.Rows.Count; i++)
            {
                if (funcTable.Rows[i]["Name"].ToString() == name)
                    if (funcTable.Rows[i]["Scope"].ToString() == currScope.ToString())
                        return false;
            }
            return true;
        }
        public bool Lookup_FT(string name, int[] arr, DataTable CT , out string type)
        {
            type = "";
            for (int i = funcTable.Rows.Count-1; i >= 0; i--)
            {
                if (funcTable.Rows[i]["Name"].ToString() == name)
                {
                    for (int j = 0; j < arr.Length; j++)
                    {
                        if (funcTable.Rows[i]["Scope"].ToString() == arr[j].ToString())
                        {
                            type = funcTable.Rows[i]["Type"].ToString();
                            return true;
                        }
                    }
                }
            }

            for (int ii = 0; ii < CT.Rows.Count; ii++)
            {
                if(CT.Rows[ii]["Name"].ToString()==name)
                {
                    type = CT.Rows[ii]["Type"].ToString();
                    return true;
                }
            }
            return false;
        }
        public DataTable GetTableData1()
        {
            //string data = "\tClass Table 01\n";
            //for (int i = 0; i < classTable.Rows.Count; i++)
            //{
            //    data += "\t" + classTable.Rows[i]["Name"].ToString();
            //    data += "\t" + classTable.Rows[i]["Type"].ToString();
            //    data += "\t" + classTable.Rows[i]["Catagory"].ToString();
            //    data += "\t" + classTable.Rows[i]["Parent"].ToString();
            //    data += "\t" + classTable.Rows[i]["Ref"].ToString();
            //    data += "\n";

            //}
            return classTable;
        }
        public DataSet GetDataSet()
        {
            return CCTs;
        }
        public DataTable GetFT()
        {
            return funcTable;
        }
        public bool IsExistClass(string name)
        {
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (name == classTable.Rows[i]["Name"].ToString())
                    return true;
            }
            return false;
        }
        public string WhatIsClassName(string name)
        {
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (name == classTable.Rows[i]["Ref"].ToString())
                    return classTable.Rows[i]["Name"].ToString();
            }
            return "Not found.";
        }
        public bool Lookup_CT(string name, string type, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["Name"].ToString() == name)
                {
                    if (dt.Rows[i]["Type"].ToString().Contains('>') && type.Contains('>'))
                    {
                        if (dt.Rows[i]["Type"].ToString().Substring(0, type.IndexOf('>')) == type.Substring(0, type.IndexOf('>')))
                            return false;
                    }
                    else if (dt.Rows[i]["Type"].ToString() == type)
                        return false;
                    
                }
            }
            return true;
        }
        public string GETRef(string cName)
        {
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (classTable.Rows[i]["Name"].ToString() == cName)
                    return classTable.Rows[i]["Parent"].ToString();
            }
            return "ERROR";
        }
        public string GETClassNameFromTableName(string tableName)
        {
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (classTable.Rows[i]["Ref"].ToString() == tableName)
                    return classTable.Rows[i]["Name"].ToString();
            }
            return "ERROR";
        }
        public string GETOBJReturnType(string cName, string name, string param = "")
        {
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (classTable.Rows[i]["Name"].ToString() == cName)
                {
                    for (int j = 0; j < CCTs.Tables.Count; j++)
                    {
                        if (CCTs.Tables[j].TableName.ToString() == classTable.Rows[i]["Ref"].ToString())
                        {
                            for (int k = 0; k < CCTs.Tables[j].Rows.Count; k++)
                            {
                                if (name == CCTs.Tables[j].Rows[k]["Name"].ToString() && CCTs.Tables[j].Rows[k]["AM"].ToString() == "public")
                                {
                                    if (param != "")
                                    {
                                        string prms = CCTs.Tables[j].Rows[k]["Type"].ToString();
                                        if (param == prms.Substring(0,prms.IndexOf(">")))
                                        {
                                            return prms.Substring(prms.IndexOf(">")+1);
                                        }
                                    }
                                    if(param == "")
                                        return CCTs.Tables[j].Rows[k]["Type"].ToString();
                                }
                            }
                            return "false";
                        }
                    }
                }
            }
            return "false";
        }
        public string CheckOBJReturnType(string cName, string name, string param = "")
        {
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (classTable.Rows[i]["Name"].ToString() == cName)
                {
                    for (int j = 0; j < CCTs.Tables.Count; j++)
                    {
                        if (CCTs.Tables[j].TableName.ToString() == classTable.Rows[i]["Ref"].ToString())
                        {
                            for (int k = 0; k < CCTs.Tables[j].Rows.Count; k++)
                            {
                                if (name == CCTs.Tables[j].Rows[k]["Name"].ToString())
                                {
                                    if (param != "")
                                    {
                                        string prms = CCTs.Tables[j].Rows[k]["Type"].ToString();
                                        if (param == prms.Substring(0, prms.IndexOf(">")))
                                        {
                                            return prms.Substring(prms.IndexOf(">") + 1);
                                        }
                                    }
                                    if (param == "")
                                        return CCTs.Tables[j].Rows[k]["Type"].ToString();
                                }
                            }
                            return "false";
                        }
                    }
                }
            }
            return "false";
        }
        public bool VerifyVO(string cName,string AM,string TM,string retType,string fName)
        {
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (classTable.Rows[i]["Name"].ToString() == cName)
                {
                    for (int j = 0; j < CCTs.Tables.Count; j++)
                    {
                        if (CCTs.Tables[j].TableName.ToString() == classTable.Rows[i]["Ref"].ToString())
                        {
                            for (int k = 0; k < CCTs.Tables[j].Rows.Count; k++)
                            {
                                if (CCTs.Tables[j].Rows[k]["AM"].ToString() == AM && CCTs.Tables[j].Rows[k]["TM"].ToString() == TM && CCTs.Tables[j].Rows[k]["Type"].ToString() == retType && CCTs.Tables[j].Rows[k]["Name"].ToString() == fName)
                                    return true;
                            }
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public DataTable GETDataTable(string tableName)
        {
            for (int i = 0; i < CCTs.Tables.Count; i++)
            {
                if (CCTs.Tables[i].TableName == tableName)
                    return CCTs.Tables[i];
            }
            return new DataTable("NULL");
        }
        public string CheckClassInheritToInterface(string interf, string tableName)
        {
            string type, parent, cat,table;
            this.Lookup(interf,out type,out cat,out parent,out table);
            if (type != "INTERFACE") return "";
            DataTable interfDT = GETDataTable(table);
            DataTable classDT = GETDataTable(tableName);
            for (int i = 0; i < interfDT.Rows.Count; i++)
            {
                for (int j = 0; j < classDT.Rows.Count; j++)
                {
                    if (interfDT.Rows[i]["Name"].ToString() == classDT.Rows[j]["Name"].ToString() && interfDT.Rows[i]["Type"].ToString() == classDT.Rows[j]["Type"].ToString())
                        break;
                    //MessageBox.Show(interfDT.Rows[i]["Name"].ToString() + "==" + classDT.Rows[j]["Name"].ToString());
                    if (j == classDT.Rows.Count - 1)
                        return "false";
                }
            }

                return "true";
        }
        public bool BaseMethod(string tName,string param)
        {
            DataTable dt = GetTableFromClassName(tName);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Name"].ToString() == tName && dt.Rows[i]["Type"].ToString() == param)
                    return true;
            }    
            return false;
        }
        public DataTable GetTableFromClassName(string tName)
        {
            string type, catagory, parent, table;
            this.Lookup(tName,out type,out catagory,out parent, out table);
            for (int j = 0; j < CCTs.Tables.Count; j++)
            {
                if (CCTs.Tables[j].TableName == table)
                {
                    return CCTs.Tables[j];
                }
            }
            return new DataTable("NULL");
        }
    }
}
