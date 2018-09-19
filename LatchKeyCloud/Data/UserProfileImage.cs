namespace LatchKeyCloud.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// This entity class represents a user profile image within the application data store.
    /// </summary>
    public class UserProfileImage : CreatedUpdatedBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileImage"/> class.
        /// </summary>
        public UserProfileImage()
        {
            this.UniqueId = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the unique identity of the associated user.
        /// </summary>
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a unique identity for the stored image.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the image binary data.
        /// </summary>
        public byte[] Content { get; set; }
    }
}
