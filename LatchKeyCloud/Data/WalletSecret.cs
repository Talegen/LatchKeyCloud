namespace LatchKeyCloud.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Contains an enumerated list of wallet account types
    /// </summary>
    public enum WalletAccountTypes
    {
        /// <summary>
        /// Account is a bank account
        /// </summary>
        BankAccount = 0,

        /// <summary>
        /// Account is a MasterCard
        /// </summary>
        MasterCard = 1,

        /// <summary>
        /// Account is a Visa
        /// </summary>
        Visa = 2,

        /// <summary>
        /// Account is a Discover
        /// </summary>
        Discover = 3,

        /// <summary>
        /// Account is a Amex
        /// </summary>
        AmEx = 4,
    }

    /// <summary>
    /// This class represents a wallet secret.
    /// </summary>
    public class WalletSecret : CreatedUpdatedBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalletSecret"/> class.
        /// </summary>
        public WalletSecret()
        {
            this.WalletSecretId = Guid.NewGuid();
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets the unique identity of the wallet identity.
        /// </summary>
        [Key]
        public Guid WalletSecretId { get; set; }

        /// <summary>
        /// Gets or sets the secret identity.
        /// </summary>
        [Required]
        public Guid SecretId { get; set; }

        /// <summary>
        /// Gets or sets the account type
        /// </summary>
        public WalletAccountTypes AccountType { get; set; }

        /// <summary>
        /// Gets or sets the entry account number
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets name on account
        /// </summary>
        [StringLength(100)]
        public string NameOnAccount { get; set; }

        /// <summary>
        /// Gets or sets the account expiration
        /// </summary>
        [StringLength(50)]
        public string AccountExpiration { get; set; }

        /// <summary>
        /// Gets or sets the account card security code
        /// </summary>
        [StringLength(5)]
        public string SecurityCode { get; set; }

        /// <summary>
        /// Gets or sets secret account pin
        /// </summary>
        [StringLength(10)]
        public string PIN { get; set; }

        #region Public Navigation Properties
        /// <summary>
        /// Gets or sets the related secret.
        /// </summary>
        public Secret Secret { get; set; }
        #endregion
        #endregion

        #region Public Methods
        /// <summary>
        /// This method is used to convert the card information to a safe string.
        /// </summary>
        /// <returns>Returns the account in a secure pattern output.</returns>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(this.AccountNumber) ? (this.AccountNumber.Length > 4 ? new string('*', this.AccountNumber.Length - 4) + this.AccountNumber.Substring(this.AccountNumber.Length - 4) : string.Empty) : string.Empty;
        }
        #endregion
    }
}
