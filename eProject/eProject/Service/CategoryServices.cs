using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eProject.Models;
using eProject.Repository;
using Microsoft.EntityFrameworkCore;

namespace eProject.Service
{
    public class CategoryServices : ICategoryServices
    {
        private readonly Data.DatabaseContext context;
        public CategoryServices(Data.DatabaseContext context)
        {
            this.context = context;
        }


        public void CreateCategory(Category newCategory)
        {
            Category category = context.Categories.SingleOrDefault(x => x.CategoryId.Equals(newCategory.CategoryId));
            if (category == null)
            {
                context.Categories.Add(newCategory);
                context.SaveChanges();
            }
            else
            {
                //do nothing
            }
        }

        public Category FindOne(int id)
        {
            Category category = context.Categories.SingleOrDefault(x => x.CategoryId.Equals(id));
            if (category != null)
            {
                return category;
            }
            else
            {
                return null;
            }
        }

        public List<Category> GetCategories()
        {
            var res = context.Categories.ToList();
            if (res != null)
            {
                return res;
            }
            else
            {
                //ViewBag.msg = "Nothing on the list";
                return null;
            }
        }

        public void RemoveCategory(int id)
        {
            Category category = context.Categories.SingleOrDefault(x => x.CategoryId.Equals(id));
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
            else
            {
                //ViewBag.msg = "Cant find that's ID";
            }
        }

        public void UpdateCategory(Category editCategory)
        {
            Category category = context.Categories.SingleOrDefault(x => x.CategoryId.Equals(editCategory.CategoryId));
            if (category != null)
            {
                category.CategoryName = editCategory.CategoryName;

                context.SaveChanges();
            }
            else
            {
                //do nothing
            }
        }



    }
}
