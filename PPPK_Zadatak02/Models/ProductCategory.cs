using PPPK_Zadatak02.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PPPK_Zadatak02.Models
{
    public class ProductCategory
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }
        public BitmapImage Image
        {
            get => ImageUtils.ByteArrayToBitmapImage(Picture!);

        }
        public List<Category>? Categories { get; set; } = new List<Category>();


    }
}
