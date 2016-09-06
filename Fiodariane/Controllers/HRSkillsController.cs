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
    public class HRSkillsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HRSkills
        public ActionResult Index()
        {
            return View(db.HRSkills.ToList());
        }

        // GET: HRSkills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRSkills hRSkills = db.HRSkills.Find(id);
            if (hRSkills == null)
            {
                return HttpNotFound();
            }
            return View(hRSkills);
        }

        // GET: HRSkills/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HRSkills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HRSkillsID,Name")] HRSkills hRSkills)
        {
            if (ModelState.IsValid)
            {
                db.HRSkills.Add(hRSkills);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hRSkills);
        }

        // GET: HRSkills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRSkills hRSkills = db.HRSkills.Find(id);
            if (hRSkills == null)
            {
                return HttpNotFound();
            }
            return View(hRSkills);
        }

        // POST: HRSkills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HRSkillsID,Name")] HRSkills hRSkills)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRSkills).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hRSkills);
        }

        // GET: HRSkills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRSkills hRSkills = db.HRSkills.Find(id);
            if (hRSkills == null)
            {
                return HttpNotFound();
            }
            return View(hRSkills);
        }

        // POST: HRSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRSkills hRSkills = db.HRSkills.Find(id);
            db.HRSkills.Remove(hRSkills);
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
