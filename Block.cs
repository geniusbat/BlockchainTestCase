using System;
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
            Utilities.PrintByteArray(nonce);
            nonce = Utilities.AddLittleEndian(nonce, one);
            Utilities.PrintByteArray(nonce);
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
    }
}
