using System;
using System.Collections.Generic;
using System.Linq;

namespace BoschCodeChallenge
{
    internal class ProductionCellV1
    {
        public string CellId { get; private set; }
        public List<MachineV1> Machines { get; private set; }
        public List<ProductionCellV1> SubCells { get; private set; }
        public ProductionCellV1(string cellId, List<MachineV1> machines, List<ProductionCellV1> subCells)
        {
            CellId = cellId;
            Machines = machines;
            SubCells = subCells;
        }
    }

    internal class MachineV1
    {
        public string CellId { get; set; }
        public string MachineId { get; private set; }
        public List<PartV1> PartV1s { get; private set; }
        public List<PartType> SupportedParts { get; private set; }
        public StateType CurrentState { get; set; } = StateType.Idle;
        public MachineV1(string machineId, List<PartType> supportedPartTypes)
        {
            MachineId = machineId;
            PartV1s = new List<PartV1>();
            SupportedParts = supportedPartTypes;
        }

        public bool Start(IEnumerable<PartV1> parts)
        {
            if (parts.Any(p => !SupportedParts.Contains(p.Type)))
            {
                return false;
            }
            CurrentState = StateType.Running;
            return true;
        }

        public bool Stop()
        {
            CurrentState = StateType.Idle;
            return true;
        }

        public bool Maintain()
        {
            CurrentState = StateType.UnderMaintenance;
            return true;
        }
    }

    internal class PartV1
    {
        public MachineV1 Machine { get; private set; }
        public PartType Type { get; private set; }
        public Guid SerialNumber { get; private set; }
        public DateTime ProductionCompletionTime { get; set; } = DateTime.MinValue;
        public PartV1(MachineV1 machine, PartType type, Guid serialNumber)
        {
            Machine = machine;
            Type = type;
            SerialNumber = serialNumber;
        }
    }
}
