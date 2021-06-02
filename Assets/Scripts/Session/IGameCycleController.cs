public interface IGameCycleController
{
    void RaiseOnGameStarted();
    void RaiseOnGameEnded();
    void RaiseOnGameWon();
}