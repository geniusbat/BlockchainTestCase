using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainTestCase
{
    class Miner
    {
        List<Transaction> transactionQueue;
        public byte[] id;
        List<Block> blockchain;
        byte[] emptyHash = new byte[32]; //Sample hash to compare to possible hashes, this hash should only be used in the first block of the chain
        private bool stopSearching = false;
        private List<Miner> miners;
        private List<User> users;
        private bool listen = false;
        public Miner()
        {
            transactionQueue = new List<Transaction>();
            Random rd = new Random();
            id= new byte[32]; rd.NextBytes(id);
            blockchain = new List<Block>();
            Listen();
        }
        public void SetMiners(List<Miner> minersList)
        {
            miners = new List<Miner>();
            miners.AddRange(minersList);
            //Remove self from neighbor miners
            miners.Remove(this);
        }
        public void SetUsers(List<User> usersList)
        {
            users= usersList;
        }
        private Block Mine()
        {
            byte[] prevHash = new byte[32];
            //Fill prevHash
            if (blockchain.Count>0)
            {
                prevHash = blockchain[blockchain.Count-1].GetHash();
            }
            Block bl = new Block(transactionQueue, prevHash);
            bool looking = true;
            int maxIterations = 7000;
            int i = 0;
            while((looking)&(i<maxIterations))
            {
                looking = !bl.NextNonce();
                i++;
            }
            return bl;
        }
        private void Listen()
        {
            while(listen)
            {
                if (stopSearching)
                {
                    stopSearching = false;
                    listen = false;
                    break;
                }
                if (transactionQueue.Count > 0)
                {
                    Block bl = Mine();
                    if ((bl.CheckValidBlock()) & (!(bl.GetHash().Equals(emptyHash)) | (blockchain.Count == 0)))
                    {
                        bool valid = true;
                        //Check if transaction id wasn't used
                        foreach (Transaction tr in transactionQueue)
                        {
                            if (Utilities.BlockchainContainsTransaction(blockchain, tr.id))
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (valid) {
                            Console.WriteLine("Block found by: "+Utilities.ByteArrayToString(id));
                            blockchain.Add(bl);
                            Utilities.PrintBlockchain(blockchain);
                            //Broadcast to nearby miners
                            foreach (Miner mn in miners)
                            {
                                mn.UpdateBlockchain(blockchain);
                            }
                            //Broadcast to nearby users
                            foreach (Transaction tr in transactionQueue)
                            {
                                foreach (User us in users)
                                {
                                    us.UsedTransaction(tr.id);
                                }
                            }
                        }
                        transactionQueue.Clear();
                    }

                }
                listen = false;
            }
        }
        public void UpdateBlockchain(List<Block> newBlockChain)
        {
            if (newBlockChain.Count>blockchain.Count)
            {
                //Console.WriteLine("New blockchain set");
                stopSearching = true;
                blockchain = newBlockChain;
            }
            else if (newBlockChain.Count==blockchain.Count)
            {
                //Console.WriteLine("Some transactions met");
                //Quit from queue all transactions covered in the new blockchain
                foreach (Transaction tr in transactionQueue)
                {
                    if (Utilities.BlockchainContainsTransaction(newBlockChain,tr.id))
                    {
                        transactionQueue.Remove(tr);
                    }
                }
                stopSearching = true;
            }
        }
        public void AssignTransaction(Transaction tr)
        {
            transactionQueue.Add(tr);
            listen = true;
            Listen();
        }
    }
}
