using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoschCodeChallenge
{
    public class StatusMessage
    {
        public string MachineId { get; set; }
        public StateType State { get; set; }
        public long Timestamp { get; set; }
    }
}
