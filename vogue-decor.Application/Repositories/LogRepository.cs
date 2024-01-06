using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain;
using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.Repositories
{
    /// <inheritdoc/>
    public class LogRepository : ILogRepository
    {
        private readonly IDBContext _dbContext;

        public LogRepository(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(User user, string message, LogType type)
        {
            var log = new Log
            {
                Id = Guid.NewGuid(),
                User = user,
                Message = message,
                Type = type,
                Date = DateTime.UtcNow
            };

            await _dbContext.Logs.AddAsync(log);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
