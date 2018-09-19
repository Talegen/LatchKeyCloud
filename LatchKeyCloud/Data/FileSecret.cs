namespace LatchKeyCloud.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// This class represents a file secret.
    /// </summary>
    public class FileSecret : CreatedUpdatedBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSecret"/> class.
        /// </summary>
        public FileSecret()
        {
            this.FileSecretId = Guid.NewGuid();
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets the unique identity of the wallet identity.
        /// </summary>
        [Key]
        public Guid FileSecretId { get; set; }

        /// <summary>
        /// Gets or sets the secret identity.
        /// </summary>
        [Required]
        public Guid SecretId { get; set; }

        /// <summary>
        /// Gets or sets the content length.
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file contents.
        /// </summary>
        public byte[] Content { get; set; }
    
        #region Public Navigation Properties
        /// <summary>
        /// Gets or sets the related secret.
        /// </summary>
        public Secret Secret { get; set; }
        #endregion
        #endregion
    }
}
