using AudioStore.DataAccess;
using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AudioStore.Services
{
    public class CategoryServices : ICategoryService
    {
        public ApplicationDbContext Context { get; }
        public CategoryServices(ApplicationDbContext context)
        {
            Context = context;
        }
        public async Task<Category> CreateCategory(Category category)
        {
            Context.Add(category);
            await Context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategory(int? id)
        {
            var category = await Context.Categories.FindAsync(id);
            if (category != null)
            {
                Context.Categories.Remove(category);
                await Context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Category with ID {id} doesn't exist!");
            }
        }

        public async Task<List<Category>> GetAllSuperCategories()
        {
            return await Context.Categories.Where(x => x.SuperCategoryID == null).ToListAsync();
        }
        public async Task<List<CategoryVM>> GetCategoriesVM()
        {
            var subCategories = await (from c in Context.Categories
                                join sc in Context.Categories on c.SuperCategoryID equals sc.CategoryID
                                select new CategoryVM
                                {
                                    CategoryID = c.CategoryID,
                                    CategoryName = c.Name,
                                    SuperCategoryName = sc.Name
                                }).ToListAsync();
            var categories = await (from c in Context.Categories
                                    where c.SuperCategoryID == null 
                                    select new CategoryVM{ 
                                        CategoryID=c.CategoryID,
                                        CategoryName = c.Name,
                                        SuperCategoryName = "None"
                                    }).ToListAsync();
            List<CategoryVM> result = new List<CategoryVM>();
            result.AddRange(categories);
            result.AddRange(subCategories);
            return result;
        }


        public Task<Category> GetCategoryById(int? id)
        {
            return Context.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            Context.Update(category);
            await Context.SaveChangesAsync();
            return category;
        }
    }
}
