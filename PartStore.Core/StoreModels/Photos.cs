using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class Photos
    {
        public long PhotoId { get; set; }
        public long ItemId { get; set; }
        public string Url { get; set; }
        public bool? Main { get; set; }
        public bool? Active { get; set; }

        public Items Item { get; set; }
    }
}
