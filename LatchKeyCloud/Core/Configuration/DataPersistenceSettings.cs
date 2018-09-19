namespace LatchKeyCloud.Core.Configuration
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Contains an enumerated list of persistence storage methods.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataPersistenceStorageMethods
    {
        /// <summary>
        /// The default storage method is localized.
        /// </summary>
        Local,

        /// <summary>
        /// A file system path will be used instead.
        /// </summary>
        FileSystem,

        /// <summary>
        /// A Redis server will be used.
        /// </summary>
        Redis,

        /// <summary>
        /// Azure vault shall be used.
        /// </summary>
        AzureVault
    }

    /// <summary>
    /// This class contains data persistence settings.
    /// </summary>
    public class DataPersistenceSettings
    {
        /// <summary>
        /// Gets or sets the data persistence storage method.
        /// </summary>
        public DataPersistenceStorageMethods StorageMethod { get; set; }

        /// <summary>
        /// Gets or sets the data persistence folder path.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets data persistence thumbprint.
        /// </summary>
        public string Thumbprint { get; set; }

        /// <summary>
        /// Gets or sets data persistence 
        /// </summary>
        public string AzureBlobUriWithToken { get; set; }
    }
}
