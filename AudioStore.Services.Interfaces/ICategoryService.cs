using AudioStore.Models;
using AudioStore.Models.ViewModels;

namespace AudioStore.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<Category> CreateCategory(Category category);
        public Task<List<Category>> GetAllSuperCategories();
        public Task<Category> GetCategoryById(int? id);
        public Task<Category> UpdateCategory(Category category);
        public Task DeleteCategory(int? id);
        public Task<List<CategoryVM>> GetCategoriesVM();
        public Task<List<Category>> GetAllSubCategories(int id);


    }
}
