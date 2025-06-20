using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Core.Interfaces;
using Pagapoco.Infrastructure.Data;
using System;

namespace Pagapoco.Application.Services;

public class PublicationService : IPublicationService
{
    private readonly AppDbContext _context;

    public PublicationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Publication?> GetByIdAsync(Guid id)
        => await _context.Publications.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Publication>> SearchAsync(string? city, string? category)
    {
        var query = _context.Publications.AsQueryable();

        if (!string.IsNullOrEmpty(city))
            query = query.Where(p => p.City == city);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Type == category);

        return await query.Where(p => !p.IsPaused).ToListAsync();
    }

    public async Task CreateAsync(Publication publication)
    {
        _context.Publications.Add(publication);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Publication publication)
    {
        _context.Publications.Update(publication);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var pub = await _context.Publications.FindAsync(id);
        if (pub != null)
        {
            _context.Publications.Remove(pub);
            await _context.SaveChangesAsync();
        }
    }

    public async Task PauseAsync(Guid id)
    {
        var pub = await _context.Publications.FindAsync(id);
        if (pub != null)
        {
            pub.IsPaused = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task SetPauseStateAsync(Guid id, bool isPaused)
    {
        var pub = await _context.Publications.FindAsync(id);
        if (pub != null)
        {
            pub.IsPaused = isPaused;
            await _context.SaveChangesAsync();
        }
    }
}
