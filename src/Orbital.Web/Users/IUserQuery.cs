using System;
using Orbital.Models;

namespace Orbital.Web.Users
{
    public interface IUserQuery
    {
        User GetById(Guid id);
    }
}
