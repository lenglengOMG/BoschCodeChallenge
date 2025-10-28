using System.Collections.Generic;

namespace BoschCodeChallenge
{
    public abstract class MachineState : IMachineState
    {
        protected Machine Machine;
        protected MachineManager MachineManager;

        public abstract StateType State { get; }

        public MachineState(Machine machine, MachineManager machineManager)
        {
            Machine = machine;
            MachineManager = machineManager;
        }

        public abstract bool Maintain();

        public abstract bool Start(IEnumerable<Part> parts);

        public abstract bool Stop();
    }
}
