using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersistantGrabberRemover
{
    public class Package
    {
        [JsonPropertyName("main")]
        public string? Main { get; set; }

        [JsonPropertyName("name")]
        public string? Name = "discord_desktop_core";

        [JsonPropertyName("version")]
        public string Version = "0.0.0";

        [JsonPropertyName("private")]
        public bool Private = true;
    }
}
