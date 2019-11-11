using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TzCA.DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BinaryContent = table.Column<byte[]>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileSuffix = table.Column<string>(maxLength: 10, nullable: true),
                    Icon = table.Column<string>(maxLength: 120, nullable: true),
                    IsInDB = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    OriginalFileName = table.Column<string>(maxLength: 500, nullable: true),
                    PhysicalPath = table.Column<string>(nullable: true),
                    RelativePath = table.Column<string>(nullable: true),
                    RelevanceObjectId = table.Column<Guid>(nullable: false),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UploaderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileSuffix = table.Column<string>(maxLength: 256, nullable: true),
                    Height = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(maxLength: 120, nullable: true),
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    OriginalFileName = table.Column<string>(maxLength: 256, nullable: true),
                    PhysicalPath = table.Column<string>(nullable: true),
                    RelativePath = table.Column<string>(nullable: true),
                    RelevanceObjectId = table.Column<Guid>(nullable: false),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UploaderId = table.Column<Guid>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BinaryContent = table.Column<byte[]>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileSuffix = table.Column<string>(maxLength: 10, nullable: true),
                    Icon = table.Column<string>(maxLength: 120, nullable: true),
                    IsInDB = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    OriginalFileName = table.Column<string>(maxLength: 500, nullable: true),
                    PhysicalPath = table.Column<string>(nullable: true),
                    RelativePath = table.Column<string>(nullable: true),
                    RelevanceObjectId = table.Column<Guid>(nullable: false),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UploaderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    IsActiveDepartment = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ParentDepartmentId = table.Column<Guid>(nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemWorkPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    IconString = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    SystemWorkPlaceType = table.Column<int>(nullable: false),
                    URL = table.Column<string>(maxLength: 250, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemWorkPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatRecordContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChatRecordId = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRecordContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRecordContents_ChatRecords_ChatRecordId",
                        column: x => x.ChatRecordId,
                        principalTable: "ChatRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationRoleType = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(maxLength: 550, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    SortCode = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AvatarId = table.Column<Guid>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CredentialsCode = table.Column<string>(maxLength: 26, nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    EmployeeCode = table.Column<string>(maxLength: 50, nullable: true),
                    ExpiredDateTime = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    InquiryPassword = table.Column<string>(maxLength: 50, nullable: true),
                    JobTitleId = table.Column<Guid>(nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Sex = table.Column<bool>(nullable: false),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    TelephoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_BusinessImages_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Persons_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Persons_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemWorkSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    SystemWorkPlaceId = table.Column<Guid>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemWorkSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemWorkSections_SystemWorkPlaces_SystemWorkPlaceId",
                        column: x => x.SystemWorkPlaceId,
                        principalTable: "SystemWorkPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AvatarId = table.Column<Guid>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PersonId = table.Column<Guid>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_BusinessImages_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuditDateTime = table.Column<DateTime>(nullable: false),
                    AuditResult = table.Column<int>(nullable: false),
                    AuditorId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ObjectId = table.Column<Guid>(nullable: false),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditRecords_Persons_AuditorId",
                        column: x => x.AuditorId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemWorkTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessEntityName = table.Column<string>(maxLength: 100, nullable: true),
                    ControllerMethod = table.Column<string>(maxLength: 100, nullable: true),
                    ControllerMethodParameter = table.Column<string>(maxLength: 500, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    HasShortCutLinkItem = table.Column<bool>(nullable: false),
                    HasTileLinkItem = table.Column<bool>(nullable: false),
                    IconName = table.Column<string>(maxLength: 100, nullable: true),
                    IsForDefaultSystemRoleGroup = table.Column<bool>(nullable: false),
                    IsForMeOnly = table.Column<bool>(nullable: false),
                    IsForMyDepartmentOnly = table.Column<bool>(nullable: false),
                    IsUsedInMenu = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    SystemWorkSectionId = table.Column<Guid>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemWorkTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemWorkTasks_SystemWorkSections_SystemWorkSectionId",
                        column: x => x.SystemWorkSectionId,
                        principalTable: "SystemWorkSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_DepartmentId",
                table: "AspNetRoles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonId",
                table: "AspNetUsers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditRecords_AuditorId",
                table: "AuditRecords",
                column: "AuditorId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRecordContents_ChatRecordId",
                table: "ChatRecordContents",
                column: "ChatRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_AvatarId",
                table: "Persons",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_DepartmentId",
                table: "Persons",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_JobTitleId",
                table: "Persons",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemWorkSections_SystemWorkPlaceId",
                table: "SystemWorkSections",
                column: "SystemWorkPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemWorkTasks_SystemWorkSectionId",
                table: "SystemWorkTasks",
                column: "SystemWorkSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditRecords");

            migrationBuilder.DropTable(
                name: "BusinessFiles");

            migrationBuilder.DropTable(
                name: "BusinessVideos");

            migrationBuilder.DropTable(
                name: "ChatRecordContents");

            migrationBuilder.DropTable(
                name: "SystemWorkTasks");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChatRecords");

            migrationBuilder.DropTable(
                name: "SystemWorkSections");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "SystemWorkPlaces");

            migrationBuilder.DropTable(
                name: "BusinessImages");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "JobTitles");
        }
    }
}
