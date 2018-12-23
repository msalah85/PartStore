using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class Models
    {
        public Models()
        {
            Items = new HashSet<Items>();
        }

        public int ModelId { get; set; }
        public int? MakeId { get; set; }
        public string ModelName { get; set; }

        public Makes Make { get; set; }
        public ICollection<Items> Items { get; set; }
    }
}
