using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class ExpenseListVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public HttpPostedFile ImageFile { get; set; }
    }
}