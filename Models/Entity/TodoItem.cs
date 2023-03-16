using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Entity
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public string Secret { get; set; }
    }
}
