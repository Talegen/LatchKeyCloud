namespace LatchKeyCloud.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LatchKeyCloud.Core;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// This class contains data initialization for migrations.
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// This method is used to implement default application data in the database.
        /// </summary>
        /// <param name="dbContext">Contains the database context.</param>
        /// <param name="environment">Contains the hosting environment.</param>
        public static void InitializeApplicationData(this ApplicationDbContext dbContext, IHostingEnvironment environment)
        {
        }

        /// <summary>
        /// This method is used to implement default user in the database.
        /// </summary>
        /// <param name="dbContext">Contains the database context.</param>
        /// <param name="userManager">Contains the user manager.</param>
        public static void InitializeDefaultUserData(this ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            // if there are no users...
            if (!dbContext.Users.Any())
            {
                // create a default user.
                var defaultUser = dbContext.Users.FirstOrDefault(u => u.Email == "rob@talegen.com");

                if (defaultUser == null)
                {
                    // we need to create the default user
                    defaultUser = new User { UserName = "rob@talegen.com", Email = "rob@talegen.com" };

                    AsyncHelper.RunSync(() => userManager.CreateAsync(defaultUser, "Testing123!"));
                }
            }
        }
    }
}
