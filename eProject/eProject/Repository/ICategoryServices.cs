using eProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Repository
{
   public interface ICategoryServices
    {
        public List<Category> GetCategories();
        public Category FindOne(int id);
        public void CreateCategory(Category newCategory);
        public void UpdateCategory(Category editCategory);
        public void RemoveCategory(int id);

    }
}
