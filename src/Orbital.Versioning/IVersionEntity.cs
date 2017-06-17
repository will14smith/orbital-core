using System;
using System.Collections.Generic;

namespace Orbital.Versioning
{
    public interface IVersionEntity<out TEntity>
    {
        long Id { get; set; }
        DateTime Date { get; set; }

        TEntity ToEntity();
        IReadOnlyDictionary<string, object> ToMetadata();
    }
}