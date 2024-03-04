namespace music_store.src.data;
using Microsoft.EntityFrameworkCore;
using music_store.src.user;
public class DbConnectionContext : DbContext
{
  public DbConnectionContext(DbContextOptions<DbConnectionContext> options) : base(options) { }

  public DbSet<UserEntity> User { get; set; }

}

