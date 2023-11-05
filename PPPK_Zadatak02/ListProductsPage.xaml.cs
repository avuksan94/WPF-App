using PPPK_Zadatak02.Models;
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
    /// Interaction logic for ListProductsPage.xaml
    /// </summary>
    public partial class ListProductsPage : ProductFramedPage
    {
        public ListProductsPage(ProductViewModel productViewModel) : base(productViewModel)
        {
            InitializeComponent();
            lvProducts.ItemsSource = productViewModel.Products;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e) => Frame.Navigate(new EditProductPage(ProductViewModel) { Frame = Frame });

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lvProducts.SelectedItem != null)
            {
                Frame.Navigate(new EditProductPage(ProductViewModel, lvProducts.SelectedItem as ProductCategory) { Frame = Frame });
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvProducts.SelectedItem != null)
            {
                ProductViewModel.DeleteProductCommand.Execute(lvProducts.SelectedItem);
            }
        }
    }
}
