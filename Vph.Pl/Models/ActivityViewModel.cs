﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StravaSharp;

namespace Vph.Pl.Models
{
    public class ActivityViewModel
    {
        private ActivitySummary _summary;

        public ActivityViewModel(ActivitySummary summary)
        {
            _summary = summary;
        }

        public string Name => _summary.Name;

        public string Start => _summary.StartDate.ToString("yyyy.MM.dd hh:mm");

        public string Distance
        {
            get
            {
                if (_summary.Distance > 1000)
                    return $"{(_summary.Distance / 1000.0f).ToString("F2")} km";
                else
                    return $"{_summary.Distance} m";
            }
        }
    }
}
