using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class Operations
    {
        public Operations()
        {
            Payments = new HashSet<Payments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Payments> Payments { get; set; }
    }
}
