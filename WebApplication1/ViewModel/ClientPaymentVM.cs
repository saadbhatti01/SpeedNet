using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ClientPaymentVM
    {
         public int Id { get; set; }
        public string Client { get; set; }
        public System.DateTime Month { get; set; }
        public string Due_Amount { get; set; }
        public string Paid_Amount { get; set; }
        public System.DateTime EntryDate { get; set; }
    }
}