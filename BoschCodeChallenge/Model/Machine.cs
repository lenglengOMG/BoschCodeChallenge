using System.Collections.Generic;
using System.Linq;

namespace BoschCodeChallenge
{
    public class Machine
    {
        private IMachineState _currentState;
        private readonly List<PartType> _supportedParts;

        public string MachineId { get; private set; }
        public string Location { get; set; }

        public Machine(string id, List<PartType> supportedParts)
        {
            MachineId = id;
            _supportedParts = supportedParts;
        }

        /// <summary>
        /// Initiates the process with the specified collection of parts.
        /// </summary>
        /// <param name="parts">A collection of <see cref="Part"/> objects to be processed. Each part must have a type that is supported by
        /// the system.</param>
        /// <returns><see langword="true"/> if the process starts successfully; otherwise, <see langword="false"/> if any part
        /// has an unsupported type.</returns>
        public bool Start(IEnumerable<Part> parts)
        {
            if (parts.Any(p => !_supportedParts.Contains(p.Type)))
            {
                return false;
            }
            return _currentState.Start(parts);
        }

        /// <summary>
        /// Stops the current operation and transitions the state to stopped.
        /// </summary>
        /// <returns><see langword="true"/> if the operation was successfully stopped; otherwise, <see langword="false"/>.</returns>
        public bool Stop()
        {
            return _currentState.Stop();
        }

        /// <summary>
        /// Attempts to maintain the current state.
        /// </summary>
        /// <returns><see langword="true"/> if the state was successfully maintained; otherwise, <see langword="false"/>.</returns>
        public bool Maintain()
        {
            return _currentState.Maintain();
        }

        /// <summary>
        /// Sets the operation state of the machine to the specified new state.
        /// </summary>
        /// <param name="newState"></param>
        public void SetOperationState(IMachineState newState)
        {
            _currentState = newState;
        }

        /// <summary>
        /// Gets the current state of the machine.
        /// </summary>
        /// <returns></returns>
        public StateType GetCurrentState()
        {
            return _currentState.State;
        }
    }
}
