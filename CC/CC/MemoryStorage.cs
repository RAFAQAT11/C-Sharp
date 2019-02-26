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
                return false;
            list.Add(new StorageStructure(word, val, line));
            return true;
        }
        public List<StorageStructure> GetList()
        {
            return list;
            //list.ForEach(data => Console.WriteLine(data));
        }
    }
}
