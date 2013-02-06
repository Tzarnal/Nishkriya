using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nishkriya.Models.ViewModels
{
    public class PaginatorViewModel
    {
        public int PageIndex { get; set; } //current element in the paginator
        public int TotalPages { get; set; } //maximum number of elements
        public int MaximumSpread { get; set; } //amount of elements to the side of the current element
        
        public string Action { get; set; } //Controller for generated links
        public string Controller { get; set; } //Action for generated links
        public int? ContentId { get; set; } //Optional Id 
    }
}