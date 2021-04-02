using System;
using System.Collections.Generic;

namespace Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Author> Authors { get; set; } = new List<Author>();

        public override string ToString()
        {
            string authors = string.Join(", ", Authors);
            return $"{Id} {Name}; {authors}";
        }
    }
}
