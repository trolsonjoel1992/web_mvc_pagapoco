using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Services.Interfaces;
using Pagapoco.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pagapoco.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public User? GetById(Guid id)
        => _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);

    /*
    // Versión anterior asíncrona
    // public async Task<User?> GetByIdAsync(Guid id)
    //     => await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    */

    public User? GetByEmail(string email)
        => _context.Users.FirstOrDefault(u => u.Email == email && !u.IsDeleted);

    /*
    // Versión anterior asíncrona
    // public async Task<User?> GetByEmailAsync(string email)
    //     => await _context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    */

    public List<Publication> GetUserPublications(Guid userId)
        => _context.Publications.Where(p => p.UserId == userId).ToList();

    /*
    // Versión anterior asíncrona
    // public async Task<IEnumerable<Publication>> GetUserPublicationsAsync(Guid userId)
    //     => await _context.Publications.Where(p => p.UserId == userId).ToListAsync();
    */

    public void DeleteUser(Guid userId, bool softDelete = true)
    {
        var user = _context.Users.Find(userId);
        if (user == null) return;

        if (softDelete)
            user.IsDeleted = true;
        else
            _context.Users.Remove(user);

        _context.SaveChanges();
    }

    /*
    // Versión anterior asíncrona
    // public async Task DeleteByIdAsync(Guid userId, bool softDelete = true)
    // {
    //     var user = await _context.Users.FindAsync(userId);
    //     if (user == null) return;
    //     if (softDelete)
    //         user.IsDeleted = true;
    //     else
    //         _context.Users.Remove(user);
    //     await _context.SaveChangesAsync();
    // }
    */

    public void DeleteUserByEmail(string email, bool softDelete = true)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null) return;

        if (softDelete)
            user.IsDeleted = true;
        else
            _context.Users.Remove(user);

        _context.SaveChanges();
    }

    /*
    // Versión anterior asíncrona
    // public async Task DeleteByEmailAsync(string email, bool softDelete = true)
    // {
    //     var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    //     if (user == null) return;
    //     if (softDelete)
    //         user.IsDeleted = true;
    //     else
    //         _context.Users.Remove(user);
    //     await _context.SaveChangesAsync();
    // }
    */

    public void UpdateUser(Guid userId, string? name, string? phone, string? city)
    {
        var existing = _context.Users.Find(userId);
        if (existing == null || existing.IsDeleted) return;

        existing.Name = name;
        existing.Phone = phone;
        existing.City = city;

        _context.SaveChanges();
    }

    /*
    // Versión anterior asíncrona
    // public async Task UpdateAsync(User user)
    // {
    //     var existing = await _context.Users.FindAsync(user.Id);
    //     if (existing == null || existing.IsDeleted) return;
    //     existing.Name = user.Name;
    //     existing.Phone = user.Phone;
    //     existing.City = user.City;
    //     await _context.SaveChangesAsync();
    // }
    */

    public void UpdateUserByEmail(string email, string name, string phone, string city)
    {
        var existing = _context.Users.FirstOrDefault(u => u.Email == email && !u.IsDeleted);
        if (existing == null) return;

        existing.Name = name;
        existing.Phone = phone;
        existing.City = city;

        _context.SaveChanges();
    }

    public User Register(string email, string password, string? name, string? phone, string? city)
    {
        // Generar salt seguro
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);

        // Generar hash seguro
        var hash = HashPassword(password, saltBytes);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = name,
            Phone = phone,
            City = city,
            PasswordHash = Convert.ToBase64String(hash),
            PasswordSalt = salt,
            IsDeleted = false
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public User? Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email && !u.IsDeleted);
        if (user == null)
            return null;

        var saltBytes = Convert.FromBase64String(user.PasswordSalt);
        var hash = HashPassword(password, saltBytes);

        if (user.PasswordHash == Convert.ToBase64String(hash))
            return user;

        return null;
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        // PBKDF2 con HMACSHA256 y 100.000 iteraciones
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(32); // 256 bits
    }
}
