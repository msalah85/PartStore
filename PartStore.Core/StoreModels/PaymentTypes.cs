using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class PaymentTypes
    {
        public PaymentTypes()
        {
            Payments = new HashSet<Payments>();
        }

        public int Id { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Lang))]
        public string Name { get; set; }

        public ICollection<Payments> Payments { get; set; }
    }
}
