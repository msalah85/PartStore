using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Photos
    {
        public long PhotoId { get; set; }
        [Display(Name = "Car", ResourceType = typeof(Lang))]
        public long ItemId { get; set; }
        [Display(Name = "Url", ResourceType = typeof(Lang))]
        public string Url { get; set; }
        [Display(Name = "IsMain", ResourceType = typeof(Lang))]
        public bool? Main { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Lang))]
        public bool? Active { get; set; }

        [Display(Name = "Car", ResourceType = typeof(Lang))]
        public Items Item { get; set; }
    }
}
