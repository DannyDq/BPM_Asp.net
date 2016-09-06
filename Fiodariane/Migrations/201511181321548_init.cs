namespace Fiodariane.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        EntityID = c.Int(nullable: false, identity: true),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Tasks_TaskID = c.Int(),
                        Tasks_TaskID1 = c.Int(),
                    })
                .PrimaryKey(t => t.EntityID)
                .ForeignKey("dbo.Tasks", t => t.Tasks_TaskID)
                .ForeignKey("dbo.Tasks", t => t.Tasks_TaskID1)
                .Index(t => t.Tasks_TaskID)
                .Index(t => t.Tasks_TaskID1);
            
            CreateTable(
                "dbo.HRSkills",
                c => new
                    {
                        HRSkillsID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.HRSkillsID);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        ManagerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ManagerID);
            
            CreateTable(
                "dbo.Procedures",
                c => new
                    {
                        ProcedureID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.ProcedureID);
            
            CreateTable(
                "dbo.ProcessModels",
                c => new
                    {
                        ProcessID = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(),
                        Name = c.String(),
                        ProcedureID = c.Int(),
                        ManagerID = c.Int(),
                        FluxDiagram = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProcessID)
                .ForeignKey("dbo.Managers", t => t.ManagerID)
                .ForeignKey("dbo.ProcessModels", t => t.ParentID)
                .ForeignKey("dbo.Procedures", t => t.ProcedureID)
                .Index(t => t.ParentID)
                .Index(t => t.ProcedureID)
                .Index(t => t.ManagerID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        NameID = c.Int(nullable: false),
                        SupportingISID = c.Int(),
                        HRFunctionID = c.Int(),
                        OrganisationUnitID = c.Int(),
                        HRSkillsID = c.Int(),
                        ArchiveID = c.Int(),
                        KPIID = c.Int(),
                        ProcessID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.SysUrls", t => t.ArchiveID)
                .ForeignKey("dbo.Entities", t => t.HRFunctionID)
                .ForeignKey("dbo.HRSkills", t => t.HRSkillsID)
                .ForeignKey("dbo.SysUrls", t => t.KPIID)
                .ForeignKey("dbo.SysUrls", t => t.NameID, cascadeDelete: true)
                .ForeignKey("dbo.Entities", t => t.OrganisationUnitID)
                .ForeignKey("dbo.ProcessModels", t => t.ProcessID, cascadeDelete: true)
                .ForeignKey("dbo.SupportingIS", t => t.SupportingISID)
                .Index(t => t.NameID)
                .Index(t => t.SupportingISID)
                .Index(t => t.HRFunctionID)
                .Index(t => t.OrganisationUnitID)
                .Index(t => t.HRSkillsID)
                .Index(t => t.ArchiveID)
                .Index(t => t.KPIID)
                .Index(t => t.ProcessID);
            
            CreateTable(
                "dbo.SysUrls",
                c => new
                    {
                        SysUrlID = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 300),
                        Description = c.String(maxLength: 160),
                        Embedded = c.String(maxLength: 500),
                        SysUrlType = c.Int(nullable: false),
                        TaskID = c.Int(nullable: false),
                        Tasks_TaskID = c.Int(),
                        Tasks_TaskID1 = c.Int(),
                        Tasks_TaskID2 = c.Int(),
                    })
                .PrimaryKey(t => t.SysUrlID)
                .ForeignKey("dbo.Tasks", t => t.Tasks_TaskID)
                .ForeignKey("dbo.Tasks", t => t.Tasks_TaskID1)
                .ForeignKey("dbo.Tasks", t => t.Tasks_TaskID2)
                .Index(t => t.Tasks_TaskID)
                .Index(t => t.Tasks_TaskID1)
                .Index(t => t.Tasks_TaskID2);
            
            CreateTable(
                "dbo.SupportingIS",
                c => new
                    {
                        SupportingISID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SupportingISID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.TaskEntities",
                c => new
                    {
                        TaskEntityID = c.Int(nullable: false, identity: true),
                        EntityID = c.Int(nullable: false),
                        EntityType = c.Int(nullable: false),
                        TaskID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskEntityID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Tasks", "SupportingISID", "dbo.SupportingIS");
            DropForeignKey("dbo.Tasks", "ProcessID", "dbo.ProcessModels");
            DropForeignKey("dbo.SysUrls", "Tasks_TaskID2", "dbo.Tasks");
            DropForeignKey("dbo.Entities", "Tasks_TaskID1", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "OrganisationUnitID", "dbo.Entities");
            DropForeignKey("dbo.Tasks", "NameID", "dbo.SysUrls");
            DropForeignKey("dbo.Tasks", "KPIID", "dbo.SysUrls");
            DropForeignKey("dbo.SysUrls", "Tasks_TaskID1", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "HRSkillsID", "dbo.HRSkills");
            DropForeignKey("dbo.Tasks", "HRFunctionID", "dbo.Entities");
            DropForeignKey("dbo.SysUrls", "Tasks_TaskID", "dbo.Tasks");
            DropForeignKey("dbo.Entities", "Tasks_TaskID", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "ArchiveID", "dbo.SysUrls");
            DropForeignKey("dbo.ProcessModels", "ProcedureID", "dbo.Procedures");
            DropForeignKey("dbo.ProcessModels", "ParentID", "dbo.ProcessModels");
            DropForeignKey("dbo.ProcessModels", "ManagerID", "dbo.Managers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SysUrls", new[] { "Tasks_TaskID2" });
            DropIndex("dbo.SysUrls", new[] { "Tasks_TaskID1" });
            DropIndex("dbo.SysUrls", new[] { "Tasks_TaskID" });
            DropIndex("dbo.Tasks", new[] { "ProcessID" });
            DropIndex("dbo.Tasks", new[] { "KPIID" });
            DropIndex("dbo.Tasks", new[] { "ArchiveID" });
            DropIndex("dbo.Tasks", new[] { "HRSkillsID" });
            DropIndex("dbo.Tasks", new[] { "OrganisationUnitID" });
            DropIndex("dbo.Tasks", new[] { "HRFunctionID" });
            DropIndex("dbo.Tasks", new[] { "SupportingISID" });
            DropIndex("dbo.Tasks", new[] { "NameID" });
            DropIndex("dbo.ProcessModels", new[] { "ManagerID" });
            DropIndex("dbo.ProcessModels", new[] { "ProcedureID" });
            DropIndex("dbo.ProcessModels", new[] { "ParentID" });
            DropIndex("dbo.Entities", new[] { "Tasks_TaskID1" });
            DropIndex("dbo.Entities", new[] { "Tasks_TaskID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TaskEntities");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SupportingIS");
            DropTable("dbo.SysUrls");
            DropTable("dbo.Tasks");
            DropTable("dbo.ProcessModels");
            DropTable("dbo.Procedures");
            DropTable("dbo.Managers");
            DropTable("dbo.HRSkills");
            DropTable("dbo.Entities");
        }
    }
}
