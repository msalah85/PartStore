using SysLanguages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartStore.Core.StoreModels
{
    public partial class AccountTypes
    {
        public AccountTypes()
        {
            Accounts = new HashSet<Accounts>();
        }

        public byte Id { get; set; }
        [Display(Name = "AccountTypeName", ResourceType = typeof(Lang))]
        public string Name { get; set; }

        public ICollection<Accounts> Accounts { get; set; }
    }
}
