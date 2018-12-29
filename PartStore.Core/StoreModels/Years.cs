using SysLanguages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Years
    {
        public Years()
        {
            Items = new HashSet<Items>();
        }

        [Display(Name = "Year", ResourceType = typeof(Lang))]
        public int YearId { get; set; }

        public ICollection<Items> Items { get; set; }
    }
}
