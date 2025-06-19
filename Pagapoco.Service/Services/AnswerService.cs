using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using Pagapoco.Core.Interfaces;
using System;

namespace Pagapoco.Application.Services;

public class AnswerService : IAnswerService
{
    private readonly AppDbContext _context;

    public AnswerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId)
        => await _context.Answers.Where(a => a.QuestionId == questionId).ToListAsync();

    public async Task CreateAsync(Answer answer)
    {
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
    }
}
