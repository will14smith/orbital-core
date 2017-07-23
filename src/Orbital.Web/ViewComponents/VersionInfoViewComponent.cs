using Microsoft.AspNetCore.Mvc;
using Orbital.Web.Models;

namespace Orbital.Web.ViewComponents
{
    [ViewComponent(Name = "VersionInfo")]
    public class VersionInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(VersionInfo versionInfo)
        {
            return View(versionInfo);
        }
    }
}
