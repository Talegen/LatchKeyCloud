namespace LatchKeyCloud.Data
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// This abstract class implements common create and update date properties.
    /// </summary>
    public abstract class CreatedUpdatedBase
    {
        /// <summary>
        /// Gets or sets a date time value when the entity was created.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a date time value when the entity was last updated.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedDate { get; set; }
    }
}
