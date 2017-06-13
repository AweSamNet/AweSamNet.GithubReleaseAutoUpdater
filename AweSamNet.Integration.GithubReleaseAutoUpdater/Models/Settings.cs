using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AweSamNet.Integration.GithubReleaseAutoUpdater.Models
{
    public class Settings
    {
        public string LatestSkippedVersion { get; set; }
        public bool EnableAutoUpdate { get; set; }
    }
}
