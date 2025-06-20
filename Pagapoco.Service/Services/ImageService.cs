using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Core.Interfaces;
using Pagapoco.Infrastructure.Data;
using System;

namespace Pagapoco.Application.Services;

public class ImageService : IImageService
{
    private readonly AppDbContext _context;

    public ImageService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Image>> GetByPublicationIdAsync(Guid publicationId)
        => await _context.Images.Where(img => img.PublicationId == publicationId).ToListAsync();

    public async Task AddImageAsync(Guid publicationId, Image image)
    {
        image.PublicationId = publicationId;
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid imageId)
    {
        var img = await _context.Images.FindAsync(imageId);
        if (img != null)
        {
            _context.Images.Remove(img);
            await _context.SaveChangesAsync();
        }
    }
}
