using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Core.Interfaces;
using Pagapoco.Infrastructure.Data;
using System;

namespace Pagapoco.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

    public async Task<IEnumerable<Publication>> GetUserPublicationsAsync(Guid userId)
        => await _context.Publications.Where(p => p.UserId == userId).ToListAsync();

    public async Task DeleteByIdAsync(Guid userId, bool softDelete = true)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;

        if (softDelete)
            user.IsDeleted = true;
        else
            _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByEmailAsync(string email, bool softDelete = true)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return;

        if (softDelete)
            user.IsDeleted = true;
        else
            _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var existing = await _context.Users.FindAsync(user.Id);
        if (existing == null || existing.IsDeleted) return;

        existing.Name = user.Name;
        existing.Phone = user.Phone;
        existing.City = user.City;
        // Email opcional si lo permitís

        await _context.SaveChangesAsync();
    }
}
