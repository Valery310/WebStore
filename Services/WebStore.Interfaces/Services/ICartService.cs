using System.Threading.Tasks;
using WebStore.Domain.ViewModel;

namespace WebStore.Interfaces.Services
{
    public interface ICartService
    {
        void DecrementFromCart(int id);
        void RemoveFromCart(int id);
        void RemoveAll();
        void AddToCart(int id);
        Task<CartViewModel> TransformCart();
        int GetItemsCoumtFromCart();
    }
}
