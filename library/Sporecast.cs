using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Gets a collection of IDs for all of this sporecast's assets.
        /// </summary>
        public Queue<long> getAllAssetIds()
        {
            var server = new SporeServer();

            var assetIds = new Queue<long>();


            var doc = server.getSporecastFeed(Id).Element("{http://www.w3.org/2005/Atom}feed");//.Elements().FirstOrDefault()?.Elements().FirstOrDefault();
            Console.WriteLine(doc?.ToString());

            if (doc is not null)
            {
                foreach (var asset in doc.Elements("{http://www.w3.org/2005/Atom}entry"))
                {
                    string entryId = asset.Element("{http://www.w3.org/2005/Atom}id")!.Value;
                    long assetId = long.Parse(entryId.Split('/')[1]);

                    assetIds.Enqueue(assetId);

                    Console.WriteLine($"Found asset ID {assetId} for sporecast {Id}");
                }
                Console.WriteLine($"Found {assetIds.Count} assets for sporecast {Id}");
            }
            else
            {
                Console.WriteLine($"Found no assets for sporecast {Id}, feed did not exist");
            }


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