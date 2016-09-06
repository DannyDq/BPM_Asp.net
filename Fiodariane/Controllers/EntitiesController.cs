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
    public class EntitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void setEntityTypeInViewBag(EntityType type)
        {
            ViewBag.Name = type == EntityType.OrganisationUnit ? "Organisational Unit" : "HR Function";
            ViewBag.Icon = type == EntityType.OrganisationUnit ? "glyphicon-th-list" : "glyphicon-list-alt";
            ViewBag.TypeCode = setType(type);
        }

        private int setType(EntityType type)
        {
            return type == EntityType.OrganisationUnit ? 1 : 2;
        }

        private EntityType getType(int type)
        {
            // 1 is used in _Layout to specify Organisational Unit, any other number is an HR Function
            return (type == 1 ? EntityType.OrganisationUnit : EntityType.HRFunction);
        }

        // GET: Entities
        public ActionResult Index(int type)
        {
            EntityType entType = getType(type);
            setEntityTypeInViewBag(entType);
            return View(db.Entities.Where(x => x.EntityType == entType).ToList());
        }

        // GET: Entities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.Entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            setEntityTypeInViewBag(entity.EntityType);
            return View(entity);
        }

        // GET: Entities/Create
        public ActionResult Create(int type)
        {
            EntityType entType = getType(type);
            setEntityTypeInViewBag(entType);
            return View(new Entity { EntityType = entType });
        }

        // POST: Entities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntityType,Name")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                db.Entities.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index", new { type = setType(entity.EntityType) });
            }

            return View(entity);
        }

        // GET: Entities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.Entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            setEntityTypeInViewBag(entity.EntityType);
            return View(entity);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntityID,EntityType,Name")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { type = setType(entity.EntityType) });
            }
            return View(entity);
        }

        // GET: Entities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.Entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            setEntityTypeInViewBag(entity.EntityType);
            return View(entity);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entity entity = db.Entities.Find(id);
            db.Entities.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index", new { type = setType(entity.EntityType) });
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
