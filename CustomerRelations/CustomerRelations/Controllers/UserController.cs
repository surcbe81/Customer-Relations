using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CustomerRelations.Models;

namespace CustomerRelations.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(Models.UserProfile userProfile)
        {
            //if (ModelState.IsValid)
            //{
            if (IsValid(userProfile.UserName, userProfile.Password))
            {
                FormsAuthentication.SetAuthCookie(userProfile.UserName, false);
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Error, Check data");
            //}
            return View(userProfile);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Models.UserProfile user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new Models.CustomerRelationsEntities1())
                    {
                        var crypto = new SimpleCrypto.PBKDF2();
                        var encrypPass = crypto.Compute(user.Password);
                        var newUser = db.UserProfiles.Create();
                        newUser.UserName = user.UserName;
                        newUser.Email = user.Email;
                        newUser.Password = encrypPass;
                        newUser.PasswordSalt = crypto.Salt;
                        newUser.FirstName = user.FirstName;
                        newUser.LastName = user.LastName;
                        newUser.CreatedDate = DateTime.Now;
                        db.UserProfiles.Add(newUser);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Data is not correct");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                //Log the error 
                ModelState.AddModelError("", String.Concat("Unable to save changes. Try again.", e.Message));
            }

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool IsValid(string username, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            bool IsValid = false;

            using (var db = new Models.CustomerRelationsEntities1())
            {
                var user = db.UserProfiles.FirstOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        IsValid = true;
                    }
                    Session["UserId"] = user.UserId;
                }
            }
            return IsValid;
        }

    }
}
