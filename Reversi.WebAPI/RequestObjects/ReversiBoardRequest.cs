using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiWebAPI.RequestObjects
{
    public class ReversiBoardRequest
    {
        public IEnumerable<ReversiBoardSpaceRequest> Spaces { get; set; }
    }
}
