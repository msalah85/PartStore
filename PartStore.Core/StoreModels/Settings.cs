using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Settings
    {
        public int Id { get; set; }

        [Display(Name = "Group", ResourceType = typeof(Lang))]
        public string GroupName { get; set; }
        [Display(Name = "Title", ResourceType = typeof(Lang))]
        public string Title { get; set; }
        [Display(Name = "Value", ResourceType = typeof(Lang))]
        public string Value { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Lang))]
        public bool? Active { get; set; }
    }
}
