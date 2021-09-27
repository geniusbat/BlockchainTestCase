using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainTestCase
{
    class Block
    {
        private long timeStamp;
        private byte[] nonce;
        private byte[] prevHash;
        List<Transaction> transactions;
        public Block()
        {
            timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            nonce = new byte[8];
            prevHash = new byte[32];
            transactions = new List<Transaction>();
            transactions.Add(new Transaction());
            //Fill with random info
            Random rd = new Random();
            //rd.NextBytes(nonce);
            rd.NextBytes(prevHash);
        }
        public bool TryNonce()
        {
            byte[] one = new byte[1];
            one[0] = 1;
            //Utilities.PrintByteArray(nonce);
            nonce = Utilities.AddLittleEndian(nonce, one);
            //Utilities.PrintByteArray(nonce);
            CheckValidBlock();
            return false;
        }
        public byte[] GetHash()
        {
            byte[] sum = Utilities.AddLittleEndian(nonce, prevHash);
            sum = Utilities.AddLittleEndian(sum, BitConverter.GetBytes(timeStamp));
            foreach (Transaction tr in transactions)
            {
                sum = Utilities.AddLittleEndian(sum,BitConverter.GetBytes(tr.id));
            }
            return sum;
        }
        public void CheckValidBlock()
        {
            byte[] hash = GetHash();
            /*
            BitArray dif = new BitArray(32);
            Console.WriteLine(hash[0]);
            byte[] f = new byte[(dif.Length - 1) / 8 + 1];
            dif.CopyTo(f, 0);
            Console.WriteLine(f.Length);
            Console.WriteLine(f[1]);
            PrintValues(dif,32);
            PrintValues(new BitArray(hash[0]),32);
            Console.WriteLine(dif.Length);
            Console.WriteLine(new BitArray(hash[0]).Length);
            */
            //Difficulty is based on the first 32 bits of the hash having 20 or more 0s
            BitArray bits = new BitArray(hash);
            int c = 0;
            for (int i = 0; i < 32; i++)
            {
                if (bits.Get(i)==false)
                {
                    c++;
                }
                //Console.WriteLine(bits.Get(i));
            }
            Console.WriteLine(c);
            Console.WriteLine(PrintValues(bits,true));
            bool valid;
            if (c>19)
            {
                valid = true;
            }
            else
            {
                valid = false;
            }
            Console.WriteLine(valid);

        }
        public static string PrintValues(BitArray bits,bool max32=false)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
                if (max32)
                {
                    if (i>32)
                    {
                        break;
                    }
                }
            }
            return sb.ToString();
        }
    }
}
