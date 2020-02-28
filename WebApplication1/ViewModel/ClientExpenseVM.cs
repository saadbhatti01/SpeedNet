using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ClientExpenseVM
    {
        public int Id { get; set; }
        public string Expensetype { get; set; }
        public System.DateTime MonthDate { get; set; }
        public string Amount { get; set; }
        public System.DateTime EntryDate { get; set; }
    }
}