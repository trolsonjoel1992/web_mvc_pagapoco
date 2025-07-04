using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Service.Interfaces;
using Pagapoco.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pagapoco.Application.Services;

public class PublicationService : IPublicationService
{
    private readonly AppDbContext _context;

    public PublicationService(AppDbContext context)
    {
        _context = context;
    }

    public Publication? GetById(Guid id)
        => _context.Publications.Include(p => p.Images).FirstOrDefault(p => p.Id == id);

    /*
    // Versión anterior asíncrona
    // public async Task<Publication?> GetByIdAsync(Guid id)
    //     => await _context.Publications.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
    */

    public IEnumerable<Publication> Search(string? city, string? category)
    {
        var query = _context.Publications.AsQueryable();

        if (!string.IsNullOrEmpty(city))
            query = query.Where(p => p.City == city);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Type == category);

        return query.Where(p => !p.IsPaused).ToList();
    }

    /*
    // Versión anterior asíncrona
    // public async Task<IEnumerable<Publication>> SearchAsync(string? city, string? category)
    // {
    //     var query = _context.Publications.AsQueryable();
    //     if (!string.IsNullOrEmpty(city))
    //         query = query.Where(p => p.City == city);
    //     if (!string.IsNullOrEmpty(category))
    //         query = query.Where(p => p.Type == category);
    //     return await query.Where(p => !p.IsPaused).ToListAsync();
    // }
    */

    public void Create(Publication publication)
    {
        _context.Publications.Add(publication);
        _context.SaveChanges();
    }

    /*
    // Versión anterior asíncrona
    // public async Task CreateAsync(Publication publication)
    // {
    //     _context.Publications.Add(publication);
    //     await _context.SaveChangesAsync();
    // }
    */

    public void Update(Publication publication)
    {
        _context.Publications.Update(publication);
        _context.SaveChanges();
    }

    /*
    // Versión anterior asíncrona
    // public async Task UpdateAsync(Publication publication)
    // {
    //     _context.Publications.Update(publication);
    //     await _context.SaveChangesAsync();
    // }
    */

    public void Delete(Guid id)
    {
        var pub = _context.Publications.Find(id);
        if (pub != null)
        {
            _context.Publications.Remove(pub);
            _context.SaveChanges();
        }
    }

    /*
    // Versión anterior asíncrona
    // public async Task DeleteAsync(Guid id)
    // {
    //     var pub = await _context.Publications.FindAsync(id);
    //     if (pub != null)
    //     {
    //         _context.Publications.Remove(pub);
    //         await _context.SaveChangesAsync();
    //     }
    // }
    */

    public void Pause(Guid id)
    {
        var pub = _context.Publications.Find(id);
        if (pub != null)
        {
            pub.IsPaused = true;
            _context.SaveChanges();
        }
    }

    /*
    // Versión anterior asíncrona
    // public async Task PauseAsync(Guid id)
    // {
    //     var pub = await _context.Publications.FindAsync(id);
    //     if (pub != null)
    //     {
    //         pub.IsPaused = true;
    //         await _context.SaveChangesAsync();
    //     }
    // }
    */

    public void SetPauseState(Guid id, bool isPaused)
    {
        var pub = _context.Publications.Find(id);
        if (pub != null)
        {
            pub.IsPaused = isPaused;
            _context.SaveChanges();
        }
    }

    /*
    // Versión anterior asíncrona
    // public async Task SetPauseStateAsync(Guid id, bool isPaused)
    // {
    //     var pub = await _context.Publications.FindAsync(id);
    //     if (pub != null)
    //     {
    //         pub.IsPaused = isPaused;
    //         await _context.SaveChangesAsync();
    //     }
    // }
    */

    public (List<Publication> Publications, int TotalCount) GetPublicationsPaginated(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public List<Publication> SearchPublications(string? city, string? publicationType)
    {
        throw new NotImplementedException();
    }

    public Publication CreatePublication(Publication publication, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Publication? GetPublicationById(Guid id, bool includeImages = true)
    {
        throw new NotImplementedException();
    }

    public void UpdatePublication(Guid publicationId, Guid userId, string title, string description, decimal price)
    {
        throw new NotImplementedException();
    }

    public void DeletePublication(Guid publicationId, Guid userId, bool softDelete = true)
    {
        throw new NotImplementedException();
    }

    public List<Publication> GetUserPublications(Guid userId)
    {
        throw new NotImplementedException();
    }

    public List<Publication> FilterPublications(string type, Dictionary<string, object> filters)
    {
        var baseQuery = _context.Publications.AsQueryable()
            .Where(p => p.Type == type && !p.IsPaused);

        switch (type)
        {
            case "Vehicle":
                var vehicleQuery = baseQuery.OfType<Vehicle>();
                if (filters.TryGetValue("Brand", out var brand))
                    vehicleQuery = vehicleQuery.Where(v => v.Brand == (string)brand);
                if (filters.TryGetValue("Model", out var model))
                    vehicleQuery = vehicleQuery.Where(v => v.Model == (string)model);
                if (filters.TryGetValue("Year", out var year))
                    vehicleQuery = vehicleQuery.Where(v => v.Year == (int)year);
                if (filters.TryGetValue("Color", out var color))
                    vehicleQuery = vehicleQuery.Where(v => v.Color == (string)color);
                if (filters.TryGetValue("FuelType", out var fuelType))
                    vehicleQuery = vehicleQuery.Where(v => v.FuelType == (string)fuelType);
                if (filters.TryGetValue("Transmission", out var transmission))
                    vehicleQuery = vehicleQuery.Where(v => v.Transmission == (string)transmission);
                if (filters.TryGetValue("EngineDisplacement", out var engineDisplacement))
                    vehicleQuery = vehicleQuery.Where(v => v.EngineDisplacement == (string)engineDisplacement);
                if (filters.TryGetValue("KilometersDriven", out var kilometersDriven))
                    vehicleQuery = vehicleQuery.Where(v => v.KilometersDriven == (int)kilometersDriven);
                if (filters.TryGetValue("Version", out var version))
                    vehicleQuery = vehicleQuery.Where(v => v.Version == (string)version);
                if (filters.TryGetValue("Doors", out var doors))
                    vehicleQuery = vehicleQuery.Where(v => v.Doors == (int)doors);
                return vehicleQuery.ToList<Publication>();

            case "Bike":
                var bikeQuery = baseQuery.OfType<Bike>();
                if (filters.TryGetValue("Brand", out var bikeBrand))
                    bikeQuery = bikeQuery.Where(b => b.Brand == (string)bikeBrand);
                if (filters.TryGetValue("Model", out var bikeModel))
                    bikeQuery = bikeQuery.Where(b => b.Model == (string)bikeModel);
                if (filters.TryGetValue("Year", out var bikeYear))
                    bikeQuery = bikeQuery.Where(b => b.Year == (int)bikeYear);
                if (filters.TryGetValue("Color", out var bikeColor))
                    bikeQuery = bikeQuery.Where(b => b.Color == (string)bikeColor);
                if (filters.TryGetValue("FuelType", out var bikeFuelType))
                    bikeQuery = bikeQuery.Where(b => b.FuelType == (string)bikeFuelType);
                if (filters.TryGetValue("Transmission", out var bikeTransmission))
                    bikeQuery = bikeQuery.Where(b => b.Transmission == (string)bikeTransmission);
                if (filters.TryGetValue("EngineDisplacement", out var bikeEngineDisplacement))
                    bikeQuery = bikeQuery.Where(b => b.EngineDisplacement == (string)bikeEngineDisplacement);
                if (filters.TryGetValue("KilometersDriven", out var bikeKilometersDriven))
                    bikeQuery = bikeQuery.Where(b => b.KilometersDriven == (int)bikeKilometersDriven);
                if (filters.TryGetValue("EnginePower", out var enginePower))
                    bikeQuery = bikeQuery.Where(b => b.EnginePower == (int)enginePower);
                if (filters.TryGetValue("WheelSize", out var wheelSize))
                    bikeQuery = bikeQuery.Where(b => b.WheelSize == (string)wheelSize);
                return bikeQuery.ToList<Publication>();

            case "Part":
                var partQuery = baseQuery.OfType<Part>();
                if (filters.TryGetValue("Brand", out var partBrand))
                    partQuery = partQuery.Where(p => p.Brand == (string)partBrand);
                if (filters.TryGetValue("Model", out var partModel))
                    partQuery = partQuery.Where(p => p.Model == (string)partModel);
                if (filters.TryGetValue("Color", out var partColor))
                    partQuery = partQuery.Where(p => p.Color == (string)partColor);
                if (filters.TryGetValue("Condition", out var condition))
                    partQuery = partQuery.Where(p => p.Condition == (string)condition);
                if (filters.TryGetValue("Compatibility", out var compatibility))
                    partQuery = partQuery.Where(p => p.Compatibility == (string)compatibility);
                return partQuery.ToList<Publication>();

            default:
                return new List<Publication>();
        }
    }
}
