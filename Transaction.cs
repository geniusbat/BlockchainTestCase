using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BlockchainTestCase
{
    class Transaction
    {
        public int id;
        public int amount;
        public byte[] from;
        public byte[] to;
        private byte[] sign;
        public Transaction()
        {
            Random rd = new Random();
            from = new byte[32]; rd.NextBytes(from);
            to= new byte[32]; rd.NextBytes(to);
            amount = rd.Next();
            id = rd.Next();
            SHA256 sh = SHA256.Create();
            byte[] sum = Utilities.AddLittleEndian(from, to);
            sum = Utilities.AddLittleEndian(sum, BitConverter.GetBytes(id));
            sum = Utilities.AddLittleEndian(sum, BitConverter.GetBytes(amount));
            sign = sh.ComputeHash(sum);
        }
        public Transaction(byte[] fr, byte[] t, int am, int idI)
        {
            from = fr;
            to = t;
            id = idI;
            amount = am;
        }
        public byte[] GetSign()
        {
            return sign;
        }
        //Check to validate if sign is correct
        public bool SetSign(byte[] input)
        {
            sign = input;
            return true;
        }
        //Return sum to be signed
        public byte[] Sum()
        {
            byte[] sum = Utilities.AddLittleEndian(from, to);
            sum = Utilities.AddLittleEndian(sum, BitConverter.GetBytes(id));
            sum = Utilities.AddLittleEndian(sum, BitConverter.GetBytes(amount));
            return sum;
        }
    }
}
