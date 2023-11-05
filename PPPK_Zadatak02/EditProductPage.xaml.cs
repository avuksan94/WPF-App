using Microsoft.Win32;
using PPPK_Zadatak02.DAL;
using PPPK_Zadatak02.Models;
using PPPK_Zadatak02.Utils;
using PPPK_Zadatak02.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PPPK_Zadatak02
{
    /// <summary>
    /// Interaction logic for EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : ProductFramedPage
    {
        private const string FILTER = "All supported graphics|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Portable Network Graphic (*.png)|*.png";
        private readonly ProductCategory? _productCategory;
        public EditProductPage(ProductViewModel productViewModel,
            ProductCategory? productCategory = null) : base(productViewModel)
        {
            InitializeComponent();
            this.Frame = Frame;
            _productCategory = productCategory ?? new ProductCategory();
            LoadCategoriesForComboBox();
            DataContext = _productCategory;

            if (_productCategory.Categories.Any())
            {
                var selectedCategory = _productCategory.Categories.First();
                CbCategory.SelectedItem = CbCategory.Items
                    .OfType<Category>()
                    .FirstOrDefault(c => c.CategoryID == selectedCategory.CategoryID);
            }
        }

        private void LoadCategoriesForComboBox()
        {
            var allCategories = RepositoryFactory.GetCategoryRepository().GetAllCategories();
            CbCategory.ItemsSource = allCategories;
        }

        private bool FormValid()
        {
            bool valid = true;
            EditGrid.Children.OfType<TextBox>().ToList().ForEach(e =>
            {
                if (string.IsNullOrEmpty(e.Text.Trim()))
                {
                    e.Background = Brushes.LightCoral;
                    valid = false;
                }
                else
                {
                    e.Background = Brushes.White;
                }
            });
            if (Picture.Source == null)
            {
                PictureBorder.BorderBrush = Brushes.LightCoral;
                valid = false;
            }
            else
            {
                PictureBorder.BorderBrush = Brushes.WhiteSmoke;
            }
            return valid;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => Frame.NavigationService.GoBack();

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (FormValid())
            {
                if (_productCategory != null)
                {
                    _productCategory.ProductName = TbProductName.Text.Trim();
                    _productCategory.Description = TbDescription.Text.Trim();
                    _productCategory.Picture = ImageUtils.BitmapImageToByteArray(Picture.Source as BitmapImage);

                    _productCategory.Categories.Clear();
                    var selectedCategory = CbCategory.SelectedItem as Category;
                    if (selectedCategory != null)
                    {
                        _productCategory.Categories.Add(selectedCategory);
                    }

                   
                    if (_productCategory.ProductID == 0)
                    {
                        ProductViewModel.AddProductCommand.Execute(_productCategory);
                    }
                    else
                    {
                        ProductViewModel.UpdateProductCommand.Execute(_productCategory);
                    }
                }

                if (this.Frame != null && this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
                else
                {
                    MessageUtils.ShowError("Something went wrong, for some reason Frame is null!");
                }
            }
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = FILTER
            };
            if (openFileDialog.ShowDialog() == true)
            {
                Picture.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
