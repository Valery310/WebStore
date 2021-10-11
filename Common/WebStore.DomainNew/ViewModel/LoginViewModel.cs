using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "Имя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя является обязательным"), MaxLength(256)]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль является обязательным"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        [Required]
        public bool RememberMe { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}
