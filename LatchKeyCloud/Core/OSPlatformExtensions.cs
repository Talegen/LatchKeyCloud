namespace LatchKeyCloud.Core
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class contains extension methods for working with operating systems.
    /// </summary>
    public static class OSPlatformExtensions
    {
        /// <summary>
        /// Gets the current operating system version.
        /// </summary>
        /// <returns>Returns a Version object if found. Otherwise returns an empty version.</returns>
        public static Version GetVersion()
        {
            Version result = new Version();
            string[] descriptionStringParts = RuntimeInformation.OSDescription.Split(' ');

            // find the numeric string
            if (descriptionStringParts != null && descriptionStringParts.Length > 0)
            {
                int i = 0;
                while (i < descriptionStringParts.Length)
                {
                    if (descriptionStringParts[i].Contains(".", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = new Version(descriptionStringParts[i]);
                        break;
                    }
                    ++i;
                }
            }

            return result;
        }
    }
}
