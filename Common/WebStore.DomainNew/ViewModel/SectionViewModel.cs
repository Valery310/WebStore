using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.ViewModel
{
    public class SectionViewModel: INamedEntity, IOrderedEntity
    {
        public List<SectionViewModel> ChildSections { get; set; }
        public SectionViewModel ParentSection { get; set; }

        public SectionViewModel() 
        {
            ChildSections = new List<SectionViewModel>();
        }

        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        
    }
}
