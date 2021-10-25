using WebStore.Domain.ViewModel;

namespace WebStore.Domain.ViewModel
{
    public class DetailsViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderViewModel Order { get; set; } = new();
    }

}
