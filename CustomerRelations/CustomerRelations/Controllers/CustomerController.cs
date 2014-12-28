using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerRelations.Models;
using System.Data.Entity.Validation;
using ModelState = System.Web.Http.ModelBinding.ModelState;

namespace CustomerRelations.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Customer List based on the Search Criteria entered
        /// </summary>
        /// <param name="searchVal"></param>
        /// <returns></returns>
        public ActionResult CustomerList(String searchVal)
        {
            IEnumerable<Customer> customerList = new List<Customer>();

            using (var db = new Models.CustomerRelationsEntities1())
            {
                var customers =
                    db.Customers.Where(x => (x.FirstName.IndexOf(searchVal) == 0 
                                            || x.LastName.IndexOf(searchVal) == 0 
                                            || x.AddressLine1.IndexOf(searchVal) == 0 
                                            || x.City.IndexOf(searchVal) == 0
                                            || x.Telephone.IndexOf(searchVal) == 0))
                        .ToList();

                customerList = customers.Select(x => 
                    new Customer()
                    {
                        FirstName =  x.FirstName,
                        LastName =  x.LastName,
                        AddressLine1 = x.AddressLine1,
                        AddressLine2 = x.AddressLine2,
                        City =  x.City,
                        State = x.State,
                        ZipCode = x.ZipCode,
                        CustomerId = x.CustomerId,
                        EMail = x.EMail,
                        Telephone = x.Telephone,
                        Picture =  x.Picture
                        
                    });
                    
            }

            return PartialView("CustomerList",customerList);
        }

        //
        // GET: /Course/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create Customer Post
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="custNote"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer, String custNote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cust = new Customer();
                    cust.FirstName = customer.FirstName;
                    cust.LastName = customer.LastName;
                    cust.AddressLine1 = customer.AddressLine1;
                    cust.AddressLine2 = customer.AddressLine2;
                    cust.EMail = customer.EMail;
                    cust.Telephone = customer.Telephone;
                    cust.City = customer.City;
                    cust.State = customer.State;
                    cust.ZipCode = customer.ZipCode;
                    cust.CreatedDate = DateTime.Now;
                    cust.UserId = Convert.ToInt32(Session["UserId"].ToString());
                    using (var db = new Models.CustomerRelationsEntities1())
                    {
                        db.Customers.Add(cust);
                        db.SaveChanges();

                        if (!String.IsNullOrWhiteSpace(custNote))
                        {
                            var customerNote = new CustomerNote();
                            customerNote.CustomerId = cust.CustomerId;
                            customerNote.UserId = cust.UserId;
                            customerNote.NoteText = custNote;
                            customerNote.CreatedDate = DateTime.Now;
                            db.CustomerNotes.Add(customerNote);
                            db.SaveChanges();

                        }
                    }

                    
                    TempData["Message"] = String.Concat("Customer ", customer.FirstName, " ", customer.LastName, " was added.");
                    return RedirectToAction("Index");
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

            return View(customer);
        }

        /// <summary>
        ///  Edit Customer Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            using (var db = new Models.CustomerRelationsEntities1())
            {
                var customer = db.Customers.Where(x => x.CustomerId == id).ToList().ElementAt(0);

                Customer cust = new Customer();
                cust.CustomerId = customer.CustomerId;
                cust.FirstName = customer.FirstName;
                cust.LastName = customer.LastName;
                cust.AddressLine1 = customer.AddressLine1;
                cust.AddressLine2 = customer.AddressLine2;
                cust.City = customer.City;
                cust.State = customer.State;
                cust.ZipCode = customer.ZipCode;
                cust.CustomerId = customer.CustomerId;
                cust.Telephone = customer.Telephone;
                cust.Picture = customer.Picture;
                cust.EMail = customer.EMail;
                cust.UserId = customer.UserId;

                var customerNotes = db.CustomerNotes.Where(x => x.CustomerId == customer.CustomerId).ToList();

                cust.CustomerNotes = customerNotes;

           
                return View("Edit", cust);
            }
        }


        /// <summary>
        /// Edit Customer Post
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer, String custNote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cust = new Customer();
                    cust.CustomerId = customer.CustomerId;
                    cust.FirstName = customer.FirstName;
                    cust.LastName = customer.LastName;
                    cust.AddressLine1 = customer.AddressLine1;
                    cust.AddressLine2 = customer.AddressLine2;
                    cust.EMail = customer.EMail;
                    cust.Telephone = customer.Telephone;
                    cust.City = customer.City;
                    cust.State = customer.State;
                    cust.ZipCode = customer.ZipCode;
                    cust.CreatedDate = DateTime.Now;
                    cust.UserId = 2;
                    using (var db = new Models.CustomerRelationsEntities1())
                    {
                        db.Entry(cust).State = EntityState.Modified;
                        db.SaveChanges();

                        if (!String.IsNullOrWhiteSpace(custNote))
                        {
                            var customerNote = new CustomerNote();
                            customerNote.CustomerId = cust.CustomerId;
                            customerNote.UserId = cust.UserId;
                            customerNote.NoteText = custNote;
                            customerNote.CreatedDate = DateTime.Now;
                            db.CustomerNotes.Add(customerNote);
                            db.SaveChanges();

                        }
                    }
                    TempData["Message"] = String.Concat("Customer ", customer.FirstName, " ", customer.LastName, " was updated.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                //Log the error 
                ModelState.AddModelError("", String.Concat("Unable to save changes. Try again.", exception.Message));
            }

            return View(customer);
        }

    }
}
