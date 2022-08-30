using ds.pms.apicommon.Settings;
using ds.pms.bl.users.Models;
using ds.pms.bl.users.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ds.pms.api.Controllers
{
    /// <summary>
    /// Base controller containing all shared functionality among API controllers
    /// </summary>
    //public class BaseController<T> : ControllerBase where T : class
    public class BaseController: ControllerBase
    {
        public ILogger<UserService> baseUserServiceLogger;

        /// <summary>
        /// Operations database connection string settings
        /// </summary>
        public ConnectionSettings connectionSettings;

        /// <summary>
        /// Identity API settings
        /// </summary>
        public IdentitySettings identitySettings;

        /// <summary>
        /// Returns current logged in user
        /// </summary>
        protected User CurrentUser
        {
            get
            {
                if (identitySettings != null && !string.IsNullOrEmpty(identitySettings.JwtSecret) && User != null)
                {
                    UserService userService =
                        new UserService(connectionSettings.OperationsDbSqlProvider,
                        connectionSettings.OperationsDbConnString, identitySettings.JwtSecret,
                        identitySettings.JwtExpirationSeconds, baseUserServiceLogger);

                    User user = userService.GetCurrentUser(User);
                    return user;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns flag if user is logged in
        /// </summary>
        protected bool IsUserAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }

        /// <summary>
        /// Base constructor for all API controllers
        /// </summary>
        /// <param name="iConnectionSettings"></param>
        public BaseController(IOptions<ConnectionSettings> iConnectionSettings)
        {
            connectionSettings = iConnectionSettings.Value;
        }

        public BaseController(IOptions<ConnectionSettings> iConnectionSettings, IOptions<IdentitySettings> iIdentitySettings)
        {
            connectionSettings = iConnectionSettings.Value;
            identitySettings = iIdentitySettings.Value;
        }

        public BaseController(IOptions<ConnectionSettings> iConnectionSettings, IOptions<IdentitySettings> iIdentitySettings, ILogger<UserService> UserServiceServiceLogger)
        {
            connectionSettings = iConnectionSettings.Value;
            identitySettings = iIdentitySettings.Value;
            baseUserServiceLogger = UserServiceServiceLogger;
        }
    }
}
