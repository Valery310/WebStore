using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "Имя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя является обязательным"), MaxLength(256)]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль является обязательным"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запоммнить?")]
        [Required]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
