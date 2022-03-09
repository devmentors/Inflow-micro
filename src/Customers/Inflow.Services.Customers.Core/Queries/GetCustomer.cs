using System;
using Convey.CQRS.Queries;
using Inflow.Services.Customers.Core.DTO;

namespace Inflow.Services.Customers.Core.Queries;

public class GetCustomer : IQuery<CustomerDetailsDto>
{
    public Guid CustomerId { get; set; }
}