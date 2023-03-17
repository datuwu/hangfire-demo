using NoteManagementAPI.Entities;

namespace NoteManagementAPI.DAL
{
    public class UnitOfWork : IDisposable
    {
        private NoteAppDbContext _context = new NoteAppDbContext();
        private IGenericRepository<Note> noteRepo;
        private IGenericRepository<User> userRepo;
        private IGenericRepository<Role> roleRepo;
        private IGenericRepository<LoginRecord> loginRecordRepo;

        public IGenericRepository<Note> NoteRepo
        {
            get
            {

                if (this.noteRepo == null)
                {
                    this.noteRepo = new GenericRepository<Note>(_context);
                }
                return noteRepo;
            }
        }

        public IGenericRepository<User> UserRepo
        {
            get
            {

                if (this.userRepo == null)
                {
                    this.userRepo = new GenericRepository<User>(_context);
                }
                return userRepo;
            }
        }

        public IGenericRepository<Role> RoleRepo
        {
            get
            {

                if (this.roleRepo == null)
                {
                    this.roleRepo = new GenericRepository<Role>(_context);
                }
                return roleRepo;
            }
        }
        
        public IGenericRepository<LoginRecord> LoginRecordRepo
        {
            get
            {

                if (this.loginRecordRepo == null)
                {
                    this.loginRecordRepo = new GenericRepository<LoginRecord>(_context);
                }
                return loginRecordRepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
