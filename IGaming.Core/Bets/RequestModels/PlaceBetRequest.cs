using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.Bets.RequestModels
{
    public record PlaceBetRequest(decimal Amount, string Details);

}
