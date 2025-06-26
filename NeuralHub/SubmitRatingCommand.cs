using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Hubs
{
    public record SubmitRatingCommand(

        [property: Required]
        Guid SessionId,

        [property: Range(0, 5)]
        int Stability,

        [property: Range(0, 5)]
        int Efficiency,

        [property: Range(0, 5)]
        int Cohension) : IRequest<Guid>;

}
