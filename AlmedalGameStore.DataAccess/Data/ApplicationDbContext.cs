using AlmedalGameStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlmedalGameStore.DataAccess
{

    //1.ladda ner Microsoft.AspNetCore.Identity.EntityFrameworkCore för att kunna använda nedan,
    // //2.ApplicationDbContext : IdentityDbContext för att kunna göra scaffoled identity
    //3.Högerklicka AlemdalGameStoreWeb och adda "New Scaffolded Item" Tryck accept sen overrida alla 
    // i  "Data context class" filer och välj ApplicationDbContext så att vi kan arbeta med vår db för
    // att skapa använda osv
    //Vi error försök att skapa det igen, det kan skapas en builder i program.cs med useSql
   // ta bort den eftersom vi redan har en! försök igen,
    public class ApplicationDbContext : IdentityDbContext
    {
        // I Konstruktorn får vi options som vi passar till vår base class (db context)
        //För alla models man skapar till sin db så behöver man skapa en db_set i applicationDbCOntext
        //Hur? public DbSet<ModelName>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //Genres blir namnet på vårt table
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet <Cart> Carts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
