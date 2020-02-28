using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RecentPayment
    {
        public string Name { get; set; }
        public string PaymentMonth { get; set; }
        public string PaidAmount { get; set; }
        public string DueAmount { get; set; }
        public DateTime EntryDate { get; set; }
    }
}