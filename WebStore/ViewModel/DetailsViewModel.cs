using WebStore.ViewModel;

namespace WebStore.ViewModel
{
    public class DetailsViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderViewModel Order { get; set; } = new();
    }

}
