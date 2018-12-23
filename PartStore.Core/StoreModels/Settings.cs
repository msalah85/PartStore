using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class Settings
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public bool? Active { get; set; }
    }
}
