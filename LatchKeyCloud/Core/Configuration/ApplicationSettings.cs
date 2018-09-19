using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LatchKeyCloud.Core.Configuration
{
    /// <summary>
    /// This class contains our application settings.
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettings"/> class.
        /// </summary>
        public ApplicationSettings()
        {
            this.MinimumCompletionPortThreads = 100;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to run in diagnostics mode.
        /// </summary>
        public bool ShowDiagnostics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the webserver forces SSL.
        /// </summary>
        public bool ForceSsl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the database migrations are automatically executed on startup.
        /// </summary>
        public bool AutoMigrate { get; set; }

        /// <summary>
        /// Gets or sets minimum completion port threads.
        /// </summary>
        public int MinimumCompletionPortThreads { get; set; }

        /// <summary>
        /// Gets or sets an optional insights instrumentation key.
        /// </summary>
        public string InsightsInstrumentationKey { get; set; }

        /// <summary>
        /// Gets or sets perisistence settings.
        /// </summary>
        public DataPersistenceSettings PersistenceSettings { get; set; }
    }
}
