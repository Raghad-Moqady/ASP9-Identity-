using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity1.Data
{
    //يوجد في الايدنتتي جداول جاهزة وبتعمل (IdentityDbContext)
    //Dbset<> جاهزة 
   //لذلك نقوم بعكس هذه الجداول الضمنية الغير ظاهرة على الداتا بيس مباشرة دون الحاجة حتى لانشاء جداول بشكل يدوي بالداتابيس*مبدئيا
   //بعدها سيظهر لدينا 3جداول بالداتا بيس 
   //تساعدني بموضوع الصلاحيات وتسجيل الدخول وهذه الامور 
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options){ }
        
    }
}
