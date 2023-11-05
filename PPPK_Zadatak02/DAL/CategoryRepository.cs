using PPPK_Zadatak02.Models;
using PPPK_Zadatak02.Utils;
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
    public class CategoryRepository : ICategoryRepository
    {
        private static readonly string cs = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        public void AddCategory(Category category)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Category.CategoryName), category.CategoryName);

            var id = new SqlParameter(nameof(Category.CategoryID), System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(id);
            cmd.ExecuteNonQuery();
            category.CategoryID = (int)id.Value;
        }

        public void DeleteCategory(Category category)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Category.CategoryID), category.CategoryID);
            cmd.ExecuteNonQuery();
        }

        public IList<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                categories.Add(new Category
                {
                    CategoryID = (int)dr[nameof(Category.CategoryID)],
                    CategoryName = dr[nameof(Category.CategoryName)].ToString()
                });
            }

            return categories;
        }

        public Category GetByIdCategory(int id)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Category.CategoryID), id);
            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Category
                {
                    CategoryID = (int)dr[nameof(Category.CategoryID)],
                    CategoryName = dr[nameof(Category.CategoryName)].ToString()
                };
            }
            throw new ArgumentException("Wrong id");
        }

        public void UpdateCategory(Category category)
        {
            using SqlConnection con = new(cs);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = MethodBase.GetCurrentMethod()?.Name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(nameof(Category.CategoryName), category.CategoryName);
            cmd.Parameters.AddWithValue(nameof(Category.CategoryID), category.CategoryID);
            cmd.ExecuteNonQuery();
        }

        public IList<Category> GetCategoriesForProduct(int productId)
        {
            IList<Category> categories = new List<Category>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetCategoriesForProduct", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Category category = new Category
                            {
                                CategoryID = dr.GetInt32(dr.GetOrdinal("CategoryID")),
                                CategoryName = dr.GetString(dr.GetOrdinal("CategoryName"))
                            };
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }

    }
}