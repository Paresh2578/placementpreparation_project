using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AdminUserModel> adminUsers { get;set; }
        public DbSet<BranchModel> branches { get;set; }
        public DbSet<CourseTypeModel> courseTypes { get;set; }
        public DbSet<DifficultyLevelModel> difficultyLevels { get;set; }

    }
}
