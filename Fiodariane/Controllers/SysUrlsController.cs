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
    public class SysUrlsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SysUrls
        public ActionResult Index()
        {
            return View(db.SysUrls.ToList());
        }

        // GET: SysUrls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysUrl sysUrl = db.SysUrls.Find(id);
            if (sysUrl == null)
            {
                return HttpNotFound();
            }
            return View(sysUrl);
        }

        // GET: SysUrls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysUrls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SysUrlID,Url,Description,SysUrlType,TaskID")] SysUrl sysUrl)
        {
            if (ModelState.IsValid)
            {
                db.SysUrls.Add(sysUrl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysUrl);
        }

        // GET: SysUrls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysUrl sysUrl = db.SysUrls.Find(id);
            if (sysUrl == null)
            {
                return HttpNotFound();
            }
            return View(sysUrl);
        }

        // POST: SysUrls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SysUrlID,Url,Description,SysUrlType,TaskID")] SysUrl sysUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysUrl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysUrl);
        }

        // GET: SysUrls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysUrl sysUrl = db.SysUrls.Find(id);
            if (sysUrl == null)
            {
                return HttpNotFound();
            }
            return View(sysUrl);
        }

        // POST: SysUrls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SysUrl sysUrl = db.SysUrls.Find(id);
            db.SysUrls.Remove(sysUrl);
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
