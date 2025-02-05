using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AdminUserModel> AdminUsers { get;set; }
        public DbSet<BranchModel> Branches { get;set; }
        public DbSet<CourseTypeModel> CourseTypes { get;set; }
        public DbSet<DifficultyLevelModel> DifficultyLevels { get;set; }

        public DbSet<CourseModel> Courses { get;set; }

        public DbSet<TopicModel> Topics { get;set; }
        public DbSet<SubTopicModel> SubTopics { get;set; }
        public DbSet<QuestionModel> Questions { get;set; }
        public DbSet<McqModel> Mcqs { get;set; }

        public DbSet<FeedbackModel> Feedbacks { get;set; }
    }
}
