using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence.Contracts.Interfaces;

namespace Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    private static readonly SetLastDayBookingInterceptor _setLastDayBookingInterceptor = new();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(_setLastDayBookingInterceptor);

    public DbSet<Booking> Bookings => Set<Booking>();
    
    public DbSet<Rental> Rentals => Set<Rental>();
}

public class SetLastDayBookingInterceptor : SaveChangesInterceptor 
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {

        var bookingBeforeSave = eventData.Context.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added && x.Entity is Booking)
            .Select(x => x.Entity).FirstOrDefault() as Booking;
        
        if (bookingBeforeSave != null)
        {
            bookingBeforeSave.LastDay = bookingBeforeSave.Start.AddDays(bookingBeforeSave.Nights);
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    // public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    // {
    //     if (instance is Booking bookingRetrive)
    //     {
    //         bookingRetrive.LastDay = bookingRetrive.Start.AddDays(bookingRetrive.Nights);
    //     }
    //     return instance;
    // }
}