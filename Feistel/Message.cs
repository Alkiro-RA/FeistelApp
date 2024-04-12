namespace CipheringApp
{
    public class Message : ABlock
    {
        public bool[] cipheredBlock { get; set; }

        public int round { get; set; }

        public Message(bool[] input)
        {
            this.input = input;
            msb = new bool[4];
            lsb = new bool[4];
            cipheredBlock = new bool[8];
        }

        // implementation

        public void SFunctionBox(bool[] key)
        {
            // lsb
            bool x1, x2, x3, x4;
            x1 = lsb[0];
            x2 = lsb[1];
            x3 = lsb[2];
            x4 = lsb[3];
            // round key
            bool k1, k2, k3, k4;
            k1 = key[0];
            k2 = key[1];
            k3 = key[2];
            k4 = key[3];

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

        public void msbXOR()
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
            for (int i = 0; i < cipheredBlock.Length; i++)
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
