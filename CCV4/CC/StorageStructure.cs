using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC
{
    class StorageStructure
    {
        public string word;
        public string clss;
        public string line;

        public StorageStructure(string word, string clss, string line)
        {
            this.word = word;
            this.clss = clss;
            this.line = line;
        }

        public override string ToString()
        {
            return "("+word+","+clss+","+line+")".ToString();
        }
    }
}
