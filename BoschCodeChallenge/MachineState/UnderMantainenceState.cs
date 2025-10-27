namespace BoschCodeChallenge
{
    public class UnderMantainenceState : MachineState
    {
        public override StateType State => StateType.UnderMaintenance;

        public UnderMantainenceState(Machine machine, MachineManager machineManager) : base(machine, machineManager)
        {
        }

        public override bool Start(PartType partType)
        {
            return false;
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
