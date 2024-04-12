/*
 * ******************************
 * klucze od rundy 3 się pierdolą
 * ******************************
 */

namespace CipheringApp
{
    internal class FeistelPermutation
    {
        static bool[][] keys = new bool[8][];
        static bool[]? key;
        static bool[]? message;
        static bool[]? cipheredMessage;
        static int round = 0;
        static int maxRound = 4;

        static internal void cypherMessage()
        {
            // wprowadź 8-bitowy blok 
            Console.WriteLine("Tworzenie wiadomości");
            var message = new Message(Program.GetInput());
            Console.WriteLine("Tworzenie klucza");
            var key = new Key(Program.GetInput());
            // zapisz input w pamięci programu
            FeistelPermutation.key = key.input;
            FeistelPermutation.message = message.input;
            // podziel key i message na 2 części (msb, lsb)
            message.DivideBlock();
            key.DivideBlock();
            // rozpocznij permutacje
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
                SaveKey(key.roundKey, round);
                // przejdź lsb przez f(S)
                message.SFunctionBox(key.roundKey);
                // wykonaj XOR dla msb i lsb
                message.XORMsbLsb();
                // na ostatniej rundzie nie swapuj
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


        static internal void decypherMessage()
        {
            if (cipheredMessage == null)
            {
                Console.WriteLine("Nie zaszyfrowałeś wiadomości");
                return;
            }
            // starts here
            round = maxRound;
            var cipher = new Message(cipheredMessage);
            cipher.DivideBlock();
            while (round > 0)
            {
                // przejdź lsb przez f(S)
                cipher.SFunctionBox(keys[round - 1]);
                // wykonaj XOR dla msb i lsb
                cipher.XORMsbLsb();
                // na ostatniej rundzie nie swapuj
                if (round != 1)
                {
                    // Zamień miejscem lsb z msb
                    cipher.Swap();
                    round--;
                }
                else
                {
                    // złóż blok do kupy
                    cipher.GetFinalBlock();
                    round--;
                }
            }
            Console.WriteLine("Odszyfrowana wiadomość:");
            ABlock.Show(cipher.cipheredBlock);
        }

        private static void SaveKey(bool[] key, int round)
        {
            keys[round] = new bool[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                keys[round][i] = key[i];
            }
        }
    }
}
