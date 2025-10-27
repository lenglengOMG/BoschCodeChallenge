using System;
using System.Collections.Generic;

namespace BoschCodeChallenge
{
    public class Machine
    {
        private IMachineState _currentState;
        private List<PartType> _supportedParts;

        public string MachineId { get; private set; }
        public string Location { get; set; }

        public Machine(string id, List<PartType> supportedParts)
        {
            MachineId = id;
            _supportedParts = supportedParts;
        }

        public bool Start(PartType partType)
        {
            if (!_supportedParts.Contains(partType))
            {
                return false;
            }
            return _currentState.Start(partType);
        }

        public bool Stop()
        {
            return _currentState.Stop();
        }

        public bool Maintain()
        {
            return _currentState.Maintain();
        }

        public void SetOperationState(IMachineState newState)
        {
            _currentState = newState;
        }

        public StateType GetCurrentState()
        {
            return _currentState.State;
        }
    }
}
