using Feistel;

bool appRunnnig = true;

while (appRunnnig)
{
    Console.WriteLine("Feistel Permutation App");
    Console.WriteLine(
    "[1] Zaszyfruj wiadomość\n" +
    "[2] Odszyfruj wiadomość\n" +
    "[3] Zakończ\n");

    int menuAction = 0;
    // getting instruction
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
                cypherMessage();
                break;
            }
        case 2:
            {
                decypherMessage();
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

void cypherMessage()
{
    // wprowadź 8-bitowy blok 
    var block = new Block(GetBlock());
    // podziel blok na 2 części (msb, lsb)
    block.DivideBlock();
    // ustaw wartości dla keyMsb i keyLlsb
    block.SetKeyMsb();
    block.SetKeyLsb();
    while (block.round < 8)
    {
        if (block.round % 2 == 0)
        {
            // przesuń bit o 1 w lewo dla keyMsb i keyLsb 
            block.keyMsb = block.MoveBit(block.keyMsb);
            block.keyLsb = block.MoveBit(block.keyLsb);

            // aktualizuj wartość keyBlock
            block.UpdateKeyBlock();
        }
        else
        {
            // przesuń bit o 1 w lewo dla keyBlock
            block.keyBlock = block.MoveBit(block.keyBlock);

            // aktualizuj wartość keyMsb i keyLsb
            block.UpdateKeyMsbLsb();
        }
        // ustaw wartość keyPreW na podstawie keyMsb i keyLsb lub keyBlock zaleźnie od rundy
        block.SetKeyPreW();
        // utwórz roundKey na podstawie keyPreW
        block.SetRoundKey();
        // przejdź lsb przez f(S)
        block.SFunctionBox();
        // wykonaj XOR dla msb i lsb
        block.XORMsbLsb();
        // na ostatniej rundzie nie swapuj
        if (block.round != 7)
        {
            // Zamień miejscem lsb z msb
            block.Swap();
            block.round++;
        }
        else
        {
            // złóż blok do kupy
            block.GetFinalBlock();
            block.round++;
        }
    }
    Console.WriteLine("Szyfr:");
    block.Show(block.cipheredBlock);
}

bool[] GetBlock()
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

bool[] BuildBlock(string input)
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

bool TryParseBlock(string input)
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

// not implemented yet
void decypherMessage()
{
    throw new NotImplementedException();
}

// hold app
Console.ReadLine();