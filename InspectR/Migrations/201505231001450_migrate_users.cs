namespace InspectR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class migrate_users : DbMigration
    {
        public override void Up()
        {
            Sql(@"
 INSERT INTO AspNetUsers(Id, UserName, PasswordHash, SecurityStamp)
 SELECT UserProfile.UserId, UserProfile.UserName, webpages_Membership.Password, webpages_Membership.PasswordSalt 
 FROM UserProfile
 LEFT OUTER JOIN webpages_Membership ON UserProfile.UserId = webpages_Membership.UserId");
            Sql(@"
 INSERT INTO AspNetRoles(Id, Name)
 SELECT RoleId, RoleName
 FROM webpages_Roles");
            Sql(@"
 INSERT INTO AspNetUserRoles(UserId, RoleId)
 SELECT UserId, RoleId
 FROM webpages_UsersInRoles");

            Sql(@"
 INSERT INTO AspNetUserLogins(UserId, LoginProvider, ProviderKey)
 SELECT UserId, Provider, ProviderUserId
 FROM webpages_OAuthMembership");
        }

        public override void Down()
        {
        }
    }
}
