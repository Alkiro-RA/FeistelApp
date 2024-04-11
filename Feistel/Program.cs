using CipheringApp;
using System.Threading.Tasks.Dataflow;


internal class Program
{
    internal static void Main(string[] args)
    {
        bool appRunnnig = true;
        // bool[] message = new bool[8];

        Console.WriteLine("Feistel Permutation App");

        while (appRunnnig)
        {
            Console.WriteLine(
            "[1] Zaszyfruj wiadomość\n" +
            "[2] Odszyfruj wiadomość\n" +
            "[3] Zakończ\n");

            int menuAction = 0;

            // getting input
            try
            {
                var input = Console.ReadLine();
                menuAction = int.Parse(input);
            }
            catch (Exception)
            {
                Console.WriteLine("Zła wartość.");
            }
            // menu handling
            switch (menuAction)
            {
                case 1:
                    {
                        FeistelPermutation.cypherMessage();
                        break;
                    }
                case 2:
                    {
                        FeistelPermutation.decypherMessage();
                        break;
                    }
                case 3:
                    {
                        appRunnnig = false;
                        Console.WriteLine("Do widzenia!");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Zła wartość.");
                        break;
                    }
            }
        }

        // hold app
        Console.ReadLine();
    }

    internal static bool[] GetInput()
    {
        bool[] block;

        while (true)
        {
            Console.WriteLine("Podaj 8-bitowy blok:");
            var input = Console.ReadLine();

            // block must have 8 bits 
            if (input.Length != 8)
            {
                Console.WriteLine("Blok musi być 8-bitowy (8 znaków).");
                continue;
            }

            // try parse all characters
            if (!TryParseBlock(input))
            {
                Console.WriteLine("Blok musi być składać się z zer i jedynek.");
                continue;
            }

            // build a block
            block = BuildBlock(input);
            break;
        }
        return block;
    }

    // spit me a 8-bit block
    static bool[] BuildBlock(string input)
    {
        var block = new bool[8];
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '0')
            {
                block[i] = false;
            }
            else
            {
                block[i] = true;
            }
        }
        return block;
    }

    // filter only 1s and 0s
    static bool TryParseBlock(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            char value = input[i];

            if (value == '1')
            {
                continue;
            }
            else if (value == '0')
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}