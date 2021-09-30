using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainTestCase
{
    class User
    {
        List<Miner> miners;
        public byte[] id;
        List<int> unusedIds;
        public User(List<Miner> minersList, List<int> ids)
        {
            Random rd = new Random();
            id = new byte[32]; rd.NextBytes(id);
            miners = minersList;
            unusedIds = ids;
        }
        public void SetMiners(List<Miner> minerList)
        {
            miners = minerList;
        }
        public void CreateRandomTransaction(User to)
        {
            Console.WriteLine("Started transaction");
            Random rd = new Random();
            SHA256 sh = SHA256.Create();
            Transaction tr = new Transaction(id, to.id, rd.Next(), unusedIds[rd.Next(0,unusedIds.Count)]);
            //Signing should require something more complex requiring the private and public key of the signer
            if (tr.SetSign(sh.ComputeHash(tr.Sum())))
            {
                Console.WriteLine("Broadcasting to miners");
                //Broadcast to miners
                foreach(Miner mn in miners)
                {
                    mn.AssignTransaction(tr);
                }
            }
            else
            {
                Console.WriteLine("Transaction failed to be created");
            }
        }
        public void UsedTransaction(int id)
        {
            unusedIds.Remove(id);
        }
    }
}
