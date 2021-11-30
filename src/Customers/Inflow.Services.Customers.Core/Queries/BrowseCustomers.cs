using Convey.CQRS.Queries;
using Inflow.Services.Customers.Core.DTO;

namespace Inflow.Services.Customers.Core.Queries;

public class BrowseCustomers : PagedQueryBase, IQuery<PagedResult<CustomerDto>>
{
    public string State { get; set; }
}