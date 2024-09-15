using Identity1.Data;
using Identity1.Models;
using Identity1.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity1.Controllers
{
    [Authorize(Roles ="Admin, Super Admin")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                                                    RoleManager<IdentityRole> roleManager) 
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //هون بدي اخزن المعلومات بالداتا بيس بس المشكلة انه لازم احول من فيو لدومين مودل لان الداتا بيس بس بتعرف الدومين وانا رفعتلها الدومين فقط
        //الحل انه نغلف الفيو بأوبجيكت من الدومين 
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //تحويل من فيو موديل الى دومين موديل 
            //هيك صار بالامكان رفع اليوزر على الداتا بيس لانها صارت من نوع IdentityUser
            ApplicationUser user = new ApplicationUser()
            {
                //الاسم المخزن بالداتا بيس = new data>> model.
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email,
                City=model.City
            };
            //بضيف الداتا (اليوزر) على الداتا بيس +بشفر الباسوورد  +بسمح لاكتر من شخص انهم يسجلو بنفس الوقت 
            //هيك رح يشتغل بشكل متوازي
            var result = await userManager.CreateAsync(user, model.Password);
            //اذا تمتت عملية الاضافة بنجاح ,القيمة اللي بسطر 42 رح تكون ترو 
            if (result.Succeeded)
            {
                //add this role"User" to user
                await userManager.AddToRoleAsync(user,"User");
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //اذا حاول المستخدم يسجل 5 مرات مثلا بقفل الحساب لان حطيت اخر اشي ترو بسطر 59
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction(nameof(GetUsers));
            }
            return View(model);
        }

        public IActionResult GetUsers()
        { 
            var users =userManager.Users.ToList();
            var usersViewModel = users.Select(user => new UserViewModel
            {
                Id=user.Id,
                Email=user.Email,
                Phone=user.PhoneNumber,
                UserName=user.Email,
                City=user.City,
                Roles=userManager.GetRolesAsync(user).Result
            }).ToList();
            
            return View(usersViewModel);
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            IdentityRole role = new IdentityRole()
            {
                Name = model.RoleName
            };
            var result=await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(GetRoles));
            }
         
            return View(model);
        }
        public IActionResult GetRoles()
        {
            var roles = roleManager.Roles.ToList();
            var rolesView = roles.Select(role => new RoleViewModel
            {
                RoleName=role.Name
            }).ToList();

            return View(rolesView);
        }

        public IActionResult EditUserRole(string id)
        {
            var viewModel = new EditUserRoleViewModel
            {
                Id = id,
                RolesList = roleManager.Roles.Select(
                    role => new SelectListItem
                    {
                        Value = role.Id,
                        Text = role.Name
                    }
                    ).ToList(),
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserRole(EditUserRoleViewModel model)//Id SelectedRole
        {
            var user =await userManager.FindByIdAsync(model.Id);
            var userRoles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, userRoles);

            var newRole = await roleManager.FindByIdAsync(model.SelectedRole);
            await userManager.AddToRoleAsync(user, newRole.Name);

            return RedirectToAction(nameof(GetUsers));
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
