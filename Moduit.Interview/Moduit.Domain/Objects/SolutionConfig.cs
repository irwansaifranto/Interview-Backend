using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.Domain.Objects
{
    public class SolutionConfig
    {
        public ResourceProfile ModuitAPI { get; set; }
        public int MinSleepTimeOnRetryErrorCallAPI { get; set; } = 5000;
        public int MaxSleepTimeOnRetryErrorCallAPI { get; set; } = 18000;
        public int NumberOfRetryFailure { get; set; } = 5;
        public int TimeoutMinCallApi { get; set; } = 300000;
        public int TimeoutMaxCallApi { get; set; } = 600000;
    }
}
