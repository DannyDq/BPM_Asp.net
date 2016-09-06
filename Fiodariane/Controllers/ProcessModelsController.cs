using Fiodariane.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Fiodariane.Controllers
{
    public class ProcessModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProcessModels
        public ActionResult Index()
        {
            ProcessIndex index = new ProcessIndex();
             index.Macro = (from pr in db.ProcessModels
                           where pr.Level == 0
                           join mn in db.Managers on pr.ManagerID equals mn.ManagerID
                           into new_pr
                           from tbl in new_pr.DefaultIfEmpty()
                           select new ProcessList
                           {
                               ProcessID = pr.ProcessID,
                               Name = pr.Name,
                               CreationTime = pr.CreationTime,
                               Manager = tbl.Name
                           }).ToList();
            index.Process = (from pr in db.ProcessModels where pr.Level == 1
                             join mn in db.Managers on pr.ManagerID equals mn.ManagerID 
                             into new_pr from tbl in new_pr.DefaultIfEmpty()
                             select new ProcessList { ProcessID = pr.ProcessID, Name = pr.Name,
                                 CreationTime = pr.CreationTime, Manager = tbl.Name }).ToList();
            index.Subprocess = (from pr in db.ProcessModels
                                where pr.Level > 1
                                join mn in db.Managers on pr.ManagerID equals mn.ManagerID
                                into new_pr
                                from tbl in new_pr.DefaultIfEmpty()
                                select new ProcessList
                                {
                                    ProcessID = pr.ProcessID,
                                    Name = pr.Name,
                                    CreationTime = pr.CreationTime,
                                    Manager = tbl.Name
                                }).ToList();
            return View(index);
        }

        //
        // GET: ProcessModels list
        [ChildActionOnly]
        public ActionResult ProcessMenu()
        {
            ProcessTreeModel model = new ProcessTreeModel();
            model.Macro = db.ProcessModels.Where(p => p.Level == 0).Select(p => new ProcessSummary { ProcessID = p.ProcessID, Name = p.Name }).ToList();
            model.Process = db.ProcessModels.Where(p => p.Level == 1).Select(p => new ProcessSummary { ProcessID = p.ProcessID, Name = p.Name }).ToList();
            model.SubProcess = db.ProcessModels.Where(p => p.Level >= 2).Select(p => new ProcessSummary { ProcessID = p.ProcessID, Name = p.Name }).ToList();
            return PartialView(model);
        }

        private void SetViewBag(ProcessModels processModels)
        {
            if (processModels.ParentID == null)
                ViewBag.Parent = new SelectList(db.ProcessModels.Where(x => x.ProcessID != processModels.ProcessID), "ProcessID", "Name");
            else
                ViewBag.Parent = new SelectList(db.ProcessModels.Where(x => x.ProcessID != processModels.ProcessID), "ProcessID", "Name", processModels.ParentID);

            if (processModels.ProcedureID == null)
                ViewBag.Procedure = new SelectList(db.Procedures, "ProcedureID", "Name");
            else
                ViewBag.Procedure = new SelectList(db.Procedures, "ProcedureID", "Name", processModels.ProcedureID);

            if (processModels.ManagerID == null)
                ViewBag.Manager = new SelectList(db.Managers, "ManagerID", "Name");
            else
                ViewBag.Manager = new SelectList(db.Managers, "ManagerID", "Name", processModels.ManagerID);
        }

        private void SetViewBag(IEnumerable<Tasks> tasks)
        {
            int size = tasks.Count();
            int idx = 0;
            ViewBag.SupportingIS = new SelectList[size];
            ViewBag.HRFunction = new SelectList[size];
            ViewBag.OrganisationUnit = new SelectList[size];
            ViewBag.HRSkills = new SelectList[size];
            ViewBag.OriginJSON = new List<JsonSelect>[size];
            ViewBag.OriginSelJSON = new List<int>[size];
            ViewBag.DestinationJSON = new List<JsonSelect>[size];
            ViewBag.DestinationSelJSON = new List<int>[size];
            foreach (var item in tasks)
            {
                if (item.SupportingISID == null)
                    ViewBag.SupportingIS[idx] = new SelectList(db.SupportingIS.ToList(), "SupportingISID", "Name");
                else
                    ViewBag.SupportingIS[idx] = new SelectList(db.SupportingIS.ToList(), "SupportingISID", "Name", item.SupportingISID);

                if (item.HRFunctionID == null)
                    ViewBag.HRFunction[idx] = new SelectList(db.Entities.Where(x => x.EntityType == EntityType.HRFunction), "EntityID", "Name");
                else
                    ViewBag.HRFunction[idx] = new SelectList(db.Entities.Where(x => x.EntityType == EntityType.HRFunction), "EntityID", "Name", item.HRFunctionID);

                if (item.OrganisationUnitID == null)
                    ViewBag.OrganisationUnit[idx] = new SelectList(db.Entities.Where(x => x.EntityType == EntityType.OrganisationUnit), "EntityID", "Name");
                else
                    ViewBag.OrganisationUnit[idx] = new SelectList(db.Entities.Where(x => x.EntityType == EntityType.OrganisationUnit), "EntityID", "Name", item.OrganisationUnitID);

                if (item.HRSkillsID == null)
                    ViewBag.HRSkills[idx] = new SelectList(db.HRSkills.ToList(), "HRSkillsID", "Name");
                else
                    ViewBag.HRSkills[idx] = new SelectList(db.HRSkills.ToList(), "HRSkillsID", "Name", item.HRSkillsID);

                ViewBag.OriginJSON[idx] = db.Entities.Where(x => (x.EntityType == EntityType.HRFunction || x.EntityType == EntityType.OrganisationUnit)).Select(x => new JsonSelect { value = x.EntityID, text = x.Name }).ToList();
                ViewBag.OriginSelJSON[idx] = db.TaskEntities.Where(x => (x.TaskID == item.TaskID) && (x.EntityType == TaskEntityType.Origin)).Select(x => x.EntityID).ToList();

                ViewBag.DestinationJSON[idx] = db.Entities.Where(x => (x.EntityType == EntityType.HRFunction || x.EntityType == EntityType.OrganisationUnit)).Select(x => new JsonSelect { value = x.EntityID, text = x.Name }).ToList();
                ViewBag.DestinationSelJSON[idx] = db.TaskEntities.Where(x => (x.TaskID == item.TaskID) && (x.EntityType == TaskEntityType.Destination)).Select(x => x.EntityID).ToList();

                idx++;
            }

        }

        // GET: ProcessModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProcessModels processModels = db.ProcessModels.Where(p => p.ProcessID == id.Value).Include(m => m.Manager).Include(p => p.Parent).Include(p => p.Procedure).Single();
            if (processModels == null)
            {
                return HttpNotFound();
            }

            ViewBag.Children = db.ProcessModels.Where(p => p.ParentID == id.Value).ToList();

            processModels.Tasks = db.Tasks.Where(p => p.ProcessID == id.Value).Include(n => n.Name).Include(s => s.SupportingIS).Include(h => h.HRFunction).Include(o => o.OrganisationUnit).Include(s => s.HRSkills).Include(a => a.Archive).Include(k => k.KPI).ToList();
            foreach (var task in processModels.Tasks)
            {
                task.Origin = db.Entities.Join(db.TaskEntities.Where(x => x.TaskID == task.TaskID && x.EntityType == TaskEntityType.Origin), entity => entity.EntityID, taskE => taskE.EntityID, (entity, taskE) => entity).ToList();
                task.Destination = db.Entities.Join(db.TaskEntities.Where(x => x.TaskID == task.TaskID && x.EntityType == TaskEntityType.Destination), entity => entity.EntityID, taskE => taskE.EntityID, (entity, taskE) => entity).ToList();
                task.Input = db.SysUrls.Where(s => s.SysUrlType == SysUrlType.Input && s.TaskID == task.TaskID).ToList();
                task.Output = db.SysUrls.Where(s => s.SysUrlType == SysUrlType.Output && s.TaskID == task.TaskID).ToList();
                task.DocRef = db.SysUrls.Where(s => s.SysUrlType == SysUrlType.DocRef && s.TaskID == task.TaskID).ToList();
            }

            return View(processModels);
        }

        // POST: Update field
        [HttpPost]
        public ActionResult Update(JsonComboUpdate psvm)
        {
            int success = 0;
            string message = "Unable to save in database";
            switch (psvm.Operation)
            {
                case OperationType.Name:
                    {
                        ProcessModels processModels = db.ProcessModels.Find(psvm.Identifier);
                        processModels.Name = psvm.Value;
                        db.SaveChanges();
                        success = 1;
                        message = "";

                        break;
                    }
                case OperationType.Parent:
                    {
                        ProcessModels processModels = db.ProcessModels.Find(psvm.Identifier);
                        ProcessModels tmp = null;
                        processModels.ParentID = psvm.Value == null ? null : (int?)int.Parse(psvm.Value);
                        if(processModels.ParentID == null)
                        {
                            processModels.Level = 0;
                            TasksController.RemoveAllTasks(processModels.ProcessID);
                        } else
                        {
                            tmp = db.ProcessModels.Find(processModels.ParentID);
                            processModels.Level = tmp.Level + 1;
                            if(db.Tasks.Where(x => x.ProcessID == processModels.ProcessID).Count() == 0)
                            {
                                TasksController.AddNewTask(processModels.ProcessID);
                            }
                        }
                        db.SaveChanges();
                        success = 2;
                        message = "";
                        break;
                    }
                case OperationType.Procedure:
                    {
                        ProcessModels processModels = db.ProcessModels.Find(psvm.Identifier);
                        processModels.ProcedureID = psvm.Value == null ? null : (int?)int.Parse(psvm.Value);
                        db.SaveChanges();
                        success = 1;
                        message = "";
                        break;
                    }
                case OperationType.Manager:
                    {
                        ProcessModels processModels = db.ProcessModels.Find(psvm.Identifier);
                        processModels.ManagerID = psvm.Value == null ? null : (int?)int.Parse(psvm.Value);
                        db.SaveChanges();
                        success = 1;
                        message = "";
                        break;
                    }
            }
            return Json(new { code = success, message = message }, "application/json");
        }

        // GET: ProcessModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProcessModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProcessID,Name")] ProcessModels processModels)
        {
            if (ModelState.IsValid)
            {
                processModels.CreationTime = DateTime.Now;
                db.ProcessModels.Add(processModels);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = processModels.ProcessID });
            }

            return View(processModels);
        }

        // GET: ProcessModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProcessModels processModels = db.ProcessModels.Find(id);
            if (processModels == null)
            {
                return HttpNotFound();
            }

            SetViewBag(processModels);

            ICollection<Tasks> tasks = db.Tasks.Where(t => t.ProcessID == id).Include(t => t.HRFunction).Include(t => t.HRSkills).Include(t => t.Name).Include(t => t.OrganisationUnit).Include(t => t.Process).Include(t => t.SupportingIS).ToList();

            foreach (var item in tasks)
            {
                item.Input = db.SysUrls.Where(i => (i.SysUrlType == SysUrlType.Input) && (i.TaskID == item.TaskID)).ToList();
                item.Output = db.SysUrls.Where(i => (i.SysUrlType == SysUrlType.Output) && (i.TaskID == item.TaskID)).ToList();
                item.DocRef = db.SysUrls.Where(i => (i.SysUrlType == SysUrlType.DocRef) && (i.TaskID == item.TaskID)).ToList();
            }

            SetViewBag(tasks);

            processModels.Tasks = tasks;
            return View(processModels);
        }

        // GET: ProcessModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProcessModels processModels = db.ProcessModels.Find(id);
            if (processModels == null)
            {
                return HttpNotFound();
            }
            return View(processModels);
        }

        // POST: ProcessModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProcessModels processModels = db.ProcessModels.Find(id);
            db.ProcessModels.Remove(processModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult UploadFile(int processID)
        {
            HttpPostedFileBase myFile = Request.Files.Count > 0 ? Request.Files[0] : null;
            bool isUploaded = false;
            string message = "File upload failed";

            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/Uploads");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        var request = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                        ProcessModels processModels = db.ProcessModels.Find(processID);
                        processModels.FluxDiagram = request + "Uploads/" + myFile.FileName;
                        db.SaveChanges();
                        message = request + "Uploads/" + myFile.FileName;
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { code = (isUploaded ? 0 : 1), url = message, data = message }, "text/html");
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
