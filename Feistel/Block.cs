using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feistel
{
    internal class Block
    {
        public bool[] block { get; set; }
        public bool[] msb { get; set; }
        public bool[] lsb { get; set; }
        public bool[] roundKey { get; set; }
        public bool[] keyMsb { get; set; }
        public bool[] keyLsb { get; set; }
        public bool[] keyBlock { get; set; }
        public bool[] keyPreW { get; set; }
        public bool[] cipheredBlock { get; set; }

        public int round { get; set; }

        public Block(bool[] input)
        {
            block = input;
            msb = new bool[4];
            lsb = new bool[4];
            roundKey = new bool[4];
            keyMsb = new bool[4];
            keyLsb = new bool[4];
            keyBlock = new bool[8];
            keyPreW = new bool[8];
            cipheredBlock = new bool[8];
            round = 0;
        }

        // Show method
        public void Show(bool[] block)
        {
            foreach (var bit in block)
            {
                Console.Write(bit ? "1" : "0");
            }
            Console.WriteLine();
        }

        // implementation
        // podziel blok na msb i lsb
        public void DivideBlock()
        {
            for (int i = 0; i < block.Length; i++)
            {
                if (i < block.Length / 2)
                {
                    msb[i] = block[i];
                }
                else
                {
                    lsb[i - 4] = block[i];
                }
            }
        }

        public void SetKeyLsb()
        {
            for (int i = 0; i < keyLsb.Length; i++)
            {
                keyLsb[i] = lsb[i];
            }
        }
        public void SetKeyMsb()
        {
            for (int i = 0; i < keyMsb.Length; i++)
            {
                keyMsb[i] = msb[i];
            }
        }

        public bool[] MoveBit(bool[] block)
        {
            bool carry = false;
            for (int i = block.Length - 1; i >= 0; i--)
            {
                bool temp;
                // ogarnij "pierwszy" index
                if (i == block.Length - 1)
                {
                    temp = block[i - 1];
                    block[i - 1] = block[i];
                    carry = temp;
                    continue;
                }
                // ogarnij "ostatni" index
                if (i == 0)
                {
                    block[block.Length - 1] = carry;
                    continue;
                }
                temp = block[i - 1];
                block[i - 1] = carry;
                carry = temp;
            }
            return block;
        }

        public void SetKeyPreW()
        {
            if (round % 2 == 0)
            {
                for (int i = 0; i < keyPreW.Length; i++)
                {
                    if (i < keyPreW.Length / 2)
                    {
                        keyPreW[i] = keyMsb[i];
                    }
                    else
                    {
                        keyPreW[i] = keyLsb[i - 4];
                    }
                }
            }
            else
            {
                for (int i = 0; i < keyPreW.Length; i++)
                {
                    keyPreW[i] = keyBlock[i];
                }
            }
        }

        public void SetRoundKey()
        {
            for (int i = 0; i < roundKey.Length; i++)
            {
                roundKey[i] = keyPreW[i * 2];
            }
        }

        public void UpdateKeyBlock()
        {
            for (int i = 0; i < keyBlock.Length; i++)
            {
                if (i < keyBlock.Length / 2)
                {
                    keyBlock[i] = keyMsb[i];
                }
                else
                {
                    keyBlock[i] = keyLsb[i - 4];
                }
            }
        }

        public void UpdateKeyMsbLsb()
        {
            for (int i = 0; i < keyBlock.Length; i++)
            {
                if (i < keyBlock.Length / 2)
                {
                    keyMsb[i] = keyBlock[i];
                }
                else
                {
                    keyLsb[i - 4] = keyBlock[i];
                }
            }
        }

        public void SFunctionBox()
        {
            // lsb
            bool x1, x2, x3, x4;
            x1 = lsb[0];
            x2 = lsb[1];
            x3 = lsb[2];
            x4 = lsb[3];
            // round key
            bool k1, k2, k3, k4;
            k1 = roundKey[0];
            k2 = roundKey[1];
            k3 = roundKey[2];
            k4 = roundKey[3];

            // f1
            bool r1 = x1 ^ (x1 && x3) ^ (x2 && x4) ^ (x2 && x3 && x4) ^ (x1 && x2 && x3 && x4) ^ k1;
            // f2
            bool r2 = x2 ^ (x1 && x3) ^ (x1 && x2 && x4) ^ (x1 && x3 && x4) ^ (x1 && x2 && x3 && x4) ^ k2;
            // f3
            bool r3 = true ^ x3 ^ (x1 && x4) ^ (x1 && x2 && x4) ^ (x1 && x2 && x4) ^ (x1 && x2 && x3 && x4) ^ k3;
            // f4
            bool r4 = true ^ (x1 && x2) ^ (x3 && x4) ^ (x1 && x2 && x4) ^ (x1 && x3 && x4) ^ (x1 && x2 && x3 && x4) ^ k4;

            // collect results
            lsb[0] = r1;
            lsb[1] = r2;
            lsb[2] = r3;
            lsb[3] = r4;
        }

        public void XORMsbLsb()
        {
            // msb
            bool x1, x2, x3, x4;
            x1 = msb[0];
            x2 = msb[1];
            x3 = msb[2];
            x4 = msb[3];
            // lsb
            bool y1, y2, y3, y4;
            y1 = lsb[0];
            y2 = lsb[1];
            y3 = lsb[2];
            y4 = lsb[3];
            // xor
            bool r1 = x1 ^ y1;
            bool r2 = x2 ^ y2;
            bool r3 = x3 ^ y3;
            bool r4 = x4 ^ y4;
            // collect results
            msb[0] = r1;
            msb[1] = r2;
            msb[2] = r3;
            msb[3] = r4;
        }

        public void Swap()
        {
            bool[] temp = new bool[msb.Length];
            for (int i = 0; i < msb.Length; i++)
            {
                temp[i] = msb[i];
                msb[i] = lsb[i];
                lsb[i] = temp[i];
            }
        }

        internal void GetFinalBlock()
        {
            for ( int i = 0; i < cipheredBlock.Length; i++)
            {
                if (i < cipheredBlock.Length / 2)
                {
                    cipheredBlock[i] = msb[i];
                }
                else
                {
                    cipheredBlock[i] = lsb[i - 4];
                }
            }
        }
    }
}
