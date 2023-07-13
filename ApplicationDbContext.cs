using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XploreIN.Models;

namespace XploreIN
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<RuralDestination> ruralDestinations { get; set; }

        public DbSet<DestinationMedia> destinationMedias { get; set; }

        public DbSet<UserItineraries> UserItineraries { get; set; }

        public DbSet<UserPost> UserPosts { get; set; }

        public DbSet<PostMedia> postMedias { get; set; }

        public DbSet<UserFavorites> UserFavorites { get; set; }
    }

}








