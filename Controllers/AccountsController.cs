using Identity1.Data;
using Identity1.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity1.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //هون بدي اخزن المعلومات بالداتا بيس بس المشكلة انه لازم احول من فيو لدومين مودل لان الداتا بيس بس بتعرف الدومين وانا رفعتلها الدومين فقط
        //الحل انه نغلف الفيو بأوبجيكت من الدومين 
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //تحويل من فيو موديل الى دومين موديل 
            //هيك صار بالامكان رفع اليوزر على الداتا بيس لانها صارت من نوع IdentityUser
            IdentityUser user = new IdentityUser()
            {
                //الاسم المخزن بالداتا بيس = new data>> model.
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email,

            };
            //بضيف الداتا (اليوزر) على الداتا بيس +بشفر الباسوورد  +بسمح لاكتر من شخص انهم يسجلو بنفس الوقت 
            //هيك رح يشتغل بشكل متوازي
            var result = await userManager.CreateAsync(user, model.Password);
            //اذا تمتت عملية الاضافة بنجاح ,القيمة اللي بسطر 42 رح تكون ترو 
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //اذا حاول المستخدم يسجل 5 مرات مثلا بقفل الحساب لان حطيت اخر اشي ترو بسطر 59
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
