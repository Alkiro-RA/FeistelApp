using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipheringApp
{
    public class Key : ABlock
    {
        public bool[] combinedBits { get; set; }
        public bool[] preRoundKey { get; set; }
        public bool[] roundKey { get; set; }

        public Key(bool[] input)
        {
            this.input = input;
            msb = new bool[4];
            lsb = new bool[4];
            combinedBits = new bool[8];
            preRoundKey = new bool[8];
            roundKey = new bool[4];
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

        public void UpdateCombinedBits()
        {
            for (int i = 0; i < combinedBits.Length; i++)
            {
                if (i < combinedBits.Length / 2)
                {
                    combinedBits[i] = msb[i];
                }
                else
                {
                    combinedBits[i] = lsb[i - 4];
                }
            }
        }

        public void UpdateMsbLsb()
        {
            for (int i = 0; i < combinedBits.Length; i++)
            {
                if (i < combinedBits.Length / 2)
                {
                    msb[i] = combinedBits[i];
                }
                else
                {
                    msb[i - 4] = combinedBits[i];
                }
            }
        }

        public void CreatePreRoundKeyWithMsbLsb()
        {
            for (int i = 0; i < preRoundKey.Length; i++)
            {
                if (i < preRoundKey.Length / 2)
                {
                    preRoundKey[i] = msb[i];
                }
                else
                {
                    preRoundKey[i] = lsb[i - 4];
                }
            }
        }

        public void CreatePreRoundKeyWithCombinedBits()
        {
            for (int i = 0; i < preRoundKey.Length; i++)
            {
                preRoundKey[i] = combinedBits[i];
            }
        }

        public void SetRoundKey()
        {
            for (int i = 0; i < roundKey.Length; i++)
            {
                roundKey[i] = preRoundKey[i * 2];
            }
        }
    }
}
