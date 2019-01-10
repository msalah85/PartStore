using SysLanguages;
using System;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class ItemParts
    {
        [Key]
        public long PartId { get; set; }
        [Display(Name = "Car", ResourceType = typeof(Lang))]
        public long ItemId { get; set; }
        [Display(Name = "PartName", ResourceType = typeof(Lang))]
        public string PartName { get; set; }
        [Display(Name = "Quantity", ResourceType = typeof(Lang))]
        public int Qty { get; set; } = 1;
        [Display(Name = "Barcode", ResourceType = typeof(Lang))]
        public string Barcode { get; set; }
        public decimal? AvgCost { get; set; }
        public decimal? LastCost { get; set; }
        [Display(Name = "Price", ResourceType = typeof(Lang))]
        public decimal? SalePrice { get; set; }
        [Display(Name = "More", ResourceType = typeof(Lang))]
        public string More { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Lang))]
        public bool? Active { get; set; }
        public bool? Starred { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? LastPurchasedDate { get; set; }
        [Display(Name = "AddDate", ResourceType = typeof(Lang))]
        public DateTime? AddDate { get; set; }
        [Display(Name = "Car", ResourceType = typeof(Lang))]
        public Items Item { get; set; }
    }
}
