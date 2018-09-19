namespace LatchKeyCloud.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// This class represents a secret custom attribute.
    /// </summary>
    public class Attribute : CreatedUpdatedBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attribute"/> class.
        /// </summary>
        public Attribute()
        {
            this.AttributeId = Guid.NewGuid();
            this.Displayed = true;
        }

        /// <summary>
        /// Gets or sets the unique identity of the custom attribute.
        /// </summary>
        [Key]
        public Guid AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the Secret Identity
        /// </summary>
        public Guid SecretId { get; set; }

        /// <summary>
        /// Gets or sets the unique name of the attribute.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the attribute.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute is displayable.
        /// </summary>
        public bool Displayed { get; set; }

        #region Navigation Properties
        /// <summary>
        /// Gets or sets the navigation properties.
        /// </summary>
        public Secret Secret { get; set; }
        #endregion
    }
}
