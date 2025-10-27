namespace BoschCodeChallenge
{
    public class RunningState : MachineState
    {
        public override StateType State => StateType.Running;

        public RunningState(Machine machine, MachineManager machineManager) : base(machine, machineManager)
        {
        }

        public override bool Start(PartType partType)
        {
            return false;
        }

        public override bool Stop()
        {
            Machine.SetOperationState(new IdleState(Machine, MachineManager));

            InstructionMessage instruction = new InstructionMessage
            {
                MachineId = Machine.MachineId,
                Instruction = InstructionType.Stop
            };

            MachineManager.SendInstructionToMachine(instruction);
            return true;
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
