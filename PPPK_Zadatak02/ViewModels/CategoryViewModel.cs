using PPPK_Zadatak02.DAL;
using PPPK_Zadatak02.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPPK_Zadatak02.ViewModels
{
    public class CategoryViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }

        public CategoryViewModel()
        {
            Categories = new ObservableCollection<Category>(
                RepositoryFactory.GetCategoryRepository().GetAllCategories());
            Categories.CollectionChanged += Categories_CollectionChanged;
        }

        private void Categories_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RepositoryFactory.GetCategoryRepository().AddCategory(Categories[e.NewStartingIndex]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RepositoryFactory.GetCategoryRepository().DeleteCategory(e.OldItems!.OfType<Category>().ToList()[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RepositoryFactory.GetCategoryRepository().UpdateCategory(e.NewItems!.OfType<Category>().ToList()[0]);
                    break;
                default:
                    break;
            }
        }

    }
}
