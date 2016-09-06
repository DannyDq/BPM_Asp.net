using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fiodariane.Models;

namespace Fiodariane.Controllers
{
    public class SupportingISController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SupportingIS
        public ActionResult Index()
        {
            return View(db.SupportingIS.ToList());
        }

        // GET: SupportingIS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportingIS supportingIS = db.SupportingIS.Find(id);
            if (supportingIS == null)
            {
                return HttpNotFound();
            }
            return View(supportingIS);
        }

        // GET: SupportingIS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupportingIS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupportingISID,Name")] SupportingIS supportingIS)
        {
            if (ModelState.IsValid)
            {
                db.SupportingIS.Add(supportingIS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supportingIS);
        }

        // GET: SupportingIS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportingIS supportingIS = db.SupportingIS.Find(id);
            if (supportingIS == null)
            {
                return HttpNotFound();
            }
            return View(supportingIS);
        }

        // POST: SupportingIS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupportingISID,Name")] SupportingIS supportingIS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supportingIS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supportingIS);
        }

        // GET: SupportingIS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportingIS supportingIS = db.SupportingIS.Find(id);
            if (supportingIS == null)
            {
                return HttpNotFound();
            }
            return View(supportingIS);
        }

        // POST: SupportingIS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupportingIS supportingIS = db.SupportingIS.Find(id);
            db.SupportingIS.Remove(supportingIS);
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
    }
}
