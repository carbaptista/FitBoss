using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Receptionists.Queries;
public record GetAllReceptionistsQuery : IRequest<Result<List<Receptionist>>>;

public class GetAllReceptionistsQueryHandler : IRequestHandler<GetAllReceptionistsQuery, Result<List<Receptionist>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllReceptionistsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Receptionist>>> Handle(GetAllReceptionistsQuery request, CancellationToken cancellationToken)
    {
        var receptionists = await _context.Receptionists.ToListAsync();

        return await Result<List<Receptionist>>.SuccessAsync(receptionists);
    }
}
