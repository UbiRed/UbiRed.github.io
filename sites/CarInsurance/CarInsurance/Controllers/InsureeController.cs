using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        public decimal CalculateQuote(Insuree insuree)
        {
            //Start with base of $50
            decimal MonthlyTotal = 50.0M;

            //Check Age
            int CalculateAge(DateTime dateOfBirth)
            {
                DateTime currentDate = DateTime.Now;
                int age = currentDate.Year - dateOfBirth.Year;

                if (currentDate < dateOfBirth.AddYears(age))
                {
                    age--; // Decrease age by 1 if the birthday hasnt occurred yet
                }

                return age;
            }

            if (insuree.DateOfBirth != null)
            {
                int age = CalculateAge(insuree.DateOfBirth);
                if (age <= 18)
                {
                    MonthlyTotal += 100.0M;
                }
                else if (age >= 19 && age <= 25)
                {
                    MonthlyTotal += 50.0M;
                }
                else
                {
                    MonthlyTotal += 25.0M;
                }
            }

            //Check car year
            if (insuree.CarYear < 2000)
            {
                MonthlyTotal += 25.0M;
            }
            if (insuree.CarYear > 2015)
            {
                MonthlyTotal += 25.0M;
            }

            // check car make and model
            if (insuree.CarMake != null && insuree.CarMake.ToLower() == "porsche")
            {
                MonthlyTotal += 25.0M;
                if (insuree.CarModel != null && insuree.CarModel.ToLower() == "911 carrara")
                {
                    MonthlyTotal += 25.0M;
                }
            }

            //speeding ticket and DUI check
            MonthlyTotal += insuree.SpeedingTickets * 10.0M;
            if (insuree.DUI)
            {
                MonthlyTotal += MonthlyTotal * 0.25M;
            }

            //FullCoverage check
            if (insuree.CoverageType)
            {
                MonthlyTotal += MonthlyTotal * 0.5M;
            }
            insuree.Quote = MonthlyTotal;

            //return quote
            return MonthlyTotal;
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Calculate the quote based on user inputs
                decimal quote = CalculateQuote(insuree);

                // Set the quote in the model
                insuree.Quote = quote;

                // Add the Insuree to the context
                db.Insurees.Add(insuree);

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        public ActionResult Admin()
        {
            // Retrieve fn, ln, ad of insured persons
            var adminData = db.Insurees.Select(i => new AdminViewModel
            {
                FirstName = i.FirstName,
                LastName = i.LastName,
                EmailAddress = i.EmailAddress,
                Quote = i.Quote
            }).ToList();

            return View(adminData);
        }



    }
}
