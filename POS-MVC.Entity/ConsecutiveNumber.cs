using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class ConsecutiveNumber
    {
        public int ConsecutiveNumberId { get; set; }
        public int? LastNumber { get; set; }
        public int? DigitsNumber { get; set; }
        public string? TransactionType { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
