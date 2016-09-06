namespace Fiodariane.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Fiodariane.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Fiodariane.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var entities = new List<Entity>
            {
                new Entity { EntityID = 1, Name = "Recruiter", EntityType=EntityType.HRFunction },
                new Entity { EntityID = 2, Name = "Administration", EntityType=EntityType.OrganisationUnit },
            };

            entities.ForEach(e => context.Entities.AddOrUpdate(s => s.EntityID, e));

            var taskEntities = new List<TaskEntity>
            {
                new TaskEntity { EntityID = 1, EntityType=TaskEntityType.Origin, TaskID = 1 },
                new TaskEntity { EntityID = 2, EntityType=TaskEntityType.Destination, TaskID = 1 },
            };
            taskEntities.ForEach(e => context.TaskEntities.AddOrUpdate(e));

            var managers = new List<Manager>
            {
                new Manager { ManagerID = 1, Name="John"},
                new Manager { ManagerID = 2, Name="Mary" }
            };

            managers.ForEach(m => context.Managers.AddOrUpdate(t => t.ManagerID, m));

            var procedures = new List<Procedure>
            {
                new Procedure { ProcedureID = 1, Name = "Approval", Link="#" },
                new Procedure { ProcedureID = 2, Name = "Purchase", Link="#" }
            };

            procedures.ForEach(p => context.Procedures.AddOrUpdate(t => t.ProcedureID, p));

            var supportingis = new List<SupportingIS>
            {
                new SupportingIS { SupportingISID = 1, Name="Oracle" },
                new SupportingIS { SupportingISID = 2, Name="SQL Server" }
            };

            supportingis.ForEach(s => context.SupportingIS.AddOrUpdate(t => t.SupportingISID, s));

            var skills = new List<HRSkills>
            {
                new HRSkills { HRSkillsID = 1, Name= "Safety" },
                new HRSkills { HRSkillsID = 2, Name = "HR" }
            };

            skills.ForEach(s => context.HRSkills.AddOrUpdate(t => t.HRSkillsID, s));

            var processes = new List<ProcessModels> {
                new ProcessModels
                {
                    ProcessID = 1,
                    CreationTime = DateTime.Now,
                    ManagerID = 1,
                    ProcedureID = 1,
                    FluxDiagram = "/Content/img/flux.png",
                    Name = "Macro-Process",
                    Level = 0
                },
                new ProcessModels
                {
                    ProcessID = 2,
                    CreationTime = DateTime.Now,
                    ManagerID = 2,
                    ProcedureID = 2,
                    FluxDiagram = "/Content/img/flux.png",
                    Name = "Process",
                    ParentID = 1,
                    Level = 1
                },
                new ProcessModels
                {
                    ProcessID = 3,
                    CreationTime = DateTime.Now,
                    ManagerID = 2,
                    ProcedureID = 2,
                    FluxDiagram = "/Content/img/flux.png",
                    Name = "Subprocess",
                    ParentID = 2,
                    Level = 2
                }
            };

            processes.ForEach(p => context.ProcessModels.AddOrUpdate(c => c.ProcessID, p));

            var tasks = new List<Tasks>
            {
                new Tasks
                {
                    TaskID = 1,
                    NameID = 1,
                    ProcessID = 2,
                    SupportingISID = 1,
                    ArchiveID = 2,
                    HRFunctionID = 1,
                    HRSkillsID = 1,
                    KPIID = 3,
                    OrganisationUnitID = 2
                }
            };

            tasks.ForEach(t => context.Tasks.AddOrUpdate(c => c.TaskID, t));

            var sysurls = new List<SysUrl>
            {
                new SysUrl
                {
                    SysUrlID = 1,
                    TaskID = 1,
                    Description = "My Task",
                    Embedded = "Stuff has to happen for completion.",
                    SysUrlType = SysUrlType.Name
                },
                new SysUrl
                {
                    SysUrlID = 2,
                    TaskID = 1,
                    Description = "My Archive",
                    Url = "#",
                    SysUrlType = SysUrlType.Archive
                },
                new SysUrl
                {
                    SysUrlID = 3,
                    TaskID = 1,
                    Description = "My KPI",
                    Url = "#",
                    SysUrlType = SysUrlType.KPI
                },
                new SysUrl
                {
                    SysUrlID = 4,
                    TaskID = 1,
                    Description = "First Input",
                    Url = "http://www.intergraph.com",
                    SysUrlType = SysUrlType.Input

                },
                new SysUrl
                {
                    SysUrlID = 5,
                    TaskID = 1,
                    Description = "Second Input",
                    Url = "#",
                    SysUrlType = SysUrlType.Input
                },
                new SysUrl
                {
                    SysUrlID = 6,
                    TaskID = 1,
                    Description = "First Output",
                    Url = "#",
                    SysUrlType = SysUrlType.Output
                },
                new SysUrl
                {
                    SysUrlID = 7,
                    TaskID = 1,
                    Description = "Second Output",
                    Url = "#",
                    SysUrlType = SysUrlType.Output
                }

            };
            sysurls.ForEach(s => context.SysUrls.AddOrUpdate(t => t.SysUrlID, s));

        }
    }
}
