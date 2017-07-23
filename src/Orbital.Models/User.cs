using System;

namespace Orbital.Models
{
    public class User
    {
        public User(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        
        public string Name { get; }
    }
}
