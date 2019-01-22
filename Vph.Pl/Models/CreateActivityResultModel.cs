using System;
using System.Collections;
using System.Collections.Generic;

namespace Vph.Pl.Models
{
    public class CreateActivityResultModel
    {
        public int Successes { get; set; }
        public int Failures { get; set; }
        public IList<Exception> Exceptions { get; } = new List<Exception>();
    }
}
