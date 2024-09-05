using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;
using AudioStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AudioStore.DataAccess.Repository
{
    public class CategoryRepository :Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public async Task<List<Category>> GetAllSuperCategories()
        {
            return await _db.Categories.Where(x => x.SuperCategoryID == null).ToListAsync();
        }
        public async Task<List<CategoryVM>> GetCategoriesVM()
        {
            var subCategories = await (from c in _db.Categories
                                       join sc in _db.Categories on c.SuperCategoryID equals sc.CategoryID
                                       select new CategoryVM
                                       {
                                           CategoryID = c.CategoryID,
                                           CategoryName = c.Name,
                                           SuperCategoryName = sc.Name
                                       }).ToListAsync();
            var categories = await (from c in _db.Categories
                                    where c.SuperCategoryID == null
                                    select new CategoryVM
                                    {
                                        CategoryID = c.CategoryID,
                                        CategoryName = c.Name,
                                        SuperCategoryName = "None"
                                    }).ToListAsync();
            List<CategoryVM> result = [.. categories, .. subCategories];
            return result;
        }

        public Task<List<Category>> GetAllSubCategories(int id)
        {
            var subCategories = _db.Categories.Where(x => x.SuperCategoryID == id);
            return subCategories.ToListAsync();
        }
        public async Task<Category> UpdateCategory(Category category)
        {
            _db.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }
    }
}
