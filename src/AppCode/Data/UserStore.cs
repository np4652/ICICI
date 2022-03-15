
using ICICI.AppCode.DAL;
using ICICI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace ICICI.AppCode.Data
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IQueryableUserStore<ApplicationUser>, IUserLockoutStore<ApplicationUser>
    {
        // private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IDapperRepository _dapperRepository;
        private readonly ILogger<UserStore> _logger;
        public UserStore(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, IDapperRepository dapperRepository, ILogger<UserStore> logger)
        {
            //_context = context;
            _roleManager = roleManager;
            _dapperRepository = dapperRepository;
            _logger = logger;
        }
        public IQueryable<ApplicationUser> AllUsers()
        {
            var res = _dapperRepository.GetAllAsync<ApplicationUser>("select * from Users where ID<>1", null, commandType: CommandType.Text).Result;
            return res.AsQueryable();
        }
        //public IQueryable<ApplicationUser> Users => _context.Users();
        public IQueryable<ApplicationUser> Users => AllUsers();

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            int roleid = _roleManager.FindByNameAsync(roleName).Result.Id;
            await _dapperRepository.ExecuteAsync("insert into UserRoles values('" + user.Id + "','" + roleid + "')", null, commandType: CommandType.Text);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _dapperRepository.ExecuteAsync("AddUser", user, commandType: CommandType.StoredProcedure);
                if (res > 0)
                {
                    return IdentityResult.Success;
                }
                else if (res < 0)
                {
                    var v = new IdentityError()
                    {
                        Code = "-1",
                        Description = res == -1 ? "Mobile Or EmailID Allready Exists." : res == -2 ? "Email Allready Exists." : "Some Thin Is Wrong Try Again Later."
                    };
                    var resp = IdentityResult.Failed(v);
                    return resp;
                }
                else
                {
                    return IdentityResult.Failed();
                }
            }
            catch(Exception ex)
            {
                return IdentityResult.Failed();
            }
            
            //return Task.FromResult(_context.AddUser(user));
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var sqlQuery = @"select * from Users where NormalizedEmail='" + normalizedEmail + "'";
            var user = await _dapperRepository.GetAsync<ApplicationUser>(sqlQuery, null, commandType: CommandType.Text);
            return user;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _dapperRepository.GetAsync<ApplicationUser>("select * from Users where Id='" + userId + "'", null, commandType: CommandType.Text);
            return user;
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _dapperRepository.GetAsync<ApplicationUser>("select * from Users where NormalizedUserName='" + normalizedUserName + "'", null, commandType: CommandType.Text);
            return user;
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedEmail);
        }

        public async Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var normalizedUserName = await _dapperRepository.GetAsync<string>("select NormalizedUserName from Users where Email='" + user.Email + "'", null, commandType: CommandType.Text);
            return normalizedUserName;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                var userRoles = await _dapperRepository.GetAllAsync<string>("proc_getUserRole", new { user.Id, user.Email, mobileNo = user.PhoneNumber }, commandType: CommandType.StoredProcedure);
                return userRoles.ToList();
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user != null && user?.Id != null && user?.Id.ToString() != "0")
                return await Task.FromResult(user.Id.ToString());
            else
            {
                string sqlQuery = @"select Id from Users(nolock) where Email=Email or PhoneNumber=@mobileNo";
                return await _dapperRepository.GetAsync<string>(sqlQuery, new { user.Email, mobileNo = user.PhoneNumber }, commandType: CommandType.Text);
            }
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            bool result = false;
            int roleid = _roleManager.FindByNameAsync(roleName).Result.Id;
            string sqlQuery = @"IF (select Count(1) from UserRoles(nolock) where RoleId=@roleid and UserId=@Id)>0 select 1 else select 0";
            result = await _dapperRepository.GetAsync<bool>(sqlQuery, new { roleid, user.Id }, commandType: CommandType.Text);
            return result;
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);

        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);

        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);

        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var result = await _dapperRepository.ExecuteAsync("UpdateUser", user, commandType: CommandType.StoredProcedure);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        //Note : if GetLockoutEnabledAsync will false GetLockoutEndDateAsync will not hit;
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnd);
        }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
           return await _dapperRepository.ExecuteAsync("update [dbo].[Users] set AccessFailedCount=AccessFailedCount + 1 where Id=@Id", new { user.Id }, commandType: CommandType.Text);
        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _dapperRepository.ExecuteAsync("update [dbo].[Users] set AccessFailedCount = 0  where Id = @Id", new { user.Id }, commandType: CommandType.Text);

        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {

            await _dapperRepository.ExecuteAsync("update [dbo].[Users] set lockoutEnd=@lockoutEnd where Id=@Id", new { user.Id, lockoutEnd }, commandType: CommandType.Text);
        }
    }
}
