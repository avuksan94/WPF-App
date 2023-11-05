using PPPK_Zadatak02.Command;
using PPPK_Zadatak02.DAL;
using PPPK_Zadatak02.Models;
using PPPK_Zadatak02.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PPPK_Zadatak02.ViewModels
{
    //https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/relaycommand
    public class ProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductCategory> Products { get; set; }

        private ProductCategory _selectedProductCategory;

        public ProductCategory SelectedProductCategory
        {
            get => _selectedProductCategory;
            set
            {
                if (_selectedProductCategory != value)
                {
                    _selectedProductCategory = value;
                    OnPropertyChanged(nameof(SelectedProductCategory));
                }
            }
        }

        // ICommand implementations
        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get; private set; }
        public ICommand UpdateProductCommand { get; private set; }

        public ProductViewModel()
        {
            if (Products == null)
            {
                Products = new ObservableCollection<ProductCategory>();
            }

            initProductCategory();

            if (_selectedProductCategory == null)
            {
                _selectedProductCategory = new();
            }

            AddProductCommand = new RelayCommand(ExecuteAddProduct);
            DeleteProductCommand = new RelayCommand(ExecuteDeleteProduct, CanExecuteProductSelected);
            UpdateProductCommand = new RelayCommand(ExecuteUpdateProduct, CanExecuteProductSelected);

        }

        private void initProductCategory()
        {
            var getAllProducts = RepositoryFactory.GetProductRepository().GetAllProducts();

            foreach (var product in getAllProducts) {
                var categoriesForProduct = RepositoryFactory.GetCategoryRepository().GetCategoriesForProduct(product.ProductID);
                Products.Add(
                        new ProductCategory
                        {
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            Description = product.Description,
                            Picture = product.Picture,
                            Categories = categoriesForProduct.ToList()
                        }
                    );
            }
        }


        private void ExecuteAddProduct(object parameter)
        {
            try
            {
                var productToAdd = parameter as ProductCategory;


                if (productToAdd != null && productToAdd.Categories.Any())
                {
                    int newProductId = RepositoryFactory.GetProductRepository().AddProduct(new Product
                    {
                        ProductName = productToAdd.ProductName,
                        Description = productToAdd.Description,
                        Picture = productToAdd.Picture
                    });

                    foreach (var category in productToAdd.Categories)
                    {
                        RepositoryFactory.GetProductRepository().LinkProductToCategory(newProductId, category.CategoryID);
                    }

                    productToAdd.ProductID = newProductId;
                    Products.Add(productToAdd);
                }
            }
            catch (Exception ex)
            {
                MessageUtils.ShowError("Someting went wrong! : " + ex);
            }
        }

        private void ExecuteDeleteProduct(object parameter)
        {
            try
            {
                var productToDelete = parameter as ProductCategory;

                if (productToDelete != null)
                {
                    Product product = new()
                    {
                        ProductID = productToDelete.ProductID,
                        ProductName = productToDelete.ProductName,
                        Description = productToDelete.Description,
                        Picture = productToDelete.Picture
                    };

                    foreach (var category in productToDelete.Categories)
                    {
                        RepositoryFactory.GetProductRepository().UnlinkProductFromCategory(productToDelete.ProductID,category.CategoryID);
                    }

                    RepositoryFactory.GetProductRepository().DeleteProduct(product);
                    Products.Remove(productToDelete);
                }
            }
            catch (Exception ex)
            {
                MessageUtils.ShowError("Someting went wrong! : " + ex);
            }
        }

        private void ExecuteUpdateProduct(object parameter)
        {
            try
            {
                var productToUpdate = parameter as ProductCategory;

                if (productToUpdate != null && productToUpdate.ProductID > 0)
                {
                    Product product = new()
                    {
                        ProductID = productToUpdate.ProductID,
                        ProductName = productToUpdate.ProductName,
                        Description = productToUpdate.Description,
                        Picture = productToUpdate.Picture
                    };

                    RepositoryFactory.GetProductRepository().UpdateProduct(product);

                    var currentCategories = RepositoryFactory.GetCategoryRepository().GetCategoriesForProduct(product.ProductID);

                    foreach (var existingCategory in currentCategories)
                    {
                        RepositoryFactory.GetProductRepository().UnlinkProductFromCategory(product.ProductID, existingCategory.CategoryID);
                    }

                    foreach (var newCategory in productToUpdate.Categories)
                    {
                        RepositoryFactory.GetProductRepository().LinkProductToCategory(product.ProductID, newCategory.CategoryID);
                    }

                    // Refresh the observable collection
                    RefreshProductInCollection(productToUpdate);
                }
            }
            catch (Exception ex)
            {
                MessageUtils.ShowError("Something went wrong! " + ex.Message);
            }
        }

        //Forsing IObservable to refresh(Category needs to refresh)
        private void RefreshProductInCollection(ProductCategory productToUpdate)
        {
            var productInCollection = Products.FirstOrDefault(p => p.ProductID == productToUpdate.ProductID);
            if (productInCollection != null)
            {
               
                var index = Products.IndexOf(productInCollection);
                Products[index] = new ProductCategory
                {
                    ProductID = productToUpdate.ProductID,
                    ProductName = productToUpdate.ProductName,
                    Description = productToUpdate.Description,
                    Picture = productToUpdate.Picture,
                    Categories = new List<Category>(productToUpdate.Categories)
                };
                OnPropertyChanged(nameof(Products));
            }
        }

        private bool CanExecuteProductSelected(object parameter)
        {
            return SelectedProductCategory != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
