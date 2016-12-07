using System.Collections.Generic;
using System.Linq;
using Business.Connectors.Petition;

namespace Business.Handlers.Request
{
    /// <summary>
    /// Request from Controller
    /// </summary>
    public class ReadRequest : Request
    {
        public List<Filter> Filters { get; set; }

        public static explicit operator ReadBusinessPetition(ReadRequest request)
        {
            return new ReadBusinessPetition
            {
                FilterString = request.Filters?.OrderBy(x => x.PropertyName).Aggregate((x, y) => x + y).ExpressionString
            };
        }
    }
}