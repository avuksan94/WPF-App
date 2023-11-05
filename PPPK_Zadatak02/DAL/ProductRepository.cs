using PPPK_Zadatak02.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PPPK_Zadatak02.DAL
{
    public class ProductRepository : IProductRepository
    {
        private static readonly string cs = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        public int AddProduct(Product product)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Product.ProductName), product.ProductName);
            cmd.Parameters.AddWithValue(nameof(Product.Description), product.Description);
            if (product.Picture != null)
            {
                cmd.Parameters.Add(
                    new SqlParameter(nameof(Product.Picture), System.Data.SqlDbType.Binary, product.Picture.Length)
                    {
                        Value = product.Picture
                    });
            }
            var id = new SqlParameter(nameof(Product.ProductID), System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(id);
            cmd.ExecuteNonQuery();
            product.ProductID = (int)id.Value;

            return (int)id.Value;
        }

        public void DeleteProduct(Product product)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Product.ProductID), product.ProductID);

            cmd.ExecuteNonQuery();
        }

        public IList<Product> GetAllProducts()
        {
            IList<Product> list = new List<Product>();

            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(ReadProduct(dr));
            }

            return list;
        }

        public Product GetByIdProduct(int idProduct)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Product.ProductID), idProduct);
            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return ReadProduct(dr);
            }
            throw new ArgumentException("Wrong id");
        }

        private Product ReadProduct(SqlDataReader dr)
        => new ()
        {
            ProductID = (int)dr[nameof(Product.ProductID)],
            ProductName = dr[nameof(Product.ProductName)].ToString(),
            Description = dr[nameof(Product.Description)].ToString(),
            Picture = dr[nameof(Product.Picture)] as byte[]
        };

        public void UpdateProduct(Product product)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Product.ProductName), product.ProductName);
            cmd.Parameters.AddWithValue(nameof(Product.Description), product.Description);
            if (product.Picture != null)
            {
                cmd.Parameters.Add(
                    new SqlParameter(nameof(Product.Picture), System.Data.SqlDbType.Binary, product.Picture.Length)
                    {
                        Value = product.Picture
                    });
            }
            cmd.Parameters.AddWithValue(nameof(Product.ProductID), product.ProductID);

            cmd.ExecuteNonQuery();
        }

        public IList<Product> GetProductsByCategoryId(int categoryId)
        {
            IList<Product> list = new List<Product>();

            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(categoryId), categoryId);
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(ReadProduct(dr));
            }

            return list;
        }

        public void LinkProductToCategory(int productId, int categoryId)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(productId), productId);
            cmd.Parameters.AddWithValue(nameof(categoryId), categoryId);

            cmd.ExecuteNonQuery();
        }

        public void UnlinkProductFromCategory(int productId, int categoryId)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(productId), productId);
            cmd.Parameters.AddWithValue(nameof(categoryId), categoryId);

            cmd.ExecuteNonQuery();
        }

        private bool ProductExists(string productName)
        {
            var allProducts = GetAllProducts();

            if (allProducts.First(p => p.ProductName == productName.Trim()) != null)
            {
                return true;
            }
            return false;
        }

    }
}
