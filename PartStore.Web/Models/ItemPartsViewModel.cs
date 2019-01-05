using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartStore.Core.StoreModels;

namespace PartStore.Web.Models
{
    public class ItemPartsViewModel
    {
        public ItemParts ItemPart { get; set; }
        public List<ItemParts> ItemParts { get; set; }
        public Items Car { get; set; } // Car info
    }
}
