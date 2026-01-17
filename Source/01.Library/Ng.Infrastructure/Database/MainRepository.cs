using Microsoft.Extensions.Options;
using Ng.Infrastructure.Database.Base;

namespace Ng.Infrastructure.Database;

public class MainRepository : RepositoryBase
{
    public override DatabaseTypes Database => DatabaseTypes.Main;
    public MainRepository(IOptions<DatabaseConfiguration> options)
        : base(options)
    {
    }
}
