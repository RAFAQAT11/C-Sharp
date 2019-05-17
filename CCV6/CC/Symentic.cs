using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string type2, catagory2, parent2;
            Lookup(name,out type2,out catagory2,out parent2);
            if (type2 == "")
            {
                classTable.Rows.Add(name,type,catagory,parent,dt.TableName.ToString());
                return true;
            } return false;

        }
        public void Lookup(string name, out string type, out string catagory,out string parent)
        {
            type = "";
            catagory = "";
            parent = "";
            for (int i = 0; i < classTable.Rows.Count; i++)
            {
                if (classTable.Rows[i]["Name"].ToString() == name)
                {
                    type = classTable.Rows[i]["Type"].ToString();
                    catagory = classTable.Rows[i]["Catagory"].ToString();
                    parent = classTable.Rows[i]["Parent"].ToString();
                    break;
                }
            }
        }
        public bool Insert_CT(string name, string type, string AM, string TM,bool isConst, DataTable dt)
        {
            string type2, AM2, TM2;
            Lookup_CT(name, out type2, out AM2, out TM2,dt);
            if (type2 == "" || type2 != type)
            {
                dt.Rows.Add(name, type, AM, TM,isConst);
                return true;
            } return false;

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
        public bool Insert_FT(string name, string type, int scope,string className)
        {
            string type2;
            Lookup_FT(name,new int[]{1,2,3,4},className, out type2);
            if (type2 == "")
            {
                funcTable.Rows.Add(name, type, scope);
                return true;
            } return false;
        }
        public void Lookup_FT(string name, int[] arr, string className, out string type)
        {
            type = "";
            for (int i = 0; i < funcTable.Rows.Count; i++)
            {
                if (funcTable.Rows[i]["name"].ToString() == name)
                {
                    type = classTable.Rows[i]["Type"].ToString();
                    break;
                }
            }
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
    }
}
