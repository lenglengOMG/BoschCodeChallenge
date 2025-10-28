using System.Collections.Generic;
using System.Linq;

namespace BoschCodeChallenge
{
    public class IdleState : MachineState
    {
        public override StateType State => StateType.Idle;

        public IdleState(Machine machine, MachineManager machineManager) : base(machine, machineManager)
        {
        }

        public override bool Start(IEnumerable<Part> parts)
        {
            Machine.SetOperationState(new RunningState(Machine, MachineManager));

            InstructionMessage instruction = new InstructionMessage
            {
                MachineId = Machine.MachineId,
                Instruction = InstructionType.Start,
                PartInfos = parts.ToList()
            };

            MachineManager.SendInstructionToMachine(instruction);
            return true;
        }

        public override bool Stop()
        {
            return false;
        }

        public override bool Maintain()
        {
            Machine.SetOperationState(new UnderMantainenceState(Machine, MachineManager));

            InstructionMessage instruction = new InstructionMessage
            {
                MachineId = Machine.MachineId,
                Instruction = InstructionType.Maintain
            };

            MachineManager.SendInstructionToMachine(instruction);
            return true;
        }
    }
}
