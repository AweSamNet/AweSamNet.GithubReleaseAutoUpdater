# AweSamNet.Integration.GithubReleaseAutoUpdater
Light-weight library to add application Auto-Updater using Github releases.

[![donate](https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif "PayPal - The safer, easier way to pay online!")](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=47QDCELAD7PN2)

## License
See license details [here](/LICENSE.md).

## Usage

1. Create an UpdaterProvider that implements `IUpdaterProvider`
```c#
    public class MyPluginUpdaterProvider : IUpdaterProvider
    ...
```
2. 
```c#
    var pluginUpdater = new GitHubReleaseAutoUpdater(new MyPluginUpdaterProvider());
    pluginUpdater.CheckLatestVersion();
```

## NuGet

https://www.nuget.org/packages/AweSamNet.Integration.GithubReleaseAutoUpdater/ 

### To Install
```
    PM> Install-Package AweSamNet.Integration.GithubReleaseAutoUpdater
```
