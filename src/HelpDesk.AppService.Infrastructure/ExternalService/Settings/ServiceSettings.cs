namespace HelpDesk.AppService.Infrastructure.ExternalService.Settings
{
    public sealed class ServiceSettings
    {
        #region Constants

        public const string SettingsKey = "ExternalService";

        #endregion

        #region Properties

        public string Url { get; set; }
        public string TokenName { get; set; }
        public int RequestTimeoutInSeconds { get; set; } = 60;

        #endregion
    }
}
