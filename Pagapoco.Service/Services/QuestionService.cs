using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Core.Interfaces;
using System;

namespace Pagapoco.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly AppDbContext _context;

    public QuestionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Question?> GetByIdAsync(Guid id)
        => await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);

    public async Task<IEnumerable<Question>> GetByPublicationIdAsync(Guid publicationId)
        => await _context.Questions.Where(q => q.PublicationId == publicationId).ToListAsync();

    public async Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId)
        => await _context.Questions.Where(q => q.UserId == userId).ToListAsync();

    public async Task CreateAsync(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
    }
}
