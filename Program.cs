using System;

namespace BlockchainTestCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Block bl = new Block();
            bl.TryNonce();
        }
    }
}
