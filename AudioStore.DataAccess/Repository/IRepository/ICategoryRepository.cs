using AudioStore.Models;
using AudioStore.Models.ViewModels;

namespace AudioStore.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {

        public Task<List<Category>> GetAllSuperCategories();
        public Task<Category> UpdateCategory(Category category);
        public Task<List<CategoryVM>> GetCategoriesVM();
        public Task<List<Category>> GetAllSubCategories(int id);

    }
}
