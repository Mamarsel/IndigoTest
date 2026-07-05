using Microsoft.EntityFrameworkCore;

namespace IndigoTest.Aggregators.Persistence;

internal interface IOnModelCreatingVisitor
{
    void Apply(ModelBuilder builder);
}
