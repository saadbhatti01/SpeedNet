using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel();
        //public ActionResult Index()
        //{
        //    return RedirectToAction("Login");
        //}
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(UserProfile objUser)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (ArchitectureEntitiesModel db = new ArchitectureEntitiesModel())
                    {

                        var obj = db.UserProfiles.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                        if (obj != null)
                        {
                            Session["UserID"] = obj.UserId.ToString();
                            Session["Image"] = obj.Image.ToString();
                            Session["UserName"] = obj.UserName.ToString();
                            return RedirectToAction("UserDashBoard");
                        }
                        else
                        {
                            TempData["Failed"] = "Invalid Username or Password";
                            return RedirectToAction("Login", "Account");
                        }
                    }
                }
                TempData["Failed"] = "Invalid Username or Password";
            }
            catch(Exception ex) { }
           
            return RedirectToAction("Login","Account");
        }

        [CheckSession]
        public ActionResult UserDashBoard()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
               
                if (Session["UserID"] != null)
                {
                    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string query = "Select  isnull(Sum(cast([Paid Amount] as float(10))),0) as ClientPayment from ClientsPayment";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {

                            int ClientPayment = 0;
                            cmd.Connection = con;
                            con.Open();
                            ClientPayment = int.Parse(cmd.ExecuteScalar().ToString());
                            ViewData["ClientPayment"] = ClientPayment;


                        }
                        string query1 = "Select  isnull(Sum(cast(C.Amount as float(10))) - Sum(cast(P.[Paid Amount] as float(10))),0)  as DueAmount  from ClientsPayment as P inner join ClientsView as C on C.Id = P.Client";
                        using (SqlCommand cmd = new SqlCommand(query1))
                        {

                            int DueAmount = 0;
                            cmd.Connection = con;

                            DueAmount = int.Parse(cmd.ExecuteScalar().ToString());
                            ViewData["DueAmount"] = DueAmount;

                        }
                        string query2 = "Select  isnull(Sum(cast(Amount as float(10))),0) as Expenses from ClientsExpense";
                        using (SqlCommand cmd = new SqlCommand(query2))
                        {

                            int Expenses = 0;
                            cmd.Connection = con;

                            Expenses = int.Parse(cmd.ExecuteScalar().ToString());
                            ViewData["Expenses"] = Expenses;

                        }
                        string query3 = "Select isnull(Count(Id),0) as UnPaidClients from ClientsPayment where [Due Amount] > 0";
                        using (SqlCommand cmd = new SqlCommand(query3))
                        {

                            int UnPaidClients = 0;
                            cmd.Connection = con;

                            UnPaidClients = int.Parse(cmd.ExecuteScalar().ToString());
                            ViewData["UnPaidClients"] = UnPaidClients;
                            Session["TypesofSubsription"] = ObjEntities.tblNotifications.Where(x => x.IsShow == 0).ToList();
                            var Eshtriackstype = new SelectList(ObjEntities.EshtiracksTypes.ToList(), "Id", "Description");
                            ViewData["EshtiracksTypes"] = Eshtriackstype;

                            TempData["Heading"] = 9;
                            

                        }

                        string query4 = "Select top 5 C.Name, datename(m,Month)+' '+cast(datepart(yyyy,Month) as varchar) as PaymentMonth,P.[Paid Amount],P.[Due Amount],P.EntryDate from ClientsPayment as P inner join ClientsView as C on C.Id=P.Client order by P.Id desc";
                        using (SqlCommand cmd = new SqlCommand(query4))
                        {
                            cmd.Connection = con;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                sda.Fill(dt);
                                ds.Tables.Add(dt);
                            }


                        }

                        string query5 = "Select top 5 Description, datename(m,MonthDate)+' '+cast(datepart(yyyy,MonthDate) as varchar) as Month,C.Amount,C.EntryDate from ClientsExpense as C inner join ExpenseListNames as E on E.Id=C.Expensetype order by C.Id desc";
                        using (SqlCommand cmd = new SqlCommand(query5))
                        {
                            cmd.Connection = con;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                sda.Fill(dt1);
                                ds.Tables.Add(dt1);
                            }


                        }

                    }
              
                    

                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                //return View("Error", "ExceptionHandling");
            }
            return View(ds);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}