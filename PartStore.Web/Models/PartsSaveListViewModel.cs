using PartStore.Core.StoreModels;
using System.Collections.Generic;

namespace PartStore.Web.Models
{
    public class PartsSaveListViewModel
    {
        public Items Item { get; set; }
        public List<Parts> Parts { get; set; }
    }
}
