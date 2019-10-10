namespace GameLoop.Internal
{
    public interface IBaseUpdatable
    {
    }

    public interface IUpdatable : IBaseUpdatable
    {
        void OnUpdate(float deltaTime);
    }

    public interface IFixedUpdatable : IBaseUpdatable
    {
        void OnFixedUpdate(float deltaTime);
    }

    public interface ILateUpdatable : IBaseUpdatable
    {
        void OnLateUpdate(float deltaTime);
    }
}