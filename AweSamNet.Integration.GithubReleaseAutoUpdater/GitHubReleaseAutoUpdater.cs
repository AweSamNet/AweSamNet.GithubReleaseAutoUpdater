using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Octokit;
using Newtonsoft.Json;
using System.IO;
using AweSamNet.Integration.GithubReleaseAutoUpdater.Models;
using AweSamNet.Integration.GithubReleaseAutoUpdater.Interfaces;

namespace AweSamNet.Integration.GitHubReleaseAutoUpdater
{
    public class GitHubReleaseAutoUpdater
    {
        public GitHubReleaseAutoUpdater(IUpdaterProvider updaterProvider)
        {
            if (updaterProvider == null) throw new ArgumentNullException("updaterProvider is required.");

            UpdaterProvider = updaterProvider;
        }

        public IUpdaterProvider UpdaterProvider { get; private set; }

        /// <summary>
        /// Downloads a specified version to the destination location.
        /// </summary>
        public async Task GetVersion(GitHubVersion version, string destination)
        {
            await GetVersion(version.TagName, version.AssetName, destination);
        }

        /// <summary>
        /// Downloads a specified version to the destination location.
        /// </summary>
        public async Task GetVersion(string tagName, string assetName, string destination)
        {
            var client = new GitHubClient(new ProductHeaderValue(UpdaterProvider.ApplicationName));
            var releases = await client.Repository.Release.GetAll(UpdaterProvider.RepositoryOwner, UpdaterProvider.RepositoryName);
            var currentRelease = releases.Single(x => x.TagName == tagName);
            var asset = currentRelease.Assets.Single(x => x.Name == assetName);

            using (var webClient = new WebClient())
            {

                try
                {
                    webClient.DownloadFile(asset.BrowserDownloadUrl, destination);
                }
                catch (Exception ex)
                {
                    throw new NewVersionDownloadException(tagName, assetName, asset.BrowserDownloadUrl, ex);
                }
            }
        }

        /// <summary>
        /// Checks for the latest version and manages the download of a newer version.
        /// </summary>
        public async void CheckLatestVersion()
        {
            var settings = UpdaterProvider.GetSettings();
            GitHubVersion latest = GetLatestVersion();

            if (latest == null) return;

            List<GitHubVersion> releases = GetAllVersions();

            var current = UpdaterProvider.GetCurrentVersion();

            if (settings.LatestSkippedVersion == latest.TagName || current?.TagName == latest.TagName) return;
            
            List<GitHubVersion> missedReleases = new List<GitHubVersion>();

            // get the missed releases
            if (current != null)
            {
                var index = releases.IndexOf(current);
                missedReleases = releases.GetRange(index, releases.Count - index + 1);
            }
            else
            {
                missedReleases = releases;
            }

            // build the release notes
            var client = new GitHubClient(new ProductHeaderValue(UpdaterProvider.ApplicationName));
            var gitReleases = await client.Repository.Release.GetAll(UpdaterProvider.RepositoryOwner, UpdaterProvider.RepositoryName);
            var missedGitReleases = gitReleases.Where(x => missedReleases.Any(r => r.TagName == x.TagName));

            var releaseNotes = string.Join(Environment.NewLine, missedGitReleases.Select(x => x.Body));

            var response = UpdaterProvider.PromptNewVersion(current?.TagName, latest.TagName, releaseNotes);

            switch (response)
            {
                case NewVersionOptions.DownloadUpdate:
                    var filePath = Path.Combine(UpdaterProvider.DownloadLocation, latest.AssetName);
                    await GetVersion(latest, filePath);
                    UpdaterProvider.AfterDownload(filePath);
                    settings.LatestSkippedVersion = null;
                    break;
                case NewVersionOptions.SkipThisVersion:
                    settings.LatestSkippedVersion = latest.TagName;
                    break;
                case NewVersionOptions.RemindMeLater:
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns the latest version available online.
        /// </summary>
        public GitHubVersion GetLatestVersion()
        {
             return GetAllVersions().LastOrDefault();
        }

        /// <summary>
        /// Returns all available version online.
        /// </summary>
        private List<GitHubVersion> GetAllVersions()
        {
            string json;
            List<GitHubVersion> releases;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(UpdaterProvider.VersionLocation);
                    releases = JsonConvert.DeserializeObject<List<GitHubVersion>>(json);
                }
            }
            catch (Exception ex)
            {
                throw new GetAllVersionsException(UpdaterProvider.VersionLocation, ex);
            }
            return releases;
        }

        /// <summary>
        /// Returns the currently installed version.
        /// </summary>
        public GitHubVersion GetCurrentVersion()
        {
            return UpdaterProvider.GetCurrentVersion();
        }
    }
}
