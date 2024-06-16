using System;

namespace HelpDesk.AppService.Infrastructure.Cluster.Settings
{
    public sealed class ClusterSettings
    {
        #region Constants

        public const string SettingsKey = "Cluster";

        #endregion

        #region Properties

        public string Hostname => Environment.MachineName;
        public string AppName { get; set; } = "HelpDesk";
        public string AuthorName { get; set; } = "GRUPO 37";
        public string AuthorUrl { get; set; } = "https://github.com/fiap-2nett";
        public string SwaggerUrl { get; set; } = "https://localhost:5001/swagger/index.html";
        public string ProjectUrl { get; set; } = "https://github.com/fiap-2nett/tc-fase4-clean-architecture";
        public string WikiUrl { get; set; } = "https://github.com/fiap-2nett/tc04-Wiki-temp/wiki";
        public string LogsUrl { get; set; } = "http://localhost:8081";

        #endregion
    }
}
