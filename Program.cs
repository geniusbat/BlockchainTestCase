using System;
using System.Collections.Generic;

namespace BlockchainTestCase
{
    class Program
    {
        //TODO: Display blockchain
        static void Main(string[] args)
        {
            Random rd = new Random();
            List<int> ids = new List<int>();
            ids.Add(rd.Next()); ids.Add(rd.Next()); ids.Add(rd.Next()); ids.Add(rd.Next()); ids.Add(rd.Next()); ids.Add(rd.Next()); ids.Add(rd.Next());
            Miner minerA = new Miner();
            Miner minerB = new Miner();
            List<Miner> miners = new List<Miner>();
            miners.Add(minerA); miners.Add(minerB);
            User userA = new User(miners,ids);
            User userB = new User(miners, ids);
            List<User> users = new List<User>();
            users.Add(userA); users.Add(userB);
            minerA.SetMiners(miners);
            minerA.SetUsers(users);
            minerB.SetMiners(miners);
            minerB.SetUsers(users);
            userA.CreateRandomTransaction(userB);
        }
    }
}
