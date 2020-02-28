using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        ArchitectureEntitiesModel con = new ArchitectureEntitiesModel();
        public ActionResult login()
        {
            return View();
        }
        public ActionResult Register()
        {
            TempData["Heading"] = 10;

            var getData = con.UserProfiles.OrderByDescending(u => u.UserId).ToList();
            if (getData.Count != 0)
            {
                TempData["Data"] = getData;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(UserProfile usr, HttpPostedFileBase file)
        {
            try
            {
                var checkEmail = con.UserProfiles.Where(s => s.Email == usr.Email).Any();
                if (checkEmail == true)
                {
                    TempData["Error"] = "This Email is already exist please choose a different one";
                    return RedirectToAction("Register");
                }
                else
                {
                    if (file != null)
                    {
                        string Imagename = Path.GetFileName(file.FileName);
                        string PhysicalPath = Path.Combine(Server.MapPath("~/UserImage/"), Imagename);
                        file.SaveAs(PhysicalPath);
                        usr.Image = Imagename;
                    }
                    con.UserProfiles.Add(usr);
                    con.SaveChanges();
                    TempData["Success"] = "User Added Successfully";
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            {

            }
            TempData["Heading"] = 10;
            return RedirectToAction("Register");
        }

        [CheckSession]
        public ActionResult UpdateUser(int id)
        {
            TempData["Heading"] = 11;
            var getData = con.UserProfiles.Where(u => u.UserId == id).FirstOrDefault();
            if (getData != null)
            {
                return View(getData);
            }
            else
            {
                TempData["Error"] = "No record Found";
                return RedirectToAction("Register");
            }
        }
        [CheckSession]
        [HttpPost]
        public ActionResult UpdateUser(int id, UserProfile usr, HttpPostedFileBase file) 
        {
            try
            {
                var getData = con.UserProfiles.Where(u => u.UserId == id).FirstOrDefault();
                if (getData != null)
                {
                    if (file != null)
                    {
                        string Imagename = Path.GetFileName(file.FileName);
                        string PhysicalPath = Path.Combine(Server.MapPath("~/UserImage/"), Imagename);
                        file.SaveAs(PhysicalPath);
                        getData.Image = Imagename;
                    }
                    getData.UserName = usr.UserName;
                    getData.Email = usr.Email;
                    getData.Password = usr.Password;
                    con.SaveChanges();
                    TempData["Success"] = "User Data Updated successfully";
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            {
                
            }
            return RedirectToAction("Register");
        }


        [CheckSession]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                var getData = con.UserProfiles.Where(u => u.UserId == id).FirstOrDefault();
                if (getData != null)
                {
                    con.UserProfiles.Remove(getData);
                    con.SaveChanges();
                    TempData["Success"] = "User Deleted Successfully";
                    return RedirectToAction("Register");
                }
                else
                {
                    TempData["Error"] = "No record Found";
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Register");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }
        public ActionResult ChangePassword()
        {
            TempData["Heading"] = 12;
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult ChangePassword(string cPassword, string nPassword, string rPassword)
        {
            try
            {
                int ID = Convert.ToInt32(Session["UserID"]);
                var getUsr = con.UserProfiles.Where(u => u.UserId == ID).FirstOrDefault();
                if (getUsr.Password == cPassword)
                {
                    if (nPassword == rPassword && nPassword != "" && rPassword != "")
                    {
                        getUsr.Password = nPassword;
                        con.Entry(getUsr).State = System.Data.Entity.EntityState.Modified;
                        con.SaveChanges();
                        TempData["Success"] = "Password has been changed successfully. Please relogin with new password.";
                        Session.Clear();
                        return RedirectToAction("login");
                    }
                    else
                    {
                        TempData["Error"] = "new password must be same.";
                    }
                }
                else
                {
                    TempData["Error"] = "Current password is not valid.";
                }
            }
            catch (Exception)
            {

            }
            return View();
        }

        public ActionResult ForgetPassword()
        {
            TempData["Heading"] = 13;
            return View();
        }

        [HttpPost]
       
        public ActionResult ForgetPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                var userInfo = con.UserProfiles.Where(u => u.Email.Equals(Email)).FirstOrDefault();
                if (userInfo != null)
                {
                    string emailBody = "<b>Dear &nbsp;" + userInfo.UserName + "</b><br/><br/> Your password is " + userInfo.Password + "";
                    Mailer email = new Mailer("speednet.hazzouri@hotmail.com", userInfo.Email, "SpeedNet Forget Password", emailBody);
                    email.Send();
                    TempData["Success"] = "Please check your email. An email sent to your registered email address to recover your password.";
                    return View();
                }
                else
                    TempData["Error"] = "No Email found, please Enter correct email address.";
            }
            return View();
        }

    }
}
