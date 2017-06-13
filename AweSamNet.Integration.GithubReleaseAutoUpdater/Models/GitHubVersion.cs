using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AweSamNet.Integration.GithubReleaseAutoUpdater.Models
{
    public class GitHubVersion
    {
        public string TagName { get; set; }
        public string AssetName { get; set; }
    }
}
