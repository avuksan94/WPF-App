using PPPK_Zadatak02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPPK_Zadatak02.DAL
{
    public interface IProductRepository
    {
        int AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Product GetByIdProduct(int id);
        IList<Product> GetAllProducts();
        IList<Product> GetProductsByCategoryId(int categoryId);
        void LinkProductToCategory(int productId, int categoryId);
        void UnlinkProductFromCategory(int productId, int categoryId);
    }
}
