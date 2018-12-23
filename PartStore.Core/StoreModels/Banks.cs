using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartStore.Core.StoreModels
{
    public partial class Banks
    {
        public Banks()
        {
            PaymentsFromBank = new HashSet<Payments>();
            PaymentsToBank = new HashSet<Payments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }

        //[ForeignKey("Id")]
        //[InverseProperty("FromBank")]
        public ICollection<Payments> PaymentsFromBank { get; set; }

        //[ForeignKey("Id")]
        //[InverseProperty("ToBank")]
        public ICollection<Payments> PaymentsToBank { get; set; }
    }
}
