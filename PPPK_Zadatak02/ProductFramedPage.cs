using PPPK_Zadatak02.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PPPK_Zadatak02
{
    public class ProductFramedPage : Page
    {
        public ProductViewModel ProductViewModel { get; set; }
        public Frame? Frame { get; set; }

        public ProductFramedPage(ProductViewModel productViewModel)
        {
            ProductViewModel = productViewModel;
        }
    }
}
