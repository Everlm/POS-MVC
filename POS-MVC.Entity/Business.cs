using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class Business
    {
        public int BusinessId { get; set; }
        public string? LogoUrl { get; set; }
        public string? LogoName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public decimal? TaxRate { get; set; }
        public string? CurrencySymbol { get; set; }
    }
}
