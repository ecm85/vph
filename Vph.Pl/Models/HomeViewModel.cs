using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Vph.Pl.Models
{
    public class HomeViewModel
    {
        public bool IsAuthenticated { get; set; }

        public string AccessToken { get; set; }
    }
}
