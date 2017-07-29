using System;
using Microsoft.AspNetCore.Mvc;
using Orbital.Models;
using Orbital.Web.Models;
using Orbital.Web.Users;

namespace Orbital.Web.ViewComponents
{
    [ViewComponent(Name = "VersionInfo")]
    public class VersionInfoViewComponent : ViewComponent
    {
        private readonly IUserQuery _userQuery;

        public VersionInfoViewComponent(IUserQuery userQuery)
        {
            _userQuery = userQuery;
        }

        public IViewComponentResult Invoke(VersionInfo versionInfo)
        {
            return View(ToViewModel(versionInfo));
        }

        private VersionInfoViewModel ToViewModel(VersionInfo versionInfo)
        {
            return new VersionInfoViewModel(
                ToViewModel(versionInfo.Created),
                ToViewModel(versionInfo.Modified),
                ToViewModel(versionInfo.Deleted));
        }
        private VersionInfoEventViewModel ToViewModel(VersionInfoEvent e)
        {
            if (e == null)
            {
                return null;
            }

            var user = _userQuery.GetById(e.By);

            return new VersionInfoEventViewModel(user, e.On);
        }
    }

    public class VersionInfoViewModel
    {
        public VersionInfoViewModel(VersionInfoEventViewModel created, VersionInfoEventViewModel modified, VersionInfoEventViewModel deleted)
        {
            Created = created;
            Modified = modified;
            Deleted = deleted;
        }

        public VersionInfoEventViewModel Created { get; }
        public VersionInfoEventViewModel Modified { get; }
        public VersionInfoEventViewModel Deleted { get; }

    }

    public class VersionInfoEventViewModel
    {
        public VersionInfoEventViewModel(User by, DateTime on)
        {
            By = by;
            On = on;
        }

        public User By { get; }
        public DateTime On { get; }

    }
}
