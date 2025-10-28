using System.Collections.Generic;

namespace BoschCodeChallenge
{
    public interface IMachineState
    {
        StateType State { get; }
        bool Start(IEnumerable<Part> parts);
        bool Stop();
        bool Maintain();
    }
}
