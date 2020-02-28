using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [CheckSession]
    public class RecapPagesController : Controller
    {

        ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel();
        int isfilter = 0;
        DataSet dsfilter = new DataSet();
        // GET: RecapPages
        public ActionResult Recappermonth()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try {
               
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var GetMonth = new SelectList(ObjEntities.tblMonths.ToList(), "Id", "Month");

                    ViewData["GetMonth"] = GetMonth;

                }
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "Select FORMAT(E.MonthDate,'MMMM') as Month, Sum(cast(C.[Paid Amount] as float(10))) as Income,isnull(E.Amount, 0) as Expense from ClientsPayment as C inner join ClientsExpense as E on Month(E.MonthDate)= Month(C.Month) group by C.Month, E.MonthDate,E.Amount order by C.Month";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }

                        DataColumn newCol = new DataColumn("NetAmount", typeof(Int64));
                        dt.Columns.Add(newCol);

                        foreach (DataRow row in dt.Rows)
                        {
                            Int64 Income = Int64.Parse(row.ItemArray[1].ToString());
                            Int64 expense = Int64.Parse(row.ItemArray[2].ToString());

                            row["NetAmount"] = Income - expense;

                        }
                        ds.Tables.Add(dt);
                    }
                }
                TempData["Heading"] = 6;
            }
            catch(Exception ex) { }
            
            return View(ds);
        }

        [HttpPost]
        public ActionResult Recappermonth(FormCollection Form)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var GetMonth = new SelectList(ObjEntities.tblMonths.ToList(), "Id", "Month");

                    ViewData["GetMonth"] = GetMonth;

                }
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string selectmonth = Request.Form["Month"].ToString();
                    string query = "Select FORMAT(E.MonthDate,'MMMM') as Month, Sum(cast(C.[Paid Amount] as float(10))) as Income,isnull(E.Amount, 0) as Expense from ClientsPayment as C inner join ClientsExpense as E on Month(E.MonthDate)= Month(C.Month)  where Month(E.MonthDate)=" + selectmonth + " group by C.Month, E.MonthDate,E.Amount order by C.Month";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }

                        DataColumn newCol = new DataColumn("NetAmount", typeof(Int64));
                        dt.Columns.Add(newCol);

                        foreach (DataRow row in dt.Rows)
                        {
                            Int64 Income = Int64.Parse(row.ItemArray[1].ToString());
                            Int64 expense = Int64.Parse(row.ItemArray[2].ToString());

                            row["NetAmount"] = Income - expense;

                        }
                        ds.Tables.Add(dt);
                    }
                }
                TempData["Heading"] = 6;
            }
            catch(Exception ex) { }
          
           
            return View(ds);
        }

        public ActionResult Recapperexpensetype()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                

                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var ClientExpense = new SelectList(ObjEntities.ExpenseListNames.ToList(), "Id", "Description");

                    ViewData["ClientExpense"] = ClientExpense;

                }
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "Select * from ClientsExpense as C inner join ExpenseListNames as E on E.Id=C.Expensetype";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }

                        ds.Tables.Add(dt);
                    }
                }
                TempData["Heading"] = 7;
            }
            catch (Exception ex) { }
          
            return View(ds);
        }
        [HttpPost]
        public ActionResult Recapperexpensetype(FormCollection Form)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var ClientExpense = new SelectList(ObjEntities.ExpenseListNames.ToList(), "Id", "Description");

                    ViewData["ClientExpense"] = ClientExpense;

                }
                
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string selectedexpensetype = Request.Form["ExpenseType"].ToString();
                    string query = "Select * from ClientsExpense as C inner join ExpenseListNames as E on E.Id=C.Expensetype where C.Expensetype= " + selectedexpensetype + "";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }

                        ds.Tables.Add(dt);
                    }
                }
                TempData["Heading"] = 7;
            }
            catch { }
         
            return View(ds);
        }


        public ActionResult Recapperclient()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try {
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {

                    using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                    {
                        var ClientExpense = new SelectList(ObjEntities.ClientsViews.ToList(), "Id", "Name");

                        ViewData["recapperclient"] = ClientExpense;

                    }

                    if (TempData["Isfilter"] == null || TempData["Isfilter"].ToString() == "0")
                    {
                        string query = "Select * from ClientsPayment as P inner join ClientsView as C on C.Id=P.Client";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                sda.Fill(dt);
                            }

                            ds.Tables.Add(dt);
                        }

                    }
                    else
                    {
                        TempData["Isfilter"] = 0;
                        

                        return View(dsfilter);
                    }


                }
                TempData["Heading"] = 8;

            }
            catch(Exception ex) { }
            
            

            return View(ds);
        }

        [HttpPost]
        public ActionResult Recapperclient(FormCollection form)
        {
            try
            {
                DataTable dt = new DataTable();

                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string selectedclient = Request.Form["Client"].ToString();
                    using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                    {
                        var ClientExpense = new SelectList(ObjEntities.ClientsViews.ToList(), "Id", "Name");

                        ViewData["recapperclient"] = ClientExpense;

                    }
                    string query = "Select * from ClientsPayment as P inner join ClientsView as C on C.Id=P.Client where P.Client= " + selectedclient + "";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);

                            TempData["Isfilter"] = 1;

                        }

                        dsfilter.Tables.Add(dt);
                    }
                }
                TempData["Heading"] = 8;
            }
            catch { }
           
            return View(dsfilter);
        }
    }
}