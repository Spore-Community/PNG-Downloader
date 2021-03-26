using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace SporeDownloader
{
    public class SporeUser
    {
        public string UserName { get; set; }

        public SporeUser(string userName)
        {
            UserName = userName;
        }

        /// <summary>
        /// Gets the profile info XML for this user
        /// </summary>
        public XDocument getProfileInfo()
        {
            return new SporeServer().getProfileInfo(UserName);
        }

        /// <summary>
        /// Gets a collection of IDs for all of this user's assets. Maximum speed of 500 creations per second, to reduce impact on the server.
        /// </summary>
        public Queue<long> getAllAssetIds()
        {
            var server = new SporeServer();

            var assetIds = new Queue<long>();

            var doc = server.getAssetsForUserFeed(UserName).Element("{http://www.w3.org/2005/Atom}feed");

            if (doc is not null)
            {
                foreach (var asset in doc.Elements("{http://www.w3.org/2005/Atom}entry"))
                {
                    string entryId = asset.Element("{http://www.w3.org/2005/Atom}id")!.Value;
                    long assetId = long.Parse(entryId.Split('/')[1]);

                    assetIds.Enqueue(assetId);

                    Console.WriteLine($"Found asset ID {assetId} for user {UserName}");
                }
                Console.WriteLine($"Found {assetIds.Count} assets for user {UserName}");
            }
            else
            {
                Console.WriteLine($"Found no assets for user {UserName}, feed did not exist");
            }

            return assetIds;
        }

        /// <summary>
        /// Downloads all assets for this user.
        /// </summary>
        public void downloadAllAssets(String filePath)
        {
            var assetIds = getAllAssetIds();

            var server = new SporeServer();

            filePath += "\\" + UserName;
            Directory.CreateDirectory(filePath);
            filePath += "\\";

            foreach (var id in assetIds)
            {
                server.downloadAssetPng(id, filePath + id + ".png");
                /*try
                {
                    server.getAssetInfo(id).Save(filePath + id + "_meta.xml");
                }
                catch (System.Xml.XmlException)
                {
                    Console.WriteLine($"Asset ID {id} for user {UserName} has invalid data in its Spore.com XML data, this data will not be saved");
                }*/

                Console.WriteLine($"Saved asset ID {id} for user {UserName}");
            }

            Console.WriteLine($"Saved {assetIds.Count} assets for user {UserName}");
        }
    }
}