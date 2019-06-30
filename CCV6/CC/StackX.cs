using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC
{
    public class StackX
    {
        int uniqueScope = 1;
        List<int> list = new List<int>();
        public void Push()
        {
            list.Add(uniqueScope);
            uniqueScope++;
        }
        public int Pop()
        {
            int rm = 0;
            if (list.Count != 0)
            {
                rm = list.ElementAt(list.Count - 1);
                list.RemoveAt(list.Count - 1);
            }
            return rm;
        }
        public int Peek()
        {
            return list.Count > 0 ? list.ElementAt(list.Count-1) : 0;
        }
        public int[] GetArr()
        {
            return list.ToArray();
        }
    }
}
