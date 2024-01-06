namespace vogue_decor.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(DBContext dbContext)
        {
            dbContext.Database.EnsureCreated();
        }
    }
}
