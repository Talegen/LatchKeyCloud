namespace LatchKeyCloud.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;

    /// <summary>
    /// Defines an enumerated list of secret types.
    /// </summary>
    public enum SecretTypes
    {
        /// <summary>
        /// Password key stored
        /// </summary>
        Account,

        /// <summary>
        /// Credit Card info
        /// </summary>
        Wallet,

        /// <summary>
        /// Blob data.
        /// </summary>
        File
    }

    /// <summary>
    /// Contains an enumerated list of expiration actions
    /// </summary>
    public enum ExpirationActions
    {
        /// <summary>
        /// No Expiration
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// When expiration date met, highlight in list
        /// </summary>
        Highlight = 1,

        /// <summary>
        /// When expiration date met, show in prompt dialog
        /// </summary>
        Prompt = 3
    }

    /// <summary>
    /// This class represents a secure piece of information for the user.
    /// </summary>
    public class Secret : CreatedUpdatedBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Secret"/> class.
        /// </summary>
        protected Secret()
        {
            this.Type = SecretTypes.Account;
            this.SecretId = Guid.NewGuid();
            this.CreatedDate = DateTime.UtcNow;
            this.Attributes = new List<Attribute>();
            this.ExpirationAction = ExpirationActions.Nothing;
            this.ForegroundColor = ColorTranslator.ToHtml(SystemColors.ControlText);
            this.BackgroundColor = ColorTranslator.ToHtml(SystemColors.Window);
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets the unique identity of the record.
        /// </summary>
        [Key]
        public Guid SecretId { get; set; }

        /// <summary>
        /// Gets or sets the secret owner identity.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the secret type.
        /// </summary>
        public SecretTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the secret name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identity of the secret group.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }
        
        /// <summary>
        /// Gets or sets the expriation actions.
        /// </summary>
        public ExpirationActions ExpirationAction { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the URL of the secret.
        /// </summary>
        [StringLength(1000)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a note related to the secret.
        /// </summary>
        [StringLength(1000)]
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the related icon index for the secret entry.
        /// </summary>
        public int IconIndex { get; set; }

        /// <summary>
        /// Gets or sets the optional foreground color in hex format.
        /// </summary>
        [StringLength(6)]
        public string ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the optional background color in hex format.
        /// </summary>
        [StringLength(6)]
        public string BackgroundColor { get; set; }
        #endregion
        
        #region Navigation Properties
        /// <summary>
        /// Gets or sets the owner of the secret.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a list of custom attributes.
        /// </summary>
        public List<Attribute> Attributes { get; set; }
        #endregion
    }
}
