using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace SporeDownloader
{
    public class Sporecast
    {
        public long Id { get; }

        public Sporecast(long id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets a collection of IDs for all of this sporecast's assets. Maximum speed of 500 creations per second, to reduce impact on the server.
        /// </summary>
        public Queue<long> getAllAssetIds()
        {
            var server = new SporeServer();

            var assetIds = new Queue<long>();

            for (int i = 0; ; i += 500)
            {
                var doc = server.getAssetsForSporecast(Id, i, 500).Element("assets");

                if(doc is null)
                {
                    break;
                }

                foreach (var asset in doc.Elements("asset"))
                {
                    long assetId = long.Parse(asset.Element("id")!.Value);

                    assetIds.Enqueue(assetId);

                    Console.WriteLine($"Found asset ID {assetId} for sporecast {Id}");
                }

                // Check if the number of retrieved creations is less than 500, if it is, exit loop
                int retrievedCount = int.Parse(doc.Element("count")!.Value);
                if (retrievedCount < 500) break;
                // Pause for a second, so we don't upset the server
                else Thread.Sleep(1000);
            }

            Console.WriteLine($"Found {assetIds.Count} assets for sporecast {Id}");

            return assetIds;
        }

        /// <summary>
        /// Downloads all assets for this sporecast.
        /// </summary>
        public void downloadAllAssets(String filePath)
        {
            var assetIds = getAllAssetIds();

            var server = new SporeServer();

            filePath += "\\" + Id;
            Directory.CreateDirectory(filePath);
            filePath += "\\";

            foreach (var id in assetIds)
            {
                server.downloadAssetPng(id, filePath + id + ".png");
                try
                {
                    server.getAssetInfo(id).Save(filePath + id + "_meta.xml");
                }
                catch (System.Xml.XmlException)
                {
                    Console.WriteLine($"Asset ID {id} for sporecast {Id} has invalid data in its Spore.com XML data, this data will not be saved");
                }

                Console.WriteLine($"Saved asset ID {id} for sporecast {Id}");
            }

            Console.WriteLine($"Saved {assetIds.Count} assets for sporecast {Id}");
        }
    }
}