using System;

namespace BoschCodeChallenge
{
    public class Part
    {
        public PartType Type { get; private set; }
        public Guid SerialNumber { get; private set; }
        public string MachineId { get; private set; }
        public DateTime ProductionCompletionTime { get; set; } = DateTime.MinValue;

        public Part(string machineId, PartType type, Guid serialNumber)
        {
            MachineId = machineId;
            Type = type;
            SerialNumber = serialNumber;
        }
    }
}
