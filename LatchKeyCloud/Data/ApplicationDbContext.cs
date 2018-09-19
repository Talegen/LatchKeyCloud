namespace LatchKeyCloud.Data
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// This class contains application database context.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">Contains the context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets a list of attributes.
        /// </summary>
        public DbSet<Attribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets a list of account secrets.
        /// </summary>
        public DbSet<AccountSecret> AccountSecrets { get; set; }

        /// <summary>
        /// Gets or sets a list of file secrets.
        /// </summary>
        public DbSet<FileSecret> FileSecrets { get; set; }

        /// <summary>
        /// Gets or sets user profile images within the identity server data store.
        /// </summary>
        public DbSet<UserProfileImage> UserProfileImages { get; set; }

        /// <summary>
        /// Gets or sets a list of secrets.
        /// </summary>
        public DbSet<Secret> Secrets { get; set; }

        /// <summary>
        /// Gets or sets a list of wallet secrets.
        /// </summary>
        public DbSet<WalletSecret> WalletSecrets { get; set; }
        
        #region Entity Framework Protected Methods
        /// <summary>
        /// This method is called automatically when generating models inside EF code-first.
        /// </summary>
        /// <param name="builder">Contains an instance of the EF model builder.</param>
        /// <remarks>
        /// This is the method where specifics about entities inside the overall database design is specified.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // build the initial identity models.
            base.OnModelCreating(builder);

            // Default Identity names (e.g. AspNetUsers) are ugly. Use clean up some table and column names.
            builder.Entity<User>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<IdentityRole>().ToTable("Roles").Property(p => p.Id).HasColumnName("RoleId");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims").Property(p => p.Id).HasColumnName("RoleClaimId");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims").Property(p => p.Id).HasColumnName("UserClaimId");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            builder.Entity<Secret>().Property(s => s.ExpirationAction).HasDefaultValue(ExpirationActions.Nothing);
            builder.Entity<Secret>().Property(s => s.Type).HasDefaultValue(SecretTypes.Account);

            // set all computed column sql statements
            string getUtcDate = "getutcdate()";
            builder.Entity<AccountSecret>().Property(a => a.CreatedDate).HasDefaultValueSql(getUtcDate);
            builder.Entity<Attribute>().Property(a => a.CreatedDate).HasDefaultValueSql(getUtcDate);
            builder.Entity<FileSecret>().Property(a => a.CreatedDate).HasDefaultValueSql(getUtcDate);
            builder.Entity<Secret>().Property(a => a.CreatedDate).HasDefaultValueSql(getUtcDate);
            builder.Entity<UserProfileImage>().Property(a => a.CreatedDate).HasDefaultValueSql(getUtcDate);
            builder.Entity<WalletSecret>().Property(a => a.CreatedDate).HasDefaultValueSql(getUtcDate);

            builder.Entity<UserProfileImage>().Property(a => a.UniqueId).HasDefaultValueSql("newid()");
        }
        #endregion
    }
}
