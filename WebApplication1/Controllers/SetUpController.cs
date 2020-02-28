using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers

{
    [CheckSession]
    public class SetUpController : Controller
    {
        ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel();
        EshtiracksType Eshtirackstype = new EshtiracksType();
        // GET: SetUp
        [HttpGet]
        public ActionResult TypesofEstiracks()
        {
            TempData["Heading"] = 1;
            try {
                TempData["TypesofSubsription"] = entities.EshtiracksTypes.OrderByDescending(e => e.Id).ToList();

                return View();
                
            }
            catch(Exception ex)
            {
           

                return View();
            }
            
        }

        [HttpPost]
        public ActionResult TypesofEstiracks(EshtiracksType Eshtirackstype)
        {
            try {

                if (ModelState.IsValid)
                {

                    using (ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel())
                    {
                        entities.EshtiracksTypes.Add(Eshtirackstype);
                        entities.SaveChanges();
                        int id = Eshtirackstype.Id;
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
            catch(Exception ex)
            {
                TempData["Failed"] = "Exception Occured";
                return View();
            }
            
            return RedirectToAction("TypesofEstiracks");      
        }

        public ActionResult EditTypesofEstiracks(Int32 id)  
        {
            EshtiracksType TypesofEstiracks = new EshtiracksType();
           try {
                TypesofEstiracks = entities.EshtiracksTypes.Where(x => x.Id == id).FirstOrDefault();

                if (TypesofEstiracks != null)
                {
                    ViewBag.Id = id;
                    TempData["Heading"] = 1;

                }
            }
            catch (Exception ex)
            {

            }
            
            return View(TypesofEstiracks);
        }

        [HttpPost]
        public ActionResult UpdateTypesofEstirack(EshtiracksType Eshtirackstype)
        {
            try
            {
                using (ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel())
                {
                    EshtiracksType updatedTypesofEstiracks = (from c in entities.EshtiracksTypes
                                                              where c.Id == Eshtirackstype.Id
                                                              select c).FirstOrDefault();
                    updatedTypesofEstiracks.Description = Eshtirackstype.Description;
                    updatedTypesofEstiracks.Amount = Eshtirackstype.Amount;
                    entities.SaveChanges();
                    int id = Eshtirackstype.Id;
                    if (id > 0)
                    {
                        TempData["Success"] = "Successfully Updated";
                        TempData["Heading"] = 1;
                        return RedirectToAction("TypesofEstiracks");
                    }
                    else
                    {
                        TempData["Failed"] = "Not Updated";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Exception Occured";
                return View();
            }
           

            
        }


    
        public ActionResult DeleteEstriacksTypes(int ? id)
        {
            try {
                if (id != null)
                {
                    var eshtiracksType = entities.EshtiracksTypes.SingleOrDefault(e => e.Id == id);
                    var checktype = entities.ClientsViews.SingleOrDefault(e => e.Type == id);
                    if (checktype == null)
                    {
                        entities.EshtiracksTypes.Remove(eshtiracksType ?? throw new InvalidOperationException());
                        entities.SaveChanges();
                        TempData["Success"] = "Successfully Deleted";
                        return RedirectToAction("TypesofEstiracks");
                    }
                    else
                    {
                        TempData["Error"] = "Subscription type is using by " +checktype.Name+". Cannot delete subscription type " + eshtiracksType.Description;
                    }

                }
            }
            catch (Exception ex)
            {


            }
           
            return RedirectToAction("TypesofEstiracks");
        }
        public ActionResult ExpensesListNames()
       {
            try {
                using (ArchitectureEntitiesModel ObjEntities = new ArchitectureEntitiesModel())
                {
                    var getexpenselist = ObjEntities.ExpenseListNames.OrderByDescending(e => e.Id).ToList();
                    if (getexpenselist.Count != 0)
                    {
                        List<ExpenseListVM> Expenselist = new List<ExpenseListVM>();
                        foreach (var i in getexpenselist)
                        {
                            ExpenseListVM v = new ExpenseListVM();
                            v.Id = i.Id;
                            v.Description = i.Description;
                            v.IconPath = i.IconPath;
                            Expenselist.Add(v);


                        }
                       
                        TempData["Expenselist"] = Expenselist;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            TempData["Heading"] = 2;

            return View();
        }

        [HttpPost]
        public ActionResult ExpensesListNames(ExpenseListName expenselistname,HttpPostedFileBase postedFile)
        {
            try {
                if (postedFile != null)
                {
                    string filename = Path.GetFileName(postedFile.FileName);

                    string extension = Path.GetExtension(postedFile.FileName);


                    expenselistname.IconPath = "~/Images/avatars/" + filename;


                    filename = Path.Combine(Server.MapPath("~/Images/avatars/"), filename);

                    postedFile.SaveAs(filename);

                    entities.ExpenseListNames.Add(expenselistname);

                    entities.SaveChanges();

                    int id1 = expenselistname.Id;
                    TempData["Heading"] = 2;
                    if (id1 > 0)
                    {
                        TempData["Success"] = "Successfully Saved";
                    }
                    else
                    {
                        TempData["Failed"] = "Not Saved";
                    }

                    return RedirectToAction("ExpensesListNames");
                }
                // entities.Entry(expenselistname).State = EntityState.Modified;
                expenselistname.IconPath = "";
                entities.ExpenseListNames.Add(expenselistname);
                entities.SaveChanges();
                int id = expenselistname.Id;
                TempData["Heading"] = 2;
                if (id > 0)
                {
                    TempData["Success"] = "Successfully Saved";
                }
                else
                {
                    TempData["Failed"] = "Not Saved";
                }
            }
            catch (Exception ex)
            {

            }
            

            return RedirectToAction("ExpensesListNames");
        }
           
    

        public ActionResult EditExpenseListNames(Int32 id)
        {
            try {
                var expenseListName = entities.ExpenseListNames.Where(x => x.Id == id).FirstOrDefault();
                if (expenseListName != null)
                {
                    ViewBag.Id = id;
                    return View(expenseListName);
                }
                TempData["Heading"] = 2;
            }
            catch (Exception ex)
            {

            }
            return View(entities.EshtiracksTypes.ToList());
        }

        [HttpPost]
        public ActionResult UpdateExpensesListNames(ExpenseListName expenselistname, HttpPostedFileBase ImageFile)
        {
            try {
                using (ArchitectureEntitiesModel entities = new ArchitectureEntitiesModel())
                {
                    ExpenseListName updatedexpenselistname = (from c in entities.ExpenseListNames
                                                              where c.Id == expenselistname.Id
                                                              select c).FirstOrDefault();

                    updatedexpenselistname.Description = expenselistname.Description;

                    //expenselistname.ImageFile = new HttpPostedFile();
                    //Extract Image File Name.

                    if (ImageFile != null)
                    {

                        string fileName = System.IO.Path.GetFileName(ImageFile.FileName);
                        // Set the Image File Path.
                        string filePath = "~/Images/avatars/" + fileName;

                        //Save the Image File in Folder.
                        ImageFile.SaveAs(Server.MapPath(filePath));

                        updatedexpenselistname.IconPath = filePath;
                    }



                    
                    entities.SaveChanges();
                    int id = expenselistname.Id;
                    if (id > 0)
                    {
                        TempData["Success"] = "Successfully Updated";
                    }
                }
            }
            catch (Exception ex){ }
            return RedirectToAction("ExpensesListNames");
        }

        public ActionResult DeleteExpenseList(int? id)
        {
           try
            {
                if (id != null)
                {
                    var expenselist = entities.ExpenseListNames.SingleOrDefault(e => e.Id == id);
                    var expensecheck = entities.ClientsExpenses.SingleOrDefault(e => e.Expensetype == id);
                    if (expensecheck == null && expenselist!=null)
                    {
                        entities.ExpenseListNames.Remove(expenselist ?? throw new InvalidOperationException());
                        entities.SaveChanges();
                        TempData["Success"] = "Successfully deleted";
                        return RedirectToAction("ExpensesListNames");
                    }
                    else
                    {
                        TempData["Error"] = "Expense type " + " " + expenselist.Description +" "+ "is using. So Cannot delete this expense type.";
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ExpensesListNames");
        }

        private void Createpath()
        {

            throw new NotImplementedException();
        }
    }
}