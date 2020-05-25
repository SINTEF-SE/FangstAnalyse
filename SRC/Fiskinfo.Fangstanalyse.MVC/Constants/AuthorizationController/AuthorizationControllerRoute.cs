using Microsoft.CodeAnalysis.Operations;

namespace SintefSecureBoilerplate.MVC.Constants.AuthorizationController
{
    public class AuthorizationControllerRoute
    {
        public const string Authorize = ControllerName.Authorization + nameof(Authorize);
        public const string Logout = ControllerName.Authorization + nameof(Logout);
        public const string Token = ControllerName.Authorization + nameof(Token);
        public const string UserInfo = ControllerName.Authorization + nameof(UserInfo);
    }
}