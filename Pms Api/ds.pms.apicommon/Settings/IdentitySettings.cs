using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.apicommon.Settings
{
    /// <summary>
    /// Identity API settings
    /// </summary>
    public class IdentitySettings
    {
        /// <summary>
        /// This is token encryption secret
        /// </summary>
        public string JwtSecret { get; set; }

        /// <summary>
        /// Keep valid user token for x seconds
        /// </summary>
        public int JwtExpirationSeconds { get; set; }
        /// <summary>
        /// Keep valid user token for x seconds if Remember me is checked
        /// </summary>
        public int JwtExpirationSecondsRememberMe { get; set; }
    }
}
