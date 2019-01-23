using System.Collections.Generic;

namespace Vph.Pl.Models
{
    public class ActivitiesViewModel
    {
        public bool IsAuthenticated { get; set; }
        public IEnumerable<ActivityViewModel> Activities { get; set; }
    }
}
