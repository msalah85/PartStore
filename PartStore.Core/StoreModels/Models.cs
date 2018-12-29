using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Models
    {
        public Models()
        {
            Items = new HashSet<Items>();
        }

        public int ModelId { get; set; }
        [Display(Name = "Make", ResourceType = typeof(Lang))]
        public int? MakeId { get; set; }
        [Display(Name = "ModelName", ResourceType = typeof(Lang))]
        public string ModelName { get; set; }

        [Display(Name = "Make", ResourceType = typeof(Lang))]
        public Makes Make { get; set; }
        public ICollection<Items> Items { get; set; }
    }
}
