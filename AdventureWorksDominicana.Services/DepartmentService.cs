using System.Linq.Expressions;
using AdventureWorksDominicana.Data.Context;
using AdventureWorksDominicana.Data.Models;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
namespace AdventureWorksDominicana.Services;
public class DepartmentService(IDbContextFactory<Contexto> DbFactory) : IService<Department, short>
{
    public async Task<bool> Guardar(Department entidad)
    {
        if (!await Existe(entidad.DepartmentId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }
    private async Task<bool> Existe(short id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Departments.AnyAsync(d => d.DepartmentId == id);
    }
    private async Task<bool> Insertar(Department entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Departments.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }
    private async Task<bool> Modificar(Department entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<Department?> Buscar(short id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
    }
    public async Task<bool> Eliminar(short id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Departments.AsNoTracking().Where(d => d.DepartmentId == id).ExecuteDeleteAsync() > 0;
    }
    public async Task<List<Department>> GetList(Expression<Func<Department, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Departments.Where(criterio).AsNoTracking().ToListAsync();
    }
}