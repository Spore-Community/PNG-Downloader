using System;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace SporeDownloader
{
    public class SporeServer
    {
        private string endpoint = "https://www.spore.com";

        /// <summary>
        /// Get daily stats about Spore.com
        /// </summary>
        public XDocument getStats()
        {
            return XDocument.Load(endpoint + "/rest/stats");
        }

        /// <summary>
        /// Get various stats like height, diet, abilities etc. for a creature, if you know it's asset Id
        /// </summary>
        public XDocument getCreatureStats(long assetId)
        {
            return XDocument.Load(endpoint + "/rest/creature/" + assetId);
        }

        /// <summary>
        /// Get profile pic, tagline, user id and creation date for a username
        /// </summary>
        public XDocument getProfileInfo(string username)
        {
            return XDocument.Load(endpoint + "/rest/user/" + username);
        }

        /// <summary>
        /// Get asset id, name, creation date, type, parent and rating for a list of assets created by a user
        /// </summary>
        public XDocument getAssetsForUser(string username, int startIndex, int length)
        {
            return XDocument.Load(endpoint + "/rest/assets/user/" + username + "/" + startIndex + "/" + length);
        }

        /// <summary>
        /// Get id, name, tags, subscription count, rating etc. for Sporecasts subscribed to by a user
        /// </summary>
        public XDocument getAssetsForSporecast(long sporecastId, int startIndex, int length)
        {
            return XDocument.Load(endpoint + "/rest/assets/sporecast/" + sporecastId + "/" + startIndex + "/" + length);
        }

        /// <summary>
        /// Get number of achievements for user and a list of achievement ids and unlock-dates
        /// </summary>
        public XDocument getAchievementsForUser(string username, int startIndex, int length)
        {
            return XDocument.Load(endpoint + "/rest/achievements/" + username + "/" + startIndex + "/" + length);
        }

        /// <summary>
        /// For a given asset id, get name, description, tags, 10 latest comments, type, parent, rating, creation date and author name/id
        /// </summary>
        public XDocument getAssetInfo(long assetId)
        {
            return XDocument.Load(endpoint + "/rest/asset/" + assetId);
        }

        /// <summary>
        /// For a given asset id, get a list of comments, sender names and comment dates
        /// </summary>
        public XDocument getCommentsForAsset(long assetId, int startIndex, int length)
        {
            return XDocument.Load(endpoint + "/rest/comments/" + assetId + "/" + startIndex + "/" + length);
        }

        /// <summary>
        /// For a given username, get a list of buddy names and ids and total buddy count
        /// </summary>
        public XDocument getBuddiesForUser(string username, int startIndex, int length)
        {
            return XDocument.Load(endpoint + "/rest/users/buddies/" + username + "/" + startIndex + "/" + length);
        }

        /// <summary>
        /// For a given username, get the list of users who have added that username as a buddy.
        /// </summary>
        public XDocument getSubscribersForUser(string username, int startIndex, int length)
        {
            return XDocument.Load(endpoint + "/rest/users/subscribers/" + username + "/" + startIndex + "/" + length);
        }

        /// <summary>
        /// List creations for a given view.
        /// View Types are: TOP_RATED, TOP_RATED_NEW, NEWEST, FEATURED, MAXIS_MADE, RANDOM, CUTE_AND_CREEPY
        /// For each asset you get id, name, author, creation date, rating, type and parent
        /// </summary>
        public XDocument search(string viewType, int startIndex, int length)
        {
            return XDocument.Load(endpoint + "/rest/assets/search/" + viewType + "/" + startIndex + "/" + length);
        }

        /// <summary>
        /// List creations for a given view.
        /// View Types are: TOP_RATED, TOP_RATED_NEW, NEWEST, FEATURED, MAXIS_MADE, RANDOM, CUTE_AND_CREEPY
        /// For each asset you get id, name, author, creation date, rating, type and parent
        /// Optionally, you can specify an asset type
        /// Asset types are: UFO, CREATURE, BUILDING, VEHICLE
        /// </summary>
        public XDocument search(string viewType, int startIndex, int length, string assetType)
        {
            return XDocument.Load(endpoint + "/rest/assets/search/" + viewType + "/" + startIndex + "/" + length + "/" + assetType);
        }



        /// <summary>
        /// Get XML for an asset id
        /// </summary>
        public XDocument getXmlAsset(long assetId)
        {
            string id = assetId.ToString();
            string subId1 = id.Substring(0, 3);
            string subId2 = id.Substring(3, 3);
            string subId3 = id.Substring(6, 3);

            return XDocument.Load(endpoint + "/static/model/" + subId1 + "/" + subId2 + "/" + subId3 + "/" + id + ".xml");
        }

        /// <summary>
        /// Get large PNG for an asset id - for viewing only, not usable in game
        /// </summary>
        public void downloadLargeAssetPng(long assetId, string fileName)
        {
            string id = assetId.ToString();
            string subId1 = id.Substring(0, 3);
            string subId2 = id.Substring(3, 3);
            string subId3 = id.Substring(6, 3);

            string uri = endpoint + "/static/image/" + subId1 + "/" + subId2 + "/" + subId3 + "/" + id + "_lrg.png";

            using (var client = new WebClient())
            {
                client.DownloadFile(uri, fileName);
            }
        }

        /// <summary>
        /// Get small PNG for an asset id - usable in game
        /// </summary>
        public void downloadAssetPng(long assetId, string fileName)
        {
            string id = assetId.ToString();
            string subId1 = id.Substring(0, 3);
            string subId2 = id.Substring(3, 3);
            string subId3 = id.Substring(6, 3);

            string uri = endpoint + "/static/thumb/" + subId1 + "/" + subId2 + "/" + subId3 + "/" + id + ".png";

            using (var client = new WebClient())
            {
                client.DownloadFile(uri, fileName);
            }
        }

    }
}
