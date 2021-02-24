using System;
using SporeDownloader;

string mode = args[0];

switch (mode)
{
    case "user":
        string username = args[1];
        new SporeUser(username).downloadAllAssets("spore_assets\\users");
        break;

    case "sporecast":
        long id = long.Parse(args[1]);
        new Sporecast(id).downloadAllAssets("spore_assets\\sporecasts");
        break;

    default:
        Console.WriteLine("Invalid mode. Run with 'user' or 'sporecast', followed by the username or ID.");
        break;
}