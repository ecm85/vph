using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Vph.Pl.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }

        public bool IsAuthenticated { get; private set; }

        public string AccessToken { get; set; }
        public IList<ActivityViewModel> Activities { get; } = new ObservableCollection<ActivityViewModel>();
    }
}
