using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string NotificationMessage { get; set; }
        public int IsShow { get; set; }
    }
}