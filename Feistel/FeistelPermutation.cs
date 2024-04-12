namespace CipheringApp
{
    internal class FeistelPermutation
    {
        static int round = 0;
        static int maxRound = 8;
        static bool[]? cipheredMessage;
        static internal void Permutate(bool decipherMode)
        {
            int reverseRound = maxRound - 1;
            bool[][] keys;
            // wprowadź 8-bitowy blok 
            Console.WriteLine("Tworzenie wiadomości");
            var message = new Message(Program.GetInput());
            Console.WriteLine("Tworzenie klucza");
            keys = PrepareKeys(new Key(Program.GetInput()));
            // podziel message na msb, lsb
            message.DivideBlock();
            // rozpocznij permutacje
            while (round < maxRound)
            {
                if (decipherMode == true)
                {
                    // przejdź lsb przez f(S) z odwróconymi kluczami
                    message.SFunctionBox(keys[reverseRound]);
                    reverseRound--;
                }
                else
                {
                    // przejdź lsb przez f(S)
                    message.SFunctionBox(keys[round]);
                }
                // wykonaj XOR dla msb i lsb
                message.msbXOR();
                // jeśli to nie ostatnia runda
                if (round != maxRound - 1)
                {
                    // Zamień miejscem lsb z msb
                    message.Swap();
                    round++;
                }
                else
                {
                    // złóż blok do kupy
                    message.GetFinalBlock();
                    round++;
                }
            }
            round = 0;
            Console.WriteLine("Wiadomość po zaszyfrowaniu:");
            ABlock.Show(message.cipheredBlock);
            cipheredMessage = message.cipheredBlock;
        }

        private static bool[][] PrepareKeys(Key key)
        {
            bool[][] keys = new bool[maxRound][];
            // podziel klucz na msb i lsb
            key.DivideBlock();
            while (round < maxRound)
            {
                // przygotuj klucz do rundy
                if (round % 2 == 0)
                {
                    // przesuń bit o 1 w lewo dla kmsb i klsb
                    key.msb = key.MoveBit(key.msb);
                    key.lsb = key.MoveBit(key.lsb);
                    // aktualizuj wartość combinedBits
                    key.UpdateCombinedBits();
                    // ustaw wartość preRoundKey na podstawie key msb/lsb
                    key.CreatePreRoundKeyWithMsbLsb();
                }
                else
                {
                    // przesuń bit o 1 w lewo dla combinedBits
                    key.combinedBits = key.MoveBit(key.combinedBits);
                    // aktualizuj wartość kmsb i klsb
                    key.UpdateMsbLsb();
                    // ustaw wartość preRoundKey na podstawie combinedBits
                    key.CreatePreRoundKeyWithCombinedBits();
                }
                // utwórz roundKey na podstawie keyRoundKey
                key.SetRoundKey();
                // zapisz klucz rundy w pamięci programu
                keys[round] = SaveKey(key.roundKey, round);
                round++;
            }
            round = 0;
            return keys;
        }

        private static bool[] SaveKey(bool[] key, int round)
        {
            bool[] tempKey = new bool[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                tempKey[i] = key[i];
            }
            return tempKey;
        }
    }
}
