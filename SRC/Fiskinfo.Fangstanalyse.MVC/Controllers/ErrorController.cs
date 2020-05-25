using Microsoft.AspNetCore.Mvc;
using SintefSecure.Framework.SintefSecure.AspNetCore;
using SintefSecureBoilerplate.MVC.Constants;
using SintefSecureBoilerplate.MVC.Constants.ErrorController;

namespace SintefSecureBoilerplate.MVC.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Provides methods that respond to HTTP requests with HTTP errors.
    /// </summary>
    [Route("[controller]")]
    public sealed class ErrorController : Controller
    {
        #region Public Methods

        /// <summary>
        /// Gets the error view for the specified HTTP error status code. Returns a <see cref="PartialViewResult"/> if
        /// the request is an Ajax request, otherwise returns a full <see cref="ViewResult"/>.
        /// </summary>
        /// <param name="statusCode">The HTTP error status code.</param>
        /// <param name="status">The name of the HTTP error status code e.g. 'notfound'. This is not used but is here
        /// for aesthetic purposes.</param>
        /// <returns>A <see cref="PartialViewResult"/> if the request is an Ajax request, otherwise returns a full
        /// <see cref="ViewResult"/> containing the error view.</returns>
        [ResponseCache(CacheProfileName = CacheProfileName.Error)]
        [HttpGet("{statusCode:int:range(400, 599)}/{status?}", Name = ErrorControllerRoute.GetError)]
        public IActionResult Error(int statusCode, string status)
        {
            Response.StatusCode = statusCode;

            ActionResult result;
            if (Request.IsAjaxRequest())
            {
                // This allows us to show errors even in partial views.
                result = PartialView(ErrorControllerAction.Error, statusCode);
            }
            else
            {
                result = View(ErrorControllerAction.Error, statusCode);
            }

            return result;
        }

        #endregion
    }
}