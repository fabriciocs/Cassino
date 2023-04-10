using Cassino.Core.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Cassino.Infra.Context;

public sealed class ApplicationDbContext : BaseApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUser authenticatedUser) : base(options, authenticatedUser)
    {
    }
}