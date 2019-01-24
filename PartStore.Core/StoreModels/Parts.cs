using SysLanguages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Parts
    {
        public Parts()
        {
            ItemParts = new HashSet<ItemParts>();
        }

        public int Id { get; set; }
        [Display(Name = "PartName", ResourceType = typeof(Lang))]
        public string Name { get; set; }

        public ICollection<ItemParts> ItemParts { get; set; }
    }
}
