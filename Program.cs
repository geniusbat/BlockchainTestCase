using System;

namespace BlockchainTestCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Block bl = new Block();
            Utilities.PrintByteArray(bl.GetHash());
            bl.CheckValidBlock();
        }
    }
}
