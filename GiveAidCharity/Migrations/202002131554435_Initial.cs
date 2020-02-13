namespace GiveAidCharity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(maxLength: 128),
                        ProjectId = c.String(maxLength: 128),
                        Title = c.String(nullable: false),
                        ContentPart1 = c.String(nullable: false),
                        ContentPart2 = c.String(nullable: false),
                        ContentPart3 = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Avatar = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                        Zipcode = c.String(),
                        CompanyName = c.String(),
                        Gender = c.Int(nullable: false),
                        Birthday = c.DateTime(),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
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
                "dbo.Donations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(maxLength: 128),
                        ProjectId = c.String(maxLength: 128),
                        Amount = c.Double(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(maxLength: 128),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        CoverImg = c.String(nullable: false),
                        ContentPart1 = c.String(nullable: false),
                        ContentPart2 = c.String(nullable: false),
                        Goal = c.Double(nullable: false),
                        CurrentFund = c.Double(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(maxLength: 128),
                        ProjectId = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectComments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(maxLength: 128),
                        ProjectId = c.String(maxLength: 128),
                        ParentId = c.String(),
                        Content = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectImages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProjectId = c.String(maxLength: 128),
                        Url = c.String(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Blogs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Blogs", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Donations", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectImages", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectComments", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectComments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Follows", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Donations", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ProjectImages", new[] { "ProjectId" });
            DropIndex("dbo.ProjectComments", new[] { "ProjectId" });
            DropIndex("dbo.ProjectComments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Follows", new[] { "ProjectId" });
            DropIndex("dbo.Follows", new[] { "ApplicationUserId" });
            DropIndex("dbo.Projects", new[] { "ApplicationUserId" });
            DropIndex("dbo.Donations", new[] { "ProjectId" });
            DropIndex("dbo.Donations", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Blogs", new[] { "ProjectId" });
            DropIndex("dbo.Blogs", new[] { "ApplicationUserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.ProjectImages");
            DropTable("dbo.ProjectComments");
            DropTable("dbo.Follows");
            DropTable("dbo.Projects");
            DropTable("dbo.Donations");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Blogs");
        }
    }
}
