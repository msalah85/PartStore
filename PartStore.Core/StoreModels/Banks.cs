using SysLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartStore.Core.StoreModels
{
    public partial class Banks
    {
        public Banks()
        {
            PaymentsFromBank = new HashSet<Payments>();
            PaymentsToBank = new HashSet<Payments>();
            Transactions = new HashSet<Transactions>();
        }

        public int Id { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Lang))]
        public string Name { get; set; }
        [Display(Name = "Cat", ResourceType = typeof(Lang))]
        public int? TypeId { get; set; }

        //[ForeignKey("Id")]
        //[InverseProperty("FromBank")]
        public ICollection<Payments> PaymentsFromBank { get; set; }

        //[ForeignKey("Id")]
        //[InverseProperty("ToBank")]
        public ICollection<Payments> PaymentsToBank { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
