using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KovaakShuffler
{
    class GroupUtil
    {
        public List<Tuple<T, int>> GroupBySame<T>(List<T> data)
        {
            var outp = new List<Tuple<T, int>>();
            if (data.Count == 0) return outp;
            T last = data[0];
            int count = 0;

            foreach (var value in data)
            {
                if (value.Equals(last))
                {
                    count += 1;
                }
                else
                {
                    outp.Add(new Tuple<T, int>(last, count));
                    count = 1;
                }
                last = value;
            }

            outp.Add(new Tuple<T, int>(last, count));

            return outp;
        }

        public List<T> UngroupBySame<T>(List<Tuple<T, int>> data)
        {
            var outp = new List<T>();
            foreach (var pair in data)
            {
                outp.AddRange(Enumerable.Repeat<T>(pair.Item1, pair.Item2));
            }
            return outp;
        }
    }
}
