using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPPK_Zadatak02.DAL
{
    public static class RepositoryFactory
    {
        private static readonly Lazy<IProductRepository> productRepository= new(() => new ProductRepository());
        private static readonly Lazy<ICategoryRepository> categoryRepository= new(() => new CategoryRepository());
        public static IProductRepository GetProductRepository() => productRepository.Value;
        public static ICategoryRepository GetCategoryRepository() => categoryRepository.Value;

    }
}
