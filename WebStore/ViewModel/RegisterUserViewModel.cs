using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModel
{
    public class RegisterUserViewModel
    {
        [Display(Name = "Имя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя является обязательным"), MaxLength(256)]
        public string UserName { get; set; }

        [Display(Name = "Почта")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Почта является обязательной"), MaxLength(256)]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль является обязательным"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
