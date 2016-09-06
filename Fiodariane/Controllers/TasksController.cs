using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fiodariane.Models;
using Newtonsoft.Json;

namespace Fiodariane.Controllers
{
    public class TasksController : Controller
    {
        /**
        * Database context
        */
        private ApplicationDbContext db = new ApplicationDbContext();

        /**
        * Creates a new task and adds it to the database. The process ID is used as 
        * a parameter and the task is added to that process ID
        */
        private Tasks CreateNewTask(int processID)
        {
            Tasks newTask = new Tasks();
            newTask.ProcessID = processID;
            db.Tasks.Add(newTask);
            SysUrl sysUrl = CreateNewSysUrl(SysUrlType.Name, newTask.TaskID);
            sysUrl.TaskID = newTask.TaskID;
            newTask.NameID = sysUrl.SysUrlID;
            db.SaveChanges();
            sysUrl = CreateNewSysUrl(SysUrlType.Archive, newTask.TaskID);
            sysUrl.TaskID = newTask.TaskID;
            newTask.ArchiveID = sysUrl.SysUrlID;
            db.SaveChanges();
            sysUrl = CreateNewSysUrl(SysUrlType.KPI, newTask.TaskID);
            sysUrl.TaskID = newTask.TaskID;
            newTask.KPIID = sysUrl.SysUrlID;
            int max = db.Tasks.Where(x => x.ProcessID == processID).Max(x => x.Order);
            newTask.Order = max + 1;
            db.SaveChanges();
            return newTask;
        }
        /**
        * Creates a new embedded link and associates a type to it. 
        * This allows embedded links to be easily managed as they are handled similarly and only
        * only depend on their types.
        * A type is used as a parameter
        */
        private SysUrl CreateNewSysUrl(SysUrlType type, int taskID)
        {
            SysUrl newSysUrl = new SysUrl();
            newSysUrl.SysUrlType = type;
            newSysUrl.TaskID = taskID;
            db.SysUrls.Add(newSysUrl);
            db.SaveChanges();
            return newSysUrl;
        }

        /**
        * Remove all tasks from a process.
        */
        private void RemoveTasks(int processID)
        {
            List<Tasks> tasks = db.Tasks.Where(x => x.ProcessID == processID).ToList();
            foreach(Tasks t in tasks)
            {
                db.SysUrls.RemoveRange(db.SysUrls.Where(x => x.TaskID == t.TaskID).ToList());
                db.TaskEntities.RemoveRange(db.TaskEntities.Where(x => x.TaskID == t.TaskID).ToList());
                db.Tasks.Remove(t);
            }
        }

        /**
        * Public method to remove all tasks
        */
        public static void RemoveAllTasks(int processID)
        {
            TasksController tc = new TasksController();
            tc.RemoveTasks(processID);

        }

        /**
        * Public method to allow a process to create a task
        */
        public static Tasks AddNewTask(int processID)
        {
            TasksController tc = new TasksController();
            Tasks task = tc.CreateNewTask(processID);
            return task;
        }
        /**
        * Updates the value of an existing embedded link
        * The link and values to update are the parameters
        */
        private void UpdateSysUrl(SysUrl sysUrl, JsonSysUrlUpdate update)
        {
            sysUrl.Description = update.name;
            sysUrl.Url = update.url;
            sysUrl.Embedded = update.embedded;
            db.SaveChanges();
        }

        /**
        * Updates values of all embedded links in the page.
        * The parameter has the values sent from the page
        * In case the task does not exist, a new task will be created.
        * After execution, code == 1 is success, and 0 if the method fails
        * message includes the error in case of failure.
        */
        [HttpPost]
        public ActionResult Update(JsonSysUrlUpdate update)
        {
            Tasks task = null;
            int code = 1;
            string message = "";
            if (update.task_id == 0)
            {
                task = CreateNewTask(update.process_id);
            } else
            {
                task = db.Tasks.Find(update.task_id);
            }
            if(task == null)
            {
                code = 0;
                message = "Unknown Task";
            }
            else
            {
                SysUrl sysUrl;
                if (update.sysurl_id == 0)
                {
                    sysUrl = CreateNewSysUrl(update.operation, task.TaskID);
                    switch (update.operation)
                    {
                        case SysUrlType.Name:
                            task.NameID = sysUrl.SysUrlID;
                            break;
                        case SysUrlType.Archive:
                            task.ArchiveID = sysUrl.SysUrlID;
                            break;
                        case SysUrlType.KPI:
                            task.KPIID = sysUrl.SysUrlID;
                            break;
                    }
                }
                else
                {
                    sysUrl = db.SysUrls.Find(update.sysurl_id);
                }
                if (sysUrl == null)
                {
                    code = 0;
                    message = "Unknown Field";
                }
                else
                    UpdateSysUrl(sysUrl, update);
            }
            return Json(new { code=code, message=message }, "application/json"); ;
        }

        /**
        * Updates values of the select links in the page.
        * In case the task does not exist, a new task will be created.
        * After execution, code == 1 is success, and 0 if the method fails
        * message includes the error in case of failure.
        */
        [HttpPost]
        public ActionResult UpdateCheck(JsonCheckUpdate update)
        {
            Tasks task = null;
            int code = 1;
            string message = "";
            if (update.task_id == 0)
            {
                task = CreateNewTask(update.process_id);
            }
            else
            {
                task = db.Tasks.Find(update.task_id);
            }
            if (task == null)
            {
                code = 0;
                message = "Unknown Task";
            }
            else
            {
                if(update.task_id != 0)
                {
                    List<TaskEntity> list = db.TaskEntities.Where(t => t.TaskID == update.task_id && t.EntityType == update.operation).ToList();
                    foreach (var t in list)
                    {
                        db.TaskEntities.Remove(t);
                    }
                }
                foreach (var value in update.values)
                {
                    db.TaskEntities.Add(new TaskEntity { EntityID = value, EntityType = update.operation, TaskID = task.TaskID });
                }
            }
            return Json(new { code=code, message=message }, "application/json"); ;
        }

        /**
        * Updates values from the combo boxes in the edit page
        * After execution, code == 1 is success, and 0 if the method fails
        * message includes the error in case of failure.
        */
        [HttpPost]
        public ActionResult UpdateDrop(JsonComboUpdate update)
        {
            int success = 0;
            string message = "Unable to save in database";
            Tasks task = db.Tasks.Find(update.TaskID);
            if (task == null)
            {
                message = "Task not found.";
            } else
            {
                switch (update.Operation)
                {
                    case OperationType.SupportingIS:
                        {
                            int sisID = 0;
                            int.TryParse(update.Value, out sisID);
                            if (update.Value != null && db.SupportingIS.Where(x => sisID == x.SupportingISID).Count() <= 0)
                            {
                                message = "Unknown system.";
                            } else
                            {
                                task.SupportingISID = sisID == 0 ? (int?)null : sisID;
                                db.SaveChanges();
                                success = 1;
                                message = "";
                            }
                            break;
                        }
                    case OperationType.HRFunction:
                        {
                            int entityID = 0;
                            int.TryParse(update.Value, out entityID);
                            if (update.Value != null && db.Entities.Where(x => entityID == x.EntityID).Count() <= 0)
                            {
                                message = "Unknown HR Function.";
                            }
                            else
                            {
                                task.HRFunctionID = entityID == 0 ? (int?) null : entityID;
                                db.SaveChanges();
                                success = 1;
                                message = "";
                            }
                            break;
                        }
                    case OperationType.OrganisationUnit:
                        {
                            int organisationUnitID = 0;
                            int.TryParse(update.Value, out organisationUnitID);
                            if (update.Value != null && db.Entities.Where(x => organisationUnitID == x.EntityID).Count() <=0)
                            {
                                message = "Unknown Organisation.";
                            }
                            else
                            {
                                task.OrganisationUnitID = organisationUnitID == 0 ? (int?) null : organisationUnitID;
                                db.SaveChanges();
                                success = 1;
                                message = "";
                            }
                            break;
                        }
                    case OperationType.Skills:
                        {
                            int skillsID = 0;
                            int.TryParse(update.Value, out skillsID);
                            if (update.Value != null && db.HRSkills.Where(x => skillsID == x.HRSkillsID).Count() <= 0)
                            {
                                message = "Unknown skill.";
                            }
                            else
                            {
                                task.HRSkillsID = skillsID == 0 ? (int?) null : skillsID;
                                db.SaveChanges();
                                success = 1;
                                message = "";
                            }
                            break;
                        }
                    default:
                        {
                            success = 0;
                            message = "Unknown option";
                            break;
                        }
                }
            }
            return Json(new { code = success, message=message }, "application/json");
        }

        /**
        * Adds or Deletes a SysUrl (embedded link to the list)
        */
        [HttpPost]
        public ActionResult ManageSysUrl(JsonSysUrl sysUrlMessage)
        {
            SysUrl aSysUrl = null;
            JsonResult res;
            switch (sysUrlMessage.OperationType)
            {
                case TaskType.Add:
                    {
                        Tasks task = db.Tasks.Find(sysUrlMessage.TaskID);
                        if(task == null)
                        {
                            res = Json(new { code = 0, message = "Unknown task" }, "application/json");
                        } else
                        {
                            aSysUrl = CreateNewSysUrl(sysUrlMessage.SysUrlType, task.TaskID);
                            res = Json(new { code = 1, message = Json(new { sysurl_id = aSysUrl.SysUrlID}).Data }, "application/json");
                        }
                        break;
                    }
                case TaskType.Delete:
                    {

                        aSysUrl = db.SysUrls.Find(sysUrlMessage.SysUrlID);
                        if (aSysUrl == null)
                        {
                            res = Json(new { code = 0, message = "Unknown link" }, "application/json");
                        }
                        else
                        {
                            db.SysUrls.Remove(aSysUrl);
                            db.SaveChanges();
                            res = Json(new { code = 1, message = "" }, "application/json");
                        }
                        break;
                    }
                default:
                    {
                        res = Json(new { code = 0, message = "Unknown operation" }, "application/json");
                        break;
                    }
            }
            return res;
        }

        /**
        * Creates and deletes tasks associated with a process ID
        */
        [HttpPost]
        public ActionResult ManageTasks(JsonTask taskMessage)
        {
            Tasks task = null;
            ActionResult res;
            switch(taskMessage.TaskType)
            {
                case TaskType.Add:
                    {
                        task = CreateNewTask(taskMessage.ProcessID);
                        res = Json(new { code = 1, message = Json(new { task_id = task.TaskID, name_id = task.NameID, archive_id = task.ArchiveID, kpi_id = task.KPIID }).Data }, "application/json");
                        break;
                    }
                case TaskType.Delete:
                    {
                        task = db.Tasks.Find(taskMessage.TaskID);
                        if (task == null)
                        {
                            res = Json(new { code = 0, message = "Unknown task" }, "application/json");
                        }
                        else
                        {
                            db.SysUrls.RemoveRange(db.SysUrls.Where(x => x.TaskID == task.TaskID).ToList());
                            db.TaskEntities.RemoveRange(db.TaskEntities.Where(x => x.TaskID == task.TaskID).ToList());
                            db.Tasks.Remove(task);
                            db.SaveChanges();
                            res = Json(new { code = 1, message = "" }, "application/json");
                        }
                        break;
                    }
               case TaskType.Clear:
                    {
                        //Update this code
                        task = db.Tasks.Find(taskMessage.TaskID);
                        if (task == null)
                        {
                            res = Json(new { code = 0, message = "Unknown task" }, "application/json");
                        }
                        else
                        {
                            SysUrl aSysUrl = null;
                            task.OrganisationUnitID = (int?)null;
                            task.SupportingISID = (int?)null;
                            task.HRFunctionID = (int?)null;
                            task.HRSkillsID = (int?)null;
                            aSysUrl = db.SysUrls.Where(x => x.TaskID == task.TaskID && x.SysUrlType == SysUrlType.Name).SingleOrDefault();
                            aSysUrl.Embedded = null;
                            aSysUrl.Url = null;
                            aSysUrl.Description = null;
                            aSysUrl = db.SysUrls.Where(x => x.TaskID == task.TaskID && x.SysUrlType == SysUrlType.Archive).SingleOrDefault();
                            aSysUrl.Embedded = null;
                            aSysUrl.Url = null;
                            aSysUrl.Description = null;
                            aSysUrl = db.SysUrls.Where(x => x.TaskID == task.TaskID && x.SysUrlType == SysUrlType.KPI).SingleOrDefault();
                            aSysUrl.Embedded = null;
                            aSysUrl.Url = null;
                            aSysUrl.Description = null;
                            db.SysUrls.RemoveRange(db.SysUrls.Where(x => (x.TaskID == task.TaskID) && (x.SysUrlType != SysUrlType.Name && x.SysUrlType != SysUrlType.Archive && x.SysUrlType != SysUrlType.KPI)).ToList());
                            db.TaskEntities.RemoveRange(db.TaskEntities.Where(x => x.TaskID == task.TaskID).ToList());
                            db.SaveChanges();
                            res = Json(new { code = 2, message = "" }, "application/json");
                        }
                        break;
                    }
                default:
                    {
                        res = Json(new { code = 0, message = "Unknown operation" }, "application/json");
                        break;
                    }
            }
            return res;
        }


        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tasks tasks = db.Tasks.Find(id);
            db.Tasks.Remove(tasks);
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
