using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Hubs
{
    public record SubmitRatingCommand(Guid SessionId, int Stability, int Efficiency, int Cohension) : IRequest<Guid>
    {

    }
}
