using FluentMigrator;

namespace ICICI.AppCode.Migrations
{
    [Migration(202106280001)]
    public class InitialTables_202106280001 : Migration
    {
        public override void Down()
        {
            Delete.Table("Employees");
            Delete.Table("Companies");
        }

        public override void Up()
        {
            /* Role */
            Create.Table("ApplicationRole")
               .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
               .WithColumn("ConcurrencyStamp").AsString(1000).NotNullable()
               .WithColumn("Name").AsString(256).NotNullable()
               .WithColumn("NormalizedName").AsString(256).NotNullable();
            /* Role Claim */
            Create.Table("RoleClaims")
               .WithColumn("Id").AsInt64().NotNullable().Identity()
               .WithColumn("ClaimType").AsString(1000).NotNullable()
               .WithColumn("ClaimValue").AsString(1000).NotNullable()
               .WithColumn("RoleId").AsInt64().NotNullable()
               .WithColumn("Country").AsString(50).NotNullable();

            /* Users */
            Create.Table("Users")
               .WithColumn("Id").AsInt64().NotNullable().Identity()
               .WithColumn("UserId").AsString(1000).NotNullable().PrimaryKey()
               .WithColumn("AccessFailedCount").AsInt64()
               .WithColumn("ConcurrencyStamp").AsString(1000)
               .WithColumn("Email").AsString(256).NotNullable()
               .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
               .WithColumn("LockoutEnabled").AsBoolean()
               .WithColumn("LockoutEnd").AsDateTime()
               .WithColumn("NormalizedEmail").AsString(256).NotNullable()
               .WithColumn("NormalizedUserName").AsString(256).NotNullable()
               .WithColumn("PasswordHash").AsString(256).NotNullable()
               .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
               .WithColumn("PhoneNumber").AsString()
               .WithColumn("SecurityStamp").AsString(1000)
               .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
               .WithColumn("UserName").AsString(256).NotNullable();

            /* UserClaims */
            Create.Table("UserClaims")
               .WithColumn("Id").AsInt64().Identity().NotNullable()
               .WithColumn("ClaimType").AsString(1000)
               .WithColumn("ClaimValue").AsString(1000)
               .WithColumn("UserId").AsString(256).NotNullable();

            /* UserLogins */
            Create.Table("UserLogins")
               .WithColumn("Id").AsInt64().Identity().NotNullable()
               .WithColumn("LoginProvider").AsString(128)
               .WithColumn("ProviderKey").AsString(128)
               .WithColumn("ProviderDisplayName").AsString(1000)
               .WithColumn("UserId").AsString(256).NotNullable();

            /* UserTokens */
            Create.Table("UserTokens")
               .WithColumn("Id").AsInt64().Identity().NotNullable()
               .WithColumn("UserId").AsString(256).NotNullable()
               .WithColumn("LoginProvider").AsString(128).NotNullable()
               .WithColumn("Name").AsString(128).NotNullable()
               .WithColumn("Value").AsString(128).NotNullable();

            /* UserRoles */
            Create.Table("UserRoles")
               .WithColumn("Id").AsInt64().Identity().NotNullable()
               .WithColumn("UserId").AsString(256).NotNullable()
               .WithColumn("RoleId").AsString(128).NotNullable();

            /* ErrorLog */
            Create.Table("ErrorLog")
               .WithColumn("Id").AsInt64().Identity().NotNullable()
               .WithColumn("ErrorMsg").AsString(256).NotNullable()
               .WithColumn("ErrorFrom").AsString(128).NotNullable()
               .WithColumn("EntryOn").AsString(128);

            /* NLogs */
            Create.Table("NLogs")
               .WithColumn("Id").AsInt64().Identity().NotNullable()
               .WithColumn("When").AsString()
               .WithColumn("Message").AsString()
               .WithColumn("Level").AsString()
               .WithColumn("Exception").AsString()
               .WithColumn("Trace").AsString()
               .WithColumn("Logger").AsString();

            //Execute.EmbeddedScript("Database.Migrations.201607150000_Initial_StoreProcedure.sql");
            Execute.Sql(@"Create proc AddUser(    
						@Id int, 
                        @UserId varchar(60),
						@SecurityStamp nvarchar(max)='' , 
						@PhoneNumberConfirmed bit,  
						@PhoneNumber nvarchar(15)='', 
						@PasswordHash nvarchar(max) ,    
						@NormalizedUserName nvarchar(256) ,   
						@NormalizedEmail nvarchar (256) ,  
						@LockoutEnd datetimeoffset(7) = '',  
						@LockoutEnabled bit,    
						@EmailConfirmed bit , 
						@Email nvarchar(256) ,    
						@ConcurrencyStamp nvarchar(max) ,    
						@AccessFailedCount int   ,
						@TwoFactorEnabled bit,    
						@UserName nvarchar(256),
						@Role varchar(30)
					)    
AS
BEGIN
	Begin Try
		Begin Tran
			
			insert into Users (UserId,	UserName,     NormalizedUserName,     Email,     NormalizedEmail,     EmailConfirmed,     PasswordHash,     SecurityStamp,     ConcurrencyStamp,    
								PhoneNumber,     PhoneNumberConfirmed,     TwoFactorEnabled,     LockoutEnd,     LockoutEnabled,     AccessFailedCount)    
						values(@UserId, @UserName,@NormalizedUserName,     @Email,     @NormalizedEmail,     @EmailConfirmed,     @PasswordHash,  ISNULL(@SecurityStamp,''),     @ConcurrencyStamp,    
								 ISNULL(@PhoneNumber,''),     @PhoneNumberConfirmed,     @TwoFactorEnabled,ISNULL(@LockoutEnd,GetDate()), @LockoutEnabled,     @AccessFailedCount)    
			Select @Id = SCOPE_IDENTITY()
			Declare @RoleId int
			Select @RoleId = Id from ApplicationRole(nolock) where [Name]=@Role
			insert into UserRoles(UserId,RoleId) values(@Id,@RoleId)
		Commit Tran
	End Try
	Begin Catch
		Rollback Tran
		insert into ErrorLog(ErrorMsg,ErrorFrom,EntryOn) values(ERROR_MESSAGE(),'AddUser',GETDATE())
	End Catch
 end");
            Execute.Sql(@"CREATE proc proc_SaveNLog @msg varchar(max),@level varchar(max),@exception varchar(max),
                                                    @trace varchar(max),@logger varchar(max)  
                          As  
                          Begin
                          	INSERT INTO [NLogs]([When],[Message],[Level],Exception,Trace,Logger) VALUES (getutcdate(),@msg,@level,@exception,@trace,@logger)  
                          End  ");
            Execute.Sql(@"CREATE proc proc_getUserRole      
@Id bigint=0,      
@Email varchar(120)='',  
@mobileNo varchar(12)=''  
as      
begin      
if(@Id=0 and @Email='')      
begin      
select * from UserRoles  where 1=2      
return       
end      
if(@Id=0 and @Email<>'')      
begin      
select  @Id=ID from Users where NormalizedUserName=@Email      
end      
      
select RoleId from UserRoles where UserID=@Id      
end");
        }
    }
}
