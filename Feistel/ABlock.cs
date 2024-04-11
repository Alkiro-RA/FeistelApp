using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipheringApp
{
    public abstract class ABlock
    {
        public bool[] input { get; set; }
        public bool[] msb { get; set; }
        public bool[] lsb { get; set; }

        public void DivideBlock()
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (i < input.Length / 2)
                {
                    msb[i] = input[i];
                }
                else
                {
                    lsb[i - 4] = input[i];
                }
            }
        }

        // Show method
        static public void Show(bool[] block)
        {
            foreach (var bit in block)
            {
                Console.Write(bit ? "1" : "0");
            }
            Console.WriteLine();
        }
    }
}
