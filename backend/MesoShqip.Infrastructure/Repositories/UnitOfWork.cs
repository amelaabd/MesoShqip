using MesoShqip.Domain.Interfaces;
using MesoShqip.Infrastructure.Data;

namespace MesoShqip.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);

    public void Dispose()
        => _context.Dispose();
}