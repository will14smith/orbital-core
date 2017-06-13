using System;

namespace Orbital.Data.Versioning
{
    public interface IVersionEntity<out T>
    {
        long Id { get; set; }
        DateTime Date { get; set; }

        T ToEntity();
    }
}