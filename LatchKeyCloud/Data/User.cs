namespace LatchKeyCloud.Data
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// This entity class represents a user within the application identity data store.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets or sets a date time value when the entity was created. 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a date time value when the user last logged-in.
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        #region Navigation Properties
        /// <summary>
        /// Gets or sets related profile image
        /// </summary>
        public UserProfileImage ProfileImage { get; set; }
        #endregion
    }
}
