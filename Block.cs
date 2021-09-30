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
        public long timeStamp;
        private byte[] nonce;
        public byte[] prevHash;
        public List<Transaction> transactions;
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
        public Block(List<Transaction> transactionsQueue, byte[] prHash)
        {
            timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            nonce = new byte[8];
            prevHash = prHash;
            transactions = transactionsQueue;
            nonce = new byte[32];
        }
        public bool NextNonce()
        {
            byte[] one = new byte[1];
            one[0] = 1;
            //Utilities.PrintByteArray(nonce);
            nonce = Utilities.AddLittleEndian(nonce, one);
            //Utilities.PrintByteArray(nonce);
            return CheckValidBlock();
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
        public bool CheckValidBlock()
        {
            byte[] hash = GetHash();
            //Difficulty is based on the first 32 bits of the hash having 15 or more 0s
            BitArray bits = new BitArray(hash);
            int c = 0;
            for (int i = 0; i < 32; i++)
            {
                if (bits.Get(i)==false)
                {
                    c++;
                }
            }
            bool valid;
            if (c>14)
            {
                valid = true;
            }
            else
            {
                //Console.WriteLine("Block wasn't valid due to no fulfilling predicate, with only " + c + " ceros");
                valid = false;
            }
            return valid;

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
