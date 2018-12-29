using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Makes
    {
        public Makes()
        {
            Items = new HashSet<Items>();
            Models = new HashSet<Models>();
        }

        public int MakeId { get; set; }
        [Display(Name = "Make", ResourceType = typeof(Lang))]
        public string MakeName { get; set; }

        public ICollection<Items> Items { get; set; }
        public ICollection<Models> Models { get; set; }
    }
}
