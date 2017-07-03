using System;

namespace Orbital.Data
{
    public interface IEntity
    {
        Guid Id { get; set; }

        bool Deleted { get; set; }
    }
}
