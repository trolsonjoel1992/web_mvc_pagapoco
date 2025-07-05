using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Infrastructure.Data;
using Pagapoco.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pagapoco.Application.Services
{
    public class PublicationService : IPublicationService
    {
        private readonly AppDbContext _context;

        public PublicationService(AppDbContext context)
        {
            _context = context;
        }

        public Publication CreatePublication(Publication publication, Guid userId)
        {
            publication.UserId = userId;
            publication.Id = Guid.NewGuid();
            _context.Publications.Add(publication);
            _context.SaveChanges();
            return publication;
        }

        public Publication? GetPublicationById(Guid id, bool includeImages = true)
        {
            var query = _context.Publications.AsQueryable();
            if (includeImages)
                query = query.Include(p => p.Images);
            return query.FirstOrDefault(p => p.Id == id);
        }

        public (List<Publication> publications, int total) GetPublicationsPaginated(int page, int pageSize)
        {
            var query = _context.Publications.Where(p => !p.IsPaused);
            int total = query.Count();
            var publications = query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return (publications, total);
        }

        public List<Publication> GetAllPublications()
        {
            return _context.Publications.Where(p => !p.IsPaused).ToList();
        }

        public List<Publication> SearchPublications(string? city, string? publicationType)
        {
            var query = _context.Publications.AsQueryable();
            if (!string.IsNullOrEmpty(city))
                query = query.Where(p => p.City == city);
            if (!string.IsNullOrEmpty(publicationType))
                query = query.Where(p => p.Type == publicationType);
            return query.Where(p => !p.IsPaused).ToList();
        }

        public void UpdatePublication(Guid publicationId, Guid userId, string title, string description, decimal price)
        {
            var pub = _context.Publications.FirstOrDefault(p => p.Id == publicationId && p.UserId == userId);
            if (pub == null) throw new InvalidOperationException("Publicación no encontrada o sin permisos.");
            pub.Title = title;
            pub.Description = description;
            pub.Price = price;
            _context.SaveChanges();
        }

        public void DeletePublication(Guid publicationId, Guid userId, bool softDelete = true)
        {
            var pub = _context.Publications.FirstOrDefault(p => p.Id == publicationId && p.UserId == userId);
            if (pub == null) throw new InvalidOperationException("Publicación no encontrada o sin permisos.");
            if (softDelete)
            {
                pub.IsPaused = true;
            }
            else
            {
                _context.Publications.Remove(pub);
            }
            _context.SaveChanges();
        }

        public List<Publication> GetUserPublications(Guid userId)
        {
            return _context.Publications.Where(p => p.UserId == userId && !p.IsPaused).ToList();
        }

        public List<Publication> FilterPublications(string type, Dictionary<string, object> filters)
        {
            var query = _context.Publications.AsQueryable().Where(p => p.Type == type && !p.IsPaused);

            foreach (var filter in filters)
            {
                switch (filter.Key)
                {
                    case "Brand":
                        query = query.Where(p => p.Brand == (string)filter.Value);
                        break;
                    case "Model":
                        query = query.Where(p => p.Model == (string)filter.Value);
                        break;
                    case "Color":
                        query = query.Where(p => p.Color == (string)filter.Value);
                        break;
                    case "Condition":
                        query = query.Where(p => p.Condition == (string)filter.Value);
                        break;
                    case "Compatibility":
                        query = query.Where(p => p.Compatibility == (string)filter.Value);
                        break;
                }
            }

            return query.ToList();
        }

        public void PausePublication(Guid publicationId)
        {
            var pub = _context.Publications.Find(publicationId);
            if (pub == null) throw new InvalidOperationException("Publicación no encontrada.");
            pub.IsPaused = true;
            _context.SaveChanges();
        }

        public void ActivatePublication(Guid publicationId)
        {
            var pub = _context.Publications.Find(publicationId);
            if (pub == null) throw new InvalidOperationException("Publicación no encontrada.");
            pub.IsPaused = false;
            _context.SaveChanges();
        }

        public void SetPauseState(Guid publicationId, bool isPaused)
        {
            var pub = _context.Publications.Find(publicationId);
            if (pub == null) throw new InvalidOperationException("Publicación no encontrada.");
            pub.IsPaused = isPaused;
            _context.SaveChanges();
        }
    }
}