using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Trainers.Queries;
public record GetTrainersByNameQuery(string Name) : IRequest<Result<List<Trainer>>>;

public class GetTrainersByNameQueryHandler : IRequestHandler<GetTrainersByNameQuery, Result<List<Trainer>>>
{
    private readonly IApplicationDbContext _context;

    public GetTrainersByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Trainer>>> Handle(GetTrainersByNameQuery request, CancellationToken cancellationToken)
    {
        var trainers = await _context.Trainers
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToListAsync();

        if (trainers.Count == 0)
            return await Result<List<Trainer>>.FailureAsync($"No trainers with name containing {request.Name} found");

        return await Result<List<Trainer>>.SuccessAsync(trainers);
    }
}