using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity1.Models.ViewModel
{
    public class EditUserRoleViewModel
    {
        public string Id { get; set; }
        //عبارة عن داتا تايب =select option
        //فهمي رسالة للHTML
        //اعمل select option
        public IEnumerable<SelectListItem> RolesList { get; set; }
        //عشان احط فيها الجواب
        public string ?SelectedRole { get; set; }
    }
}
