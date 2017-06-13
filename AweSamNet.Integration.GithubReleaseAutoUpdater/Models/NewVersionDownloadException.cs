using System;

namespace AweSamNet.Integration.GithubReleaseAutoUpdater.Models
{
    public class NewVersionDownloadException : Exception
    {
        public NewVersionDownloadException(string tagName, string asset, string downloadPath, Exception innerException) 
            : base($"Could not download version: '{tagName}', filename: '{asset}' from url: '{downloadPath}'", innerException)
        {            
        }
    }
}
