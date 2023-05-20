using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Trainers.Queries;
public record class GetAllTrainersQuery : IRequest<Result<List<Trainer>>>;

public class GetAllTrainersQueryHandler : IRequestHandler<GetAllTrainersQuery, Result<List<Trainer>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTrainersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Trainer>>> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
    {
        var trainers = await _context.Trainers.ToListAsync();

        return await Result<List<Trainer>>.SuccessAsync(trainers);
    }
}
