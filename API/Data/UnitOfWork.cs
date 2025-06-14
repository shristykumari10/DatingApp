using API.Interfaces;

namespace API.Data
{
    public class UnitOfWork(DataContext context, IUserRepository userRepository,
        IMessageRepository messageRepository, ILikesRespository likesRespository) : IUnitOfWork
    {
        public IUserRepository UserRepository => userRepository;

        public IMessageRepository MessageRepository => messageRepository;

        public ILikesRespository LikesRespository => likesRespository;

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
           return context.ChangeTracker.HasChanges();
        }
    }
}
