using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AweSamNet.Integration.GithubReleaseAutoUpdater.Models
{
    public class GetAllVersionsException : Exception
    {
        public GetAllVersionsException(Uri versionFileLocation, Exception innerException )
            : base($"Could not get existing versions information from {versionFileLocation}, try opening the url in a browser and inspect the result.", innerException)
        {}
    }
}
