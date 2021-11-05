using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;

namespace WebStore.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для работы с товарами
    /// </summary>
    public interface IProductData
    {
        /// <summary>
        /// Список секций
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Section>> GetSections();
        /// <summary>
        /// Список брендов
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Brand>> GetBrands();
        /// <summary>
        /// Список товаров
        /// </summary>
        /// <param name="filter">Фильтр товаров</param>
        /// <returns></returns>
        //Task<IEnumerable<Product>> GetProducts(ProductFilter filter);
        ///// <summary>
        ///// Продукт
        ///// </summary>
        ///// <param name="id">Идентификатор</param>
        ///// <returns>Сущность Product, если нашел, иначе null</returns>
        Task<Product> GetProductById(int id);
        /// <summary>
        /// Редактирование
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        Task<int> UpdateAsync(Product product);
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
        /// <summary>
        /// Категория
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Section, если нашел, иначе null</returns>
        Task<Section> GetSectionsById(int id);
        /// <summary>
        /// Бренд
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Brand, если нашел, иначе null</returns>
        Task<Brand> GetBrandsById(int id);
        /// <summary>
        /// Список товаров с постраничным разбиением
        /// </summary>
        /// <param name="filter">Фильтр товаров</param>
        /// <returns></returns>
        Task<PageProduct> GetProducts(ProductFilter filter);
        /// <summary>
        /// Создать продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        Task<SaveResult> CreateProduct(Product product);
        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        Task<SaveResult> UpdateProduct(Product product);
        /// <summary>
        /// Удалить продукт
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        Task<SaveResult> DeleteProduct(int productId);
    }

}
