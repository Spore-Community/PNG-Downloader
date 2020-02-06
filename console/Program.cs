using System;

namespace SporeDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = args[0];

            new SporeUser(username).downloadAllAssets("spore_assets");
        }
    }
}