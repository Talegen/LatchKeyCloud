namespace LatchKeyCloud.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// This class represents additional secret metadata related to an account.
    /// </summary>
    public class AccountSecret : CreatedUpdatedBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountSecret"/> class.
        /// </summary>
        public AccountSecret()
        {
            this.AccountSecretId = Guid.NewGuid();
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets the account secret identity value.
        /// </summary>
        [Key]
        public Guid AccountSecretId { get; set; }

        /// <summary>
        /// Gets or sets the related secret identity value.
        /// </summary>
        [Required]
        public Guid SecretId { get; set; }

        /// <summary>
        /// Gets or sets the login name.
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password text.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password text hint.
        /// </summary>
        public string Hint { get; set; }

        #region Public Navigation Properties
        /// <summary>
        /// Gets or sets the related secret.
        /// </summary>
        public Secret Secret { get; set; }
        #endregion
        #endregion
    }
}
