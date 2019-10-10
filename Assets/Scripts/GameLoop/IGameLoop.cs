namespace GameLoop.Internal
{
    public interface IGameLoop
    {
        bool IsPaused { get; set; }
        void Subscribe(IBaseUpdatable updatable);
        void Unsubscribe(IBaseUpdatable updatable);
    }
}
