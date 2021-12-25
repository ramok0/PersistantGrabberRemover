using PersistantGrabberRemover;
using System.Collections.Generic;
using System.Text.Json;

List<string> builds = new List<string>() { "Discord", "DiscordCanary", "DiscordDevelopment", "DiscordPTB" };

#nullable enable
string? appdata = Environment.GetEnvironmentVariable("userprofile");
#nullable disable

if (appdata == null)
{
    Console.WriteLine("Could not find Appdata variable.");
    Console.Write("Path to your %appdata% : ");
    appdata = Console.ReadLine();
}

appdata = Path.Join(appdata, "AppData", "Local");

builds.Where(build => Directory.Exists(Path.Join(appdata, build))).ToList().ForEach(build =>
{

    var path = Path.Combine(appdata, build);
    var appFolders = Directory.GetDirectories(path).Where(folder => folder.Split("\\").Last().StartsWith("app-"));
    if (appFolders.Count() == 0)
    {
        Console.WriteLine("No app folder found in " + build);
        return;
    }
    var appFolder = appFolders.First();
    if (!Directory.Exists(Path.Join(appFolder, "modules")))
    {
        Console.WriteLine("No modules folder found in " + build);
        return;
        
    }
    var DiscordDesktopFolders = Directory.GetDirectories(Path.Join(appFolder, "modules")).Where(folder => folder.Split("\\").Last().StartsWith("discord_desktop_core"));
    if (DiscordDesktopFolders.Count() == 0)
    {
        Console.WriteLine($"no Discord Desktop Core folder found in {build}");
        return;
        
    }
    var DiscordDesktopCore = DiscordDesktopFolders.First();
    if (!Directory.Exists(Path.Join(DiscordDesktopCore, "discord_desktop_core")))
    {
        Console.WriteLine($"no Discord Desktop Core\\Discord Desktop Core folder found in {build}");
        return;
 
    }
    var finaldirectory = Path.Join(DiscordDesktopCore, "discord_desktop_core");
    int safe = 0;
    if (File.Exists(Path.Join(finaldirectory, "index.js"))) {
        if (File.ReadAllText(Path.Join(finaldirectory, "index.js")) != "module.exports = require('./core.asar');") {
            File.WriteAllText(Path.Join(finaldirectory, "index.js"), "module.exports = require('./core.asar');");
            Console.WriteLine($"Grabber removed in {build}");
        } else
        {
            safe++;
        }
    }

    if (File.Exists(Path.Join(finaldirectory, "package.json"))) {
        var content = File.ReadAllText(Path.Join(finaldirectory, "package.json"));
        Package jsonpackaxe = JsonSerializer.Deserialize<Package>(content);
        if(jsonpackaxe.Main != "index.js")
        {
            jsonpackaxe.Main = "index.js";
            jsonpackaxe.Name = "discord_desktop_core";
            jsonpackaxe.Version = "0.0.0.0";
            jsonpackaxe.Private = true;
            var newcontent = JsonSerializer.Serialize(jsonpackaxe, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(Path.Join(finaldirectory, "package.json"), newcontent);
            Console.WriteLine($"Grabber removed in {build}");
        } else
        {
            safe++;
        }
    } else {
        safe++;
    }

    if(safe == 2)
    {
        Console.WriteLine($"{build} is safe.");
    }
});

//les grab de billy nez

List<string> billyed = new List<string>() { "riot.pyw", "plague.pyw" };
var roaming = Environment.GetEnvironmentVariable("APPDATA");
var startupgrabs = billyed.Where(grab => File.Exists(Path.Join(roaming, "Microsoft", "Windows", "Start Menu", "Programs", "Startup", grab))).ToList();
    if(startupgrabs.Count == 0) Console.WriteLine("No startup grab founds.");


    startupgrabs.ForEach(xd =>
{
    File.Delete(Path.Join(roaming, "Microsoft", "Windows", "Start Menu", "Programs", "Startup", xd));
    Console.WriteLine($"Grabber removed in your startup folder ({xd})");
});
