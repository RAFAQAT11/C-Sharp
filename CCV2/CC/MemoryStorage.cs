using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC
{
    class MemoryStorage
    {
        LaxicalComponents lax = new LaxicalComponents();
        List<StorageStructure> list = new List<StorageStructure>();
        public bool Add(string word,string line)
        {
            string val = lax.Analyze(word);
            if (val == "error")
            {
                list.Add(new StorageStructure(word, val, line));
                return false;
            }
            if (val == "string_const" || val == "char_const")
                word = word.Substring(1, word.Length - 2);
            list.Add(new StorageStructure(word, val, line));
            return true;
        }
        public List<StorageStructure> GetList()
        {
            //int a = -5*-5;
            return list;
            //list.ForEach(data => Console.WriteLine(data));
        }
    }
}
