using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionsViewComponent:ViewComponent
    {
        private readonly IProductData _productData;

        public SectionsViewComponent(IProductData productData)
        {
            _productData = productData;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var sections = await GetSections().ConfigureAwait(false);
            return View(sections);
        }

        private async Task<List<SectionViewModel>> GetSections() 
        {
            var categories = await _productData.GetSections().ConfigureAwait(false);
            var parentCategories = categories.Where(p => !p.ParentId.HasValue).ToArray();
            var parentSections = new List<SectionViewModel>();
            foreach (var parentCategory in parentCategories)
            {
                parentSections.Add(new SectionViewModel()
                {
                    Id = parentCategory.Id,
                    Name = parentCategory.Name,
                    Order = parentCategory.Order,
                    ParentSection = null
                }) ;
            }

            foreach (var sectionViewModel in parentSections)
            {
                var childCategories = categories.Where(c=> c.ParentId.Equals(sectionViewModel.Id));
                foreach (var childCategory in childCategories)
                {
                    sectionViewModel.ChildSections.Add(new SectionViewModel() 
                    {
                        Id = childCategory.Id,
                        Name = childCategory.Name,
                        Order = childCategory.Order,
                        ParentSection = sectionViewModel
                    });
                }

                sectionViewModel.ChildSections = sectionViewModel.ChildSections.OrderBy(c => c.Order).ToList();
            }
            parentSections = parentSections.OrderBy(c => c.Order).ToList();
            return parentSections;
        }
    }
}
