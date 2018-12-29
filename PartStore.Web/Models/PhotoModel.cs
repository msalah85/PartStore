using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using PartStore.Core.StoreModels;
using System.Collections.Generic;

namespace PartStore.Web.Models
{
    public class PhotoModel
    {
        public Items Car { get; set; }
        public FormFile files { get; set; }
        public List<Photos> Photo { get; set; }
    }
}
