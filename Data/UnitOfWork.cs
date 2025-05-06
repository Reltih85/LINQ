using Lab8_Bernaola_Pacheco.Repositories;
using Lab8_Bernaola_Pacheco.Repositories.Interfaces;

namespace Lab8_Bernaola_Pacheco.Data;

public interface IUnitOfWork : IDisposable
{
    IRepository Repository { get; } // Cambiado a un único repositorio
    int SaveChanges();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Repository = new Repository(_context); // Instancia única
    }

    public IRepository Repository { get; }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}