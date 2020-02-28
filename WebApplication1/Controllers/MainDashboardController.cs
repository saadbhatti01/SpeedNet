using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [CheckSession]
    public class MainDashboardController : Controller
    {

        ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel();
         
        // GET: MainDashboard
        public ActionResult ClientsGridView()
        {
            TempData["Heading"] = 3;
            try {
                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var Eshtriackstype = new SelectList(ObjEntities.EshtiracksTypes.ToList(), "Id", "Description");
                    ViewData["EshtiracksTypes"] = Eshtriackstype;
                }

                var getClientData = ObjEntities.ClientsViews.OrderByDescending(c => c.Id).ToList();
                if (getClientData.Count != 0)
                {
                    List<ClientVM> ClientList = new List<ClientVM>();
                    foreach (var i in getClientData)
                    {
                        ClientVM v = new ClientVM();
                        v.Amount = i.Amount;
                        var getType = ObjEntities.EshtiracksTypes.Where(e => e.Id == i.Type).FirstOrDefault();
                        if (getType != null)
                        {
                            v.Type = getType.Description;
                        }
                        else
                        {
                            v.Type = "General";
                        }
                        v.Id = i.Id;
                        v.Name = i.Name;
                        v.Location = i.Location;
                        v.Date = i.Date;
                        v.FullPaymentuptodate = i.FullPaymentuptodate;
                        v.EntryDate = i.EntryDate;
                        ClientList.Add(v);


                    }
                    TempData["ClientData"] = ClientList;
                    
                }
            }
            catch (Exception ex) { }
            
            return View();
        }

        [HttpPost]
        public ActionResult ClientsGridView(ClientsView clientsview,FormCollection Form)
        {
            try {
                if (ModelState.IsValid)
                {

                    using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                    {
                        clientsview.Date = DateTime.Parse(clientsview.Date.ToShortTimeString());
                        clientsview.FullPaymentuptodate = DateTime.Parse(clientsview.FullPaymentuptodate.ToShortTimeString());
                        clientsview.EntryDate = DateTime.Parse(DateTime.Now.ToShortTimeString());
                        ObjEntities.ClientsViews.Add(clientsview);
                        ObjEntities.SaveChanges();
                        int id = clientsview.Id;
                        if (id > 0)
                        {
                            TempData["Success"] = "Successfully Saved";
                        }
                        else
                        {
                            TempData["Failed"] = "Not Saved";
                        }
                    }
                }
            }
            catch (Exception ex) { }
            
            //TempData["ClientData"] = ObjEntities.ClientsView.ToList();
            return RedirectToAction("ClientsGridView");
        }

        public ActionResult EditClientsGridView(Int32 id)
        {
            var clientsview = new ClientsView();
            try
            {
                clientsview = ObjEntities.ClientsViews.Where(x => x.Id == id).FirstOrDefault();
                DateTime myDateTime = new DateTime();
                if (clientsview != null)
                {
                    ViewBag.Id = id;
                    var Eshtriackstype = new SelectList(ObjEntities.EshtiracksTypes.ToList(), "Id", "Description");
                    ViewData["EshtiracksTypes"] = Eshtriackstype;

                }
                var date = clientsview.Date.ToString();
                if (date != "")
                {
                    myDateTime = Convert.ToDateTime(clientsview.Date.ToString());
                    ViewBag.Tdate = myDateTime.ToString("yyyy-MM-dd");
                }
                date = clientsview.FullPaymentuptodate.ToString();
                if (date != "")
                {
                    myDateTime = Convert.ToDateTime(clientsview.FullPaymentuptodate.ToString());
                    ViewBag.FDate = myDateTime.ToString("yyyy-MM-dd");
                }
                ViewBag.Id = id;
                TempData["Heading"] = 3;
                TempData["Success"] = "Successfully Updated";
            }
            catch(Exception ex) { }
          
            return View(clientsview);
        }

        [HttpPost]
        public ActionResult UpdateClientsGridView(ClientsView clientsview)
        {
            try
            {
                using (ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel())
                {
                    ClientsView updatedclientsview = (from c in entities.ClientsViews
                                                      where c.Id == clientsview.Id
                                                      select c).FirstOrDefault();

                    updatedclientsview.Name = clientsview.Name;
                    updatedclientsview.Location = clientsview.Location;
                    updatedclientsview.Type = clientsview.Type;
                    updatedclientsview.Date = clientsview.Date;
                    updatedclientsview.Amount = clientsview.Amount;
                    updatedclientsview.FullPaymentuptodate = clientsview.FullPaymentuptodate;
                    updatedclientsview.EntryDate = DateTime.Now;
                    entities.SaveChanges();
                    TempData["Success"] = "Successfully Updated";
                }
            }
            catch (Exception ex) { }
           
            return RedirectToAction("ClientsGridView");
        }

        public ActionResult DeleteClient(int? id)
        {
            try
            {
                if (id != null)
                {
                   
                    var clientpayment = ObjEntities.ClientsPayments.SingleOrDefault(e => e.Client == id );
                    if (clientpayment.Id > 0 && int.Parse(clientpayment.Due_Amount) > 0)
                    {
                        TempData["Warning"] = "Cannot Delete Client having due amount.Please Pay Full Due Amount";
                        return RedirectToAction("ClientsGridView");
                    }
                    else
                    {
                        var client = ObjEntities.ClientsViews.SingleOrDefault(e => e.Id == id);
                        if (client != null)
                        {
                            ObjEntities.ClientsViews.Remove(client ?? throw new InvalidOperationException());
                            ObjEntities.SaveChanges();
                            TempData["Success"] = "Successfully deleted";
                            return RedirectToAction("ClientsGridView");
                        }
                    }

                }
            }
            catch(Exception ex) { }
           
            return RedirectToAction("ClientsGridView");
        }

        public ActionResult AddPayments()
        {
            TempData["Heading"] = 4;
            try
            {
                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var Client = new SelectList(ObjEntities.ClientsViews.ToList(), "Id", "Name");
                    var Month = new SelectList(ObjEntities.tblMonths.ToList(), "Id", "Month");
                    ViewData["ClientPayment"] = Client;
                    ViewData["Month"] = Month;
                }
                var getClientPayment = ObjEntities.ClientsPayments.OrderByDescending(p => p.Id).ToList();
                if (getClientPayment.Count != 0)
                {
                    List<ClientPaymentVM> ClientPaymentlist = new List<ClientPaymentVM>();
                    foreach (var i in getClientPayment)
                    {
                        ClientPaymentVM v = new ClientPaymentVM();

                        var getclient = ObjEntities.ClientsViews.Where(e => e.Id == i.Client).FirstOrDefault();
                        //var getmonth = ObjEntities.tblMonth.Where(e => e.Id == i.Month).FirstOrDefault();
                        if (getclient != null)
                        {
                            v.Client = getclient.Name;
                        }
                        else
                        {
                            v.Client = "No Result";
                        }

                        v.Id = i.Id;
                        v.Month = i.Month;
                        v.Due_Amount = i.Due_Amount;
                        v.Paid_Amount = i.Paid_Amount;
                        v.EntryDate = i.EntryDate;
                        ClientPaymentlist.Add(v);


                    }
                    TempData["ClientPaymentData"] = ClientPaymentlist;
                    
                }
                
            }
            catch(Exception ex) { }
           
            return View();
        }

        [HttpPost]
        public ActionResult AddPayments(ClientsPayment clientpayment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //UpdateClientsPayment(clientpayment.Paid_Amount,clientpayment.Client);

                    using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                    {
                        var getclient = ObjEntities.ClientsViews.Where(e => e.Id == clientpayment.Client).FirstOrDefault();

                        //My Code Saad Bhatti
                        var chkData = ObjEntities.ClientsPayments.Where(c => c.Client == clientpayment.Client 
                        && c.EntryDate.Year == clientpayment.Month.Year && c.EntryDate.Month == clientpayment.Month.Month).FirstOrDefault();
                        if(chkData != null)
                        {
                          
                               if(Convert.ToInt32(getclient.Amount) >= Convert.ToInt32(clientpayment.Paid_Amount) + Convert.ToInt32(chkData.Paid_Amount))
                                {
                                    if (long.Parse(clientpayment.Paid_Amount.ToString()) <= long.Parse(getclient.Amount.ToString()))
                                    {
                                        int TotalAmount = Convert.ToInt32(getclient.Amount);
                                        int Due = Convert.ToInt32(TotalAmount) - (Convert.ToInt32(clientpayment.Paid_Amount) + (Convert.ToInt32(chkData.Paid_Amount)));
                                        int Paid = Convert.ToInt32(chkData.Paid_Amount) + Convert.ToInt32(clientpayment.Paid_Amount);
                                        chkData.Paid_Amount = Paid.ToString();
                                        chkData.Due_Amount = Due.ToString();
                                        ObjEntities.SaveChanges();

                                        //clientpayment.EntryDate = DateTime.Now;

                                        //chkData.Due_Amount = Due.ToString();
                                        //ObjEntities.ClientsPayments.Add(clientpayment);
                                        //ObjEntities.SaveChanges();
                                        //int id1 = 0;
                                        //id1 = clientpayment.Id;
                                        //if (id1 > 0)
                                        //{

                                            // var getmonth = ObjEntities.tblMonth.Where(e => e.Id == clientpayment.Month).FirstOrDefault();
                                            TempData["Success"] = Session["UserName"] + " " + "Added" + " " + clientpayment.Paid_Amount + " " + "for" + " " + getclient.Name + " " + "for" + " " + "Month" + "-" + clientpayment.Month.Month;
                                            tblNotification tblnotification = new tblNotification();
                                            tblnotification.NotificationMessage = Session["UserName"] + " " + "Added" + " " + clientpayment.Paid_Amount + " " + "for" + " " + getclient.Name + " " + "for" + " " + "Month" + "-" + clientpayment.Month.Month;
                                            tblnotification.IsShow = 0;
                                            ObjEntities.tblNotifications.Add(tblnotification);
                                            ObjEntities.SaveChanges();
                                        //}
                                    }
                                    else
                                    {
                                        TempData["Warning"] = "Paid Amount Should be less than due amount!";

                                    }
                                }
                                else
                                {
                                    TempData["Warning"] = "Paid Amount Should be less than due amount!";

                                }
                        }
                      
                        else
                        {
                            int id = 0;
                            clientpayment.EntryDate = Convert.ToDateTime(clientpayment.Month);
                            clientpayment.Due_Amount = Convert.ToString(long.Parse((getclient.Amount)) - long.Parse((clientpayment.Paid_Amount)));
                            ObjEntities.ClientsPayments.Add(clientpayment);
                            ObjEntities.SaveChanges();
                            id = clientpayment.Id;
                            if (id > 0)
                            {

                                // var getmonth = ObjEntities.tblMonth.Where(e => e.Id == clientpayment.Month).FirstOrDefault();
                                TempData["Success"] = Session["UserName"] + " " + "Added" + " " + clientpayment.Paid_Amount + "Payment" + " " + "for" + " " + getclient.Name + " " + "for" + " " + "Month" + "-" + clientpayment.Month;
                                tblNotification tblnotification = new tblNotification();
                                tblnotification.NotificationMessage = Session["UserName"] + " " + "Added" + " " + clientpayment.Paid_Amount + " " + "for" + " " + getclient.Name + " " + "for" + " " + "Month" + "-" + clientpayment.Month;
                                tblnotification.IsShow = 0;
                                ObjEntities.tblNotifications.Add(tblnotification);
                                ObjEntities.SaveChanges();
                            }

                        }
                    }
                }
                //var Clients = new SelectList(ObjEntities.ClientsView.ToList(), "Id", "Name");
                //ViewData["ClientPayment"] = Clients;
                
            }
            catch(Exception ex) { }
           
            return RedirectToAction("AddPayments"); 
        }

        private void UpdateClientsPayment(string Amount,int id)
        {
            using (ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel())
            {
                ClientsView updatedclientsview = (from c in entities.ClientsViews
                                                  where c.Id == id
                                                  select c).FirstOrDefault();

                long clientamount = long.Parse(updatedclientsview.Amount);
                long clientpayments = long.Parse(Amount);

                long updatedamount = clientamount - clientpayments;

                updatedclientsview.Amount = updatedamount.ToString();
               
                updatedclientsview.EntryDate = DateTime.Now;

                entities.SaveChanges();
            }
        }
       

        public ActionResult EditClientsPayment(int id)
        {
            var clientspayment = new ClientsPayment();
            try
            {
                 clientspayment = ObjEntities.ClientsPayments.Where(x => x.Id == id).FirstOrDefault();

                if (clientspayment != null)
                {
                    ViewBag.Id = id;
                    var Client = new SelectList(ObjEntities.ClientsViews.ToList(), "Id", "Name");
                    var Month = new SelectList(ObjEntities.tblMonths.ToList(), "Id", "Month");
                    ViewData["ClientPayment"] = Client;
                    ViewData["MonthData"] = Month;

                }

                ViewBag.Id = id;
                TempData["Heading"] = 4;
            }
            catch(Exception ex) { }
           
            return View(clientspayment);
        }

        [HttpPost]
        public ActionResult UpdateClientsPayment(ClientsPayment clientspayment)
        {
            try {
                using (ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel())
                {
                    var getclientpayment = ObjEntities.ClientsPayments.Where(e => e.Client == clientspayment.Client).FirstOrDefault();
                    var getclient = ObjEntities.ClientsViews.Where(e => e.Id == clientspayment.Client).FirstOrDefault();

                    if(getclientpayment != null)
                    {
                        if(Convert.ToInt32(clientspayment.Paid_Amount) <= Convert.ToInt32(getclient.Amount))
                        {
                            int Due = Convert.ToInt32(getclient.Amount) - Convert.ToInt32(clientspayment.Paid_Amount);
                            getclientpayment.Paid_Amount = clientspayment.Paid_Amount;
                            getclientpayment.Due_Amount = Due.ToString();
                            //getclientpayment.EntryDate = Convert.ToDateTime(clientspayment.Month);
                            entities.SaveChanges();
                            TempData["Success"] = "Data Updated successfully";
                        }
                        else
                        {
                            TempData["Warning"] = "Paid Amount Should be less than due amount!";
                            return RedirectToAction("EditClientsPayment");
                        }
                    }
                    
                    //if (long.Parse(clientspayment.Paid_Amount.ToString()) <= long.Parse(getclientpayment.Due_Amount.ToString()))
                    //{
                    //    ClientsPayment updatedclientspayment = (from c in entities.ClientsPayments
                    //                                            where c.Id == clientspayment.Id
                    //                                            select c).FirstOrDefault();

                    //    updatedclientspayment.Client = clientspayment.Client;
                    //    updatedclientspayment.Month = clientspayment.Month;
                    //    updatedclientspayment.Paid_Amount = Convert.ToString(long.Parse((updatedclientspayment.Paid_Amount)) + long.Parse((clientspayment.Paid_Amount)));
                    //    updatedclientspayment.Due_Amount = Convert.ToString(long.Parse((getclient.Amount)) - long.Parse((updatedclientspayment.Paid_Amount)));
                    //    updatedclientspayment.Month = clientspayment.Month;
                    //    updatedclientspayment.EntryDate = DateTime.Now;
                    //    entities.SaveChanges();
                    //    TempData["Success"] = "Successfully updated";
                    //}
                    //else
                    //{
                    //    TempData["Warning"] = "Paid Amount Should be less than due amount!";
                    //    return RedirectToAction("EditClientsPayment");
                    //}
                }
            }
            catch(Exception ex) { }
            
            return RedirectToAction("AddPayments");
        }

        public ActionResult DeletePayment(int? id)
        {
            try {
                if (id != null)
                {
                    var clientpayment = ObjEntities.ClientsPayments.SingleOrDefault(e => e.Id == id);
                    if (clientpayment != null)
                    {
                        if (long.Parse(clientpayment.Due_Amount) > 0)
                        {
                            TempData["Warning"] = "Cannot delete entry till full payment paid!";

                        }
                        else
                        {
                            ObjEntities.ClientsPayments.Remove(clientpayment ?? throw new InvalidOperationException());
                            ObjEntities.SaveChanges();
                            TempData["Success"] = "Successfully deleted";
                            return RedirectToAction("AddPayments");
                        }
                    }

                }
            }
            catch(Exception ex) { }
                
            
            return RedirectToAction("AddPayments");
        }

        public ActionResult ChooseExpense()
        {
            TempData["Heading"] = 5;
            try {
                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var ClientExpense = new SelectList(ObjEntities.ExpenseListNames.ToList(), "Id", "Description");

                    ViewData["ClientExpense"] = ClientExpense;

                }


                var getClientExpense = ObjEntities.ClientsExpenses.OrderByDescending(p => p.Id).ToList();
                if (getClientExpense.Count != 0)
                {
                    List<ClientExpenseVM> ClientExpenselist = new List<ClientExpenseVM>();
                    foreach (var i in getClientExpense)
                    {
                        ClientExpenseVM v = new ClientExpenseVM();

                        var getclientexpense = ObjEntities.ExpenseListNames.Where(e => e.Id == i.Expensetype).FirstOrDefault();

                        if (getclientexpense != null)
                        {
                            v.Expensetype = getclientexpense.Description;
                        }
                        else
                        {
                            v.Expensetype = "Select Expense Type";
                        }
                        v.Id = i.Id;
                        v.Amount = i.Amount;
                        v.MonthDate = i.MonthDate;
                        v.EntryDate = i.EntryDate;

                        ClientExpenselist.Add(v);


                    }
                    TempData["ClientExpenseData"] = ClientExpenselist;
                    
                }


            }
            catch(Exception ex) { }
           
            return View();
        }

        [HttpPost]
        public ActionResult AddExpense(ClientsExpense clientexpense)
        {
            try {
                if (ModelState.IsValid)
                {

                    using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                    {

                        clientexpense.EntryDate = DateTime.Now;
                        ObjEntities.ClientsExpenses.Add(clientexpense);
                        ObjEntities.SaveChanges();
                        int id = clientexpense.Id;
                        if (id >0)
                        {
                            TempData["Success"] = "Successfully Saved";
                        }
                        else
                        {
                            TempData["Failed"] = "Not Saved";
                        }
                    }
                }

            }
            catch(Exception ex) { }
            
            //var Clients = new SelectList(ObjEntities.ClientsView.ToList(), "Id", "Name");
            //ViewData["ClientPayment"] = Clients;

            return RedirectToAction("ChooseExpense"); ;
        }

        public ActionResult EditClientsExpenses(Int32 id)
        {
            var clientsexpense = new ClientsExpense();
            try {
                 clientsexpense = ObjEntities.ClientsExpenses.Where(x => x.Id == id).FirstOrDefault();

                if (clientsexpense != null)
                {
                    ViewBag.Id = id;
                    var ClientExpense = new SelectList(ObjEntities.ExpenseListNames.ToList(), "Id", "Description");

                    ViewData["ClientExpense"] = ClientExpense;

                }

                ViewBag.Id = id;
                TempData["Heading"] = 5;
                
            }
            catch(Exception ex) { }
            
            return View(clientsexpense);
        }

        [HttpPost]
        public ActionResult UpdateClientsExpense(ClientsExpense clientsexpense)
        {
            try {
                using (ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel())
                {
                    ClientsExpense updatedclientsexpenset = (from c in entities.ClientsExpenses
                                                             where c.Id == clientsexpense.Id
                                                             select c).FirstOrDefault();

                    updatedclientsexpenset.Expensetype = clientsexpense.Expensetype;
                    updatedclientsexpenset.Amount = clientsexpense.Amount;

                    updatedclientsexpenset.EntryDate = DateTime.Now;
                    entities.SaveChanges();
                    TempData["Success"] = "Successfully Updated";
                }
            }
            catch(Exception ex) { }
            

            return RedirectToAction("ChooseExpense");
        }

        public ActionResult DeleteClientExpense(int? id)
        {
            try
            {
                if (id != null)
                {
                    var clientexpense = ObjEntities.ClientsExpenses.SingleOrDefault(e => e.Id == id);
                    if (clientexpense != null)
                    {
                        ObjEntities.ClientsExpenses.Remove(clientexpense ?? throw new InvalidOperationException());
                        ObjEntities.SaveChanges();
                        return RedirectToAction("ChooseExpense");
                    }

                }
            }
            catch { }
           
            return RedirectToAction("ChooseExpense");
        }


        public ActionResult GetSubsAmount(int subsId)
        {
            try
            {
                var Clear = "";
                if (subsId != 0)
                {

                    var getAmount = ObjEntities.EshtiracksTypes.Where(o => o.Id == subsId).FirstOrDefault();
                    if (getAmount != null)
                    {
                        var Amount = getAmount.Amount;
                        return Json(new { Success = "true", Data = new { Amount } });
                    }
                    else
                    {
                        return Json(new { Success = "No", Data = new { Clear } });
                    }
                }
                else
                {
                    return Json(new { Success = "Id", Data = new { Clear } });
                }
            }
            catch (Exception ex)
            {

                return Json(new { Success = "false" });
            }
        }

    }
}