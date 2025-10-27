namespace BoschCodeChallenge
{
    public interface IMachineState
    {
        StateType State { get; }
        bool Start(PartType partType);
        bool Stop();
        bool Maintain();
    }
}
