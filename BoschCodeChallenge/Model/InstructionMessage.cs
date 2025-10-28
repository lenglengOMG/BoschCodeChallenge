using System;
using System.Collections.Generic;

namespace BoschCodeChallenge
{
    public class InstructionMessage
    {
        public string MachineId { get; set; }
        public InstructionType Instruction { get; set; }
        public List<Part> PartInfos { get; set; }
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
