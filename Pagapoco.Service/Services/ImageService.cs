using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Services.Interfaces;
using Pagapoco.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pagapoco.Application.Services;

public class ImageService : IImageService
{
    private readonly AppDbContext _context;

    public ImageService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Image> GetByPublicationId(Guid publicationId)
        => _context.Images.Where(img => img.PublicationId == publicationId).ToList();

    /*
    // Versión anterior asíncrona
    // public async Task<IEnumerable<Image>> GetByPublicationIdAsync(Guid publicationId)
    //     => await _context.Images.Where(img => img.PublicationId == publicationId).ToListAsync();
    */

    public void AddImage(Guid publicationId, Image image)
    {
        image.PublicationId = publicationId;
        _context.Images.Add(image);
        _context.SaveChanges();
    }

    /*
    // Versión anterior asíncrona
    // public async Task AddImageAsync(Guid publicationId, Image image)
    // {
    //     image.PublicationId = publicationId;
    //     _context.Images.Add(image);
    //     await _context.SaveChangesAsync();
    // }
    */

    public void Remove(Guid imageId)
    {
        var img = _context.Images.Find(imageId);
        if (img != null)
        {
            _context.Images.Remove(img);
            _context.SaveChanges();
        }
    }

    /*
    // Versión anterior asíncrona
    // public async Task RemoveAsync(Guid imageId)
    // {
    //     var img = await _context.Images.FindAsync(imageId);
    //     if (img != null)
    //     {
    //         _context.Images.Remove(img);
    //         await _context.SaveChangesAsync();
    //     }
    // }
    */

    public void AddImagesToPublication(Guid publicationId, Guid userId, List<string> imageUrls)
    {
        throw new NotImplementedException();
    }

    public void DeleteImage(Guid imageId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public void UpdateImage(Guid imageId, Guid userId, string? newUrl = null, string? newAltText = null, int? newOrder = null)
    {
        throw new NotImplementedException();
    }

    public List<Image> GetPublicationImages(Guid publicationId)
    {
        throw new NotImplementedException();
    }
}
