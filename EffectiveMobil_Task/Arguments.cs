using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectiveMobil_Task
{
    public class Arguments
    {
        public string LogFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public string StartAddress { get; set; }
        public int? Mask { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
