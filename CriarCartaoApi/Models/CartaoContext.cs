using Microsoft.EntityFrameworkCore;

namespace CriarCartaoApi.Models
{
    public class CartaoContext : DbContext
    {
        public CartaoContext(DbContextOptions<CartaoContext> options)
            : base(options)
        {
        }

        public DbSet<Cartao> Cartoes { get; set; }
        
    }
}