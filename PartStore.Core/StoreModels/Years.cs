using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class Years
    {
        public Years()
        {
            Items = new HashSet<Items>();
        }

        public int YearId { get; set; }

        public ICollection<Items> Items { get; set; }
    }
}
