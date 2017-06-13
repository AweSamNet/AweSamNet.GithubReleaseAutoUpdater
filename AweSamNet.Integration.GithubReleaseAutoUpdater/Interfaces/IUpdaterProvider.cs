using AweSamNet.Integration.GithubReleaseAutoUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AweSamNet.Integration.GithubReleaseAutoUpdater.Interfaces
{
    public interface IUpdaterProvider
    {
        /// <summary>
        /// Path to the directory the latest release should be downloaded to.
        /// </summary>
        string DownloadLocation { get; }

        /// <summary>
        /// Location of the file containing a list of versions.
        /// </summary>
        Uri VersionLocation { get; }

        /// <summary>
        /// Name representing the application using the updater.
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Owner of the repository to download the release from.
        /// </summary>
        string RepositoryOwner { get; }

        /// <summary>
        /// Name of the repository to download the release from.
        /// </summary>
        string RepositoryName { get; }

        /// <summary>
        /// Returns the settings stored locally.
        /// </summary>
        Settings GetSettings();

        /// <summary>
        /// Saves the settings locally.
        /// </summary>
        void SaveSettings(Settings settings);

        /// <summary>
        /// Prompts the user that a new version is available and returns <see cref="AweSamNet.Integration.GithubReleaseAutoUpdater.Models.NewVersionOptions"/> as the response. 
        /// </summary>
        /// <param name="currentVersion">Currently installed version.</param>
        /// <param name="latestVersion">Latest available version.</param>
        /// <param name="releaseNotes">Release notes of all missed versions.</param>
        NewVersionOptions PromptNewVersion(string currentVersion, string latestVersion, string releaseNotes);

        /// <summary>
        /// Prompts the user that a new version is available and returns <see cref="AweSamNet.Integration.GithubReleaseAutoUpdater.Models.NewVersionOptions"/> as the response. 
        /// </summary>
        /// <param name="latestVersion">Latest available version.</param>
        /// <param name="releaseNotes">Release notes of all missed versions.</param>
        NewVersionOptions PromptNewVersion(string latestVersion, string releaseNotes);

        /// <summary>
        /// Action to execute after a successful download.
        /// </summary>
        /// <param name="filePath"></param>
        void AfterDownload(string filePath);

        /// <summary>
        /// Gets the currently installed version.
        /// </summary>
        /// <returns></returns>
        GitHubVersion GetCurrentVersion();
    }
}
