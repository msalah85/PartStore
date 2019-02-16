using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class Items
    {
        public Items()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
            ItemParts = new HashSet<ItemParts>();
            Photos = new HashSet<Photos>();
        }

        public long ItemsPk { get; set; } = 0; // manual serial.
        public long ItemId { get; set; }
        [Display(Name = "Barcode", ResourceType = typeof(Lang))]
        public string Barcode { get; set; }
        public decimal? AvgCost { get; set; }
        [Display(Name = "PurchasePrice", ResourceType = typeof(Lang))]
        public decimal? LastCost { get; set; }
        public DateTime? LastPurchasedDate { get; set; }
        [Display(Name = "Quantity", ResourceType = typeof(Lang))]
        public int? Qty { get; set; }
        public bool? Starred { get; set; }
        [Display(Name = "Make", ResourceType = typeof(Lang))]
        public int? MakeId { get; set; }
        [Display(Name = "Model", ResourceType = typeof(Lang))]
        public int? ModelId { get; set; }
        public int? YearId { get; set; }
        [Display(Name = "VIN", ResourceType = typeof(Lang))]
        public string Vin { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NetPrice { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Lang))]
        public bool? Active { get; set; }
        public string Photo { get; set; }
        [Display(Name = "More", ResourceType = typeof(Lang))]
        public string More { get; set; }
        [Display(Name = "LotNo", ResourceType = typeof(Lang))]
        public string LotNo { get; set; }
        [Display(Name = "Supplier", ResourceType = typeof(Lang))]
        public int? SupplierId { get; set; }
        [Display(Name = "RefNo", ResourceType = typeof(Lang))]
        public string RefNo { get; set; }
        [Display(Name = "SupplierCarNo", ResourceType = typeof(Lang))]
        public string SupplierCarNo { get; set; }
        [Display(Name = "Sold", ResourceType = typeof(Lang))]
        public bool? Sold { get; set; } = false;
        [Display(Name = "Make", ResourceType = typeof(Lang))]
        public Makes Make { get; set; }
        [Display(Name = "ModelName", ResourceType = typeof(Lang))]
        public Models Model { get; set; }
        [Display(Name = "Year", ResourceType = typeof(Lang))]
        public Years Year { get; set; }
        [Display(Name = "Supplier", ResourceType = typeof(Lang))]
        public Accounts Supplier { get; set; }
        public ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public ICollection<Photos> Photos { get; set; }
        public ICollection<ItemParts> ItemParts { get; set; }

        public static implicit operator Items(List<InvoiceDetails> v)
        {
            throw new NotImplementedException();
        }
    }
}
