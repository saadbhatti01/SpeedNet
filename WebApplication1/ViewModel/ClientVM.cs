using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ClientVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public System.DateTime Date { get; set; }
        public string Amount { get; set; }
        public System.DateTime FullPaymentuptodate { get; set; }
        public System.DateTime EntryDate { get; set; }
    }
}