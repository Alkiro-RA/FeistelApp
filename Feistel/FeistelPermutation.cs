using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipheringApp
{
    internal class FeistelPermutation
    {
        static bool[][] keys = new bool[8][];
        static bool[]? key;
        static bool[]? message;
        static bool[]? cipheredMessage;
        static int round = 0;

        static internal void cypherMessage()
        {
            // wprowadź 8-bitowy blok 
            Console.WriteLine("Tworzenie wiadomości");
            var messageBlock = new Message(Program.GetInput());
            Console.WriteLine("Tworzenie klucza");
            var keyBlock = new Key(Program.GetInput());
            // zapisz input do programu
            key = keyBlock.input;
            message = messageBlock.input;
            // podziel key i message na 2 części (msb, lsb)
            messageBlock.DivideBlock();
            keyBlock.DivideBlock();
            // rozpocznij permutacje
            while (round < 8)
            {
                // przygotuj klucz do rundy
                if (round % 2 == 0)
                {
                    // przesuń bit o 1 w lewo dla kmsb i klsb
                    keyBlock.msb = keyBlock.MoveBit(keyBlock.msb);
                    keyBlock.lsb = keyBlock.MoveBit(keyBlock.lsb);
                    // aktualizuj wartość combinedBits
                    keyBlock.UpdateCombinedBits();
                }
                else
                {
                    // przesuń bit o 1 w lewo dla combinedBits
                    keyBlock.combinedBits = keyBlock.MoveBit(keyBlock.combinedBits);
                    // aktualizuj wartość kmsb i klsb
                    keyBlock.UpdateMsbLsb();
                }
                // zaleźnie od rundy ustaw wartość preRoundKey na podstawie key msb/lsb lub combinedBits
                if (round % 2 == 0)
                {
                    keyBlock.CreatePreRoundKeyWithMsbLsb();
                }
                else
                {
                    keyBlock.CreatePreRoundKeyWithCombinedBits();
                }
                // utwórz roundKey na podstawie keyPreW
                keyBlock.SetRoundKey();
                // zapisz klucz rundy do programu
                keys[round] = keyBlock.roundKey;
                // przejdź lsb przez f(S)
                messageBlock.SFunctionBox();
                // wykonaj XOR dla msb i lsb
                messageBlock.XORMsbLsb();
                // na ostatniej rundzie nie swapuj
                if (round != 7)
                {
                    // Zamień miejscem lsb z msb
                    messageBlock.Swap();
                    round++;
                }
                else
                {
                    // złóż blok do kupy
                    messageBlock.GetFinalBlock();
                    round++;
                }
            }
            round = 0;
            Console.WriteLine("Wiadomość po zaszyfrowaniu:");
            ABlock.Show(messageBlock.cipheredBlock);
        }

        // not implemented yet
        static internal void decypherMessage()
        {
            throw new NotImplementedException();
        }

        // algorithm methods ???
    }
}
