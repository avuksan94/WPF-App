using PPPK_Zadatak02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPPK_Zadatak02.DAL
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Category GetByIdCategory(int id);
        IList<Category> GetAllCategories();
        IList<Category> GetCategoriesForProduct(int productId);
    }
}
