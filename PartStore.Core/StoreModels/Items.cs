using System;
using System.Collections.Generic;

namespace PartStore.Core.StoreModels
{
    public partial class Items
    {
        public Items()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
            Photos = new HashSet<Photos>();
        }

        public long ItemsPk { get; set; }
        public long ItemId { get; set; }
        public string Barcode { get; set; }
        public decimal? AvgCost { get; set; }
        public decimal? LastCost { get; set; }
        public DateTime? LastPurchasedDate { get; set; }
        public int? Qty { get; set; }
        public bool? Starred { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? YearId { get; set; }
        public string Vin { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NetPrice { get; set; }
        public bool? Active { get; set; }
        public string Photo { get; set; }
        public string More { get; set; }

        public Makes Make { get; set; }
        public Models Model { get; set; }
        public Years Year { get; set; }
        public ICollection<InvoiceDetails> InvoiceDetails { get; set; }
        public ICollection<Photos> Photos { get; set; }
    }
}
