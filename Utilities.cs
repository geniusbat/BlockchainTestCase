using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainTestCase
{
    class Utilities
    {
        //Code thanks to Mike Danes <Wyck>
        public static byte[] AddLittleEndian(byte[] a, byte[] b)
        {
            List<byte> result = new List<byte>();
            if (a.Length < b.Length)
            {
                byte[] t = a;
                a = b;
                b = t;
            }
            int carry = 0;
            for (int i = 0; i < b.Length; ++i)
            {
                int sum = a[i] + b[i] + carry;
                result.Add((byte)(sum & 0xFF));
                carry = sum >> 8;
            }
            for (int i = b.Length; i < a.Length; ++i)
            {
                int sum = a[i] + carry;
                result.Add((byte)(sum & 0xFF));
                carry = sum >> 8;
            }
            if (carry > 0)
            {
                result.Add((byte)carry);
            }
            return result.ToArray();
        }
        public static void PrintByteArray(byte[] a)
        {
            Console.Write("[");
            for (int i = a.Length-1; i >= 0; i--)
            {
                if (i != a.Length-1)
                {
                    Console.Write(", ");
                }
                byte item = a[i];
                Console.Write(item);
            }
            Console.Write("]");
            Console.WriteLine("");
        }
        public static String ByteArrayToString(byte[] a)
        {
            String rt = "[";
            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (i != a.Length - 1)
                {
                    rt += ", ";
                }
                byte item = a[i];
                rt += item;
            }
            rt += "]";
            return rt;
        }
        public static bool BlockchainContainsTransaction(List<Block> blockchain, int id)
        {
            bool inside = false;
            foreach(Block bl in blockchain)
            {
                if (TransactionListContainsTransaction(bl.transactions, id))
                {
                    inside = true;
                    break;
                }
            }
            return inside;
        }
        public static bool TransactionListContainsTransaction(List<Transaction> transactionList, int id)
        {
            bool inside = false;
            foreach(Transaction tr in transactionList)
            {
                if (tr.id==id)
                {
                    inside = true;
                    break;
                }
            }
            return inside;
        }
        public static void PrintBlockchain(List<Block> blockchain)
        {
            foreach(Block bl in blockchain)
            {
                Console.WriteLine("Hash: "+ ByteArrayToString(bl.GetHash()));
                Console.WriteLine("Previous: "+ ByteArrayToString(bl.prevHash));
                Console.WriteLine("With transactions: ");
                foreach(Transaction tr in bl.transactions)
                {
                    Console.WriteLine(tr.id+": "+ByteArrayToString(tr.from)+" --> "+ByteArrayToString(tr.to)+ " , amount: "+tr.amount);
                }
                Console.WriteLine();
            }
        }
    }
}
