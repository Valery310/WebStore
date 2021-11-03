using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Areas.Admin.Models
{
    public class ProductViewModel : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        [Required, Display(Name = "Название")]
        public string Name { get; set; }
        [Required, Display(Name = "Порядок")]
        public int Order { get; set; }
        [Required, Display(Name = "Изображение")]
        public string ImageUrl { get; set; }
        [Required, Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Required, Display(Name = "Категория")]
        public Section Section { get; set; }
        [Display(Name = "Бренд")]
        public Brand Brand { get; set; }

        public SelectList Sections { get; set; }
        public SelectList Brands { get; set; }
    }
}
