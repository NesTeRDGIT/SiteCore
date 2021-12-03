using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SiteCore.Data
{
  

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationClaim, ApplicationUserRole,IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("SITE"); // Use uppercase!
            modelBuilder.Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<ApplicationUser>().HasMany(e => e.UserRoles).WithOne(role => role.User).HasForeignKey(ur => ur.UserId).IsRequired();
            modelBuilder.Entity<ApplicationUser>().HasMany(e => e.Claims).WithOne(x=>x.User).HasForeignKey(uc => uc.UserId).IsRequired();
            modelBuilder.Entity<ApplicationRole>().HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
       


            modelBuilder.ToUpperCaseTables();
            modelBuilder.ToUpperCaseColumns();

        }
    }
    public class ApplicationUser : IdentityUser
    {
        public PasswordHashVersion HashVersion { get; set; } = PasswordHashVersion.Core;
        public string CODE_MO { get; set; }
        public string CODE_SMO { get; set; }
        public string FAM { get; set; } = "";
        public string IM { get; set; } = "";
        public string OT { get; set; } = "";
        public bool WithSing { get; set; }
        public string FIO => $"{FAM} {IM} {OT}";
        public string FamAndInitial => $"{FAM}  {(!string.IsNullOrEmpty(IM) ? $"{IM}." : "")}{(!string.IsNullOrEmpty(OT) ? $"{OT}." : "")}";
        public bool IsPers => (FAM != null && IM != null && OT != null && PhoneNumber != null);
        [ForeignKey("CODE_MO")]
        public virtual CODE_MO CODE_MO_NAME { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationClaim> Claims { get; set; }
    }
    [Table("F003")]
    public class CODE_MO
    {
        [Key]
        public string MCOD { get; set; }
        public string NAM_MOP { get; set; }
        public string NAM_MOK { get; set; }
        public DateTime? D_END { get; set; }

    }

  
    public class ApplicationRole : IdentityRole
    {
        public string COMENT { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }




    public enum PasswordHashVersion
    {
        Mvc5 = 0,
        Core = 1
    }
    //Обновление паролей
    public class Mvc5MvcPasswordHasher : PasswordHasher<ApplicationUser> 
    {

       
        public override PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
           
            // if it's the new algorithm version, delegate the call to parent class
            if (user.HashVersion == PasswordHashVersion.Core)
                return base.VerifyHashedPassword(user, hashedPassword, providedPassword);

            byte[] buffer4;
            if (hashedPassword == null)
            {
                return PasswordVerificationResult.Failed;
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }
            var src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return PasswordVerificationResult.Failed;
            }
            var dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            var buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (var bytes = new Rfc2898DeriveBytes(providedPassword, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            if (AreHashesEqual(buffer3, buffer4))
            {
                /*user.HashVersion = PasswordHashVersion.Core;
                return PasswordVerificationResult.SuccessRehashNeeded;*/

                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }

        private bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            var _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (var i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
    }
    public static class ApplicationDbContextEx
    {
        
        public static ApplicationClaim PasswordClaim(this ApplicationUser user)
        {
            user.Claims ??= new List<ApplicationClaim>();
            var c = user.Claims.FirstOrDefault(x => x.ClaimType == "Password");
            if (c == null)
            {
                c = new ApplicationClaim { ClaimType = "Password" };
                user.Claims.Add(c);
            }
            return c;
        }
        public static string CODE_MO(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "CODE_MO")?.Value;
        }
        /*
        public static string CODE_MO (this ClaimsPrincipal user)
        {
           return  user.Claims.FirstOrDefault(x=>x.Type == "CODE_MO")?.Value;
        }

        public static string CODE_SMO(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "CODE_SMO")?.Value;
        }

        public static string ID(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "ID")?.Value;
        }

        public static string NAME(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        }*/
    }

    public class UserInfo
    {
        public string CODE_MO { get; set; }
        public string CODE_MO_NAME { get; set; }
        public string CODE_SMO { get; set; }
        public string USER_ID { get; set; }
        public string NAME { get; set; }
        public bool WithSing { get; set; }
        public string Password { get; set; }
    }


    public class UserInfoHelper
    {
        ApplicationUserManager userManager;
        public UserInfoHelper(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }
        public  UserInfo GetInfo(string name)
        {
            var user = userManager.FindByName(name);
            return new UserInfo()
            {
                CODE_MO = user.CODE_MO,
                CODE_MO_NAME = user.CODE_MO_NAME?.NAM_MOK,
                CODE_SMO = user.CODE_SMO,
                USER_ID = user.Id,
                NAME = user.UserName,
                WithSing = user.WithSing,
                Password = user.PasswordClaim()?.ClaimValue
            };
        }

    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

        public override Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return Users.Include(c => c.UserRoles).Include(x=>x.Claims).Include(x=>x.CODE_MO_NAME).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Users.Include(c => c.UserRoles).Include(x => x.Claims).Include(x => x.CODE_MO_NAME).FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
        }
        public ApplicationUser FindByName(string userName)
        {
            return Users.Include(c => c.UserRoles).Include(x => x.Claims).Include(x => x.CODE_MO_NAME).FirstOrDefault(u => u.NormalizedUserName == userName.ToUpper());
        }



    }
    public class ApplicationClaimsIdentityFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public ApplicationClaimsIdentityFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager,roleManager, optionsAccessor)
        { }
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            if (principal.Identity is ClaimsIdentity ci)
            {
                ci.AddClaim(new Claim("CODE_MO", user.CODE_MO));
                ci.AddClaim(new Claim("CODE_SMO", user.CODE_SMO));
                ci.AddClaim(new Claim("ID", user.Id));
                foreach (var role in user.UserRoles)
                {
                    ci.AddClaim(new Claim("ROLE_ID", role.RoleId));
                }
            }
            

            return principal;
        }
    }
}
