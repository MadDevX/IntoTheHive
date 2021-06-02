using Zenject;

/// <summary>
/// This class acts as a port between singular factories and the rest of weapon system which operates on projectile array factories.
/// </summary>
public abstract class MultiFactory<TArgs, T> : IFactory<TArgs, T[]>
{
    private T[] _objects = new T[1];

    private IFactory<TArgs, T> _factory;
    
    public MultiFactory(IFactory<TArgs, T> factory)
    {
        _factory = factory;
    }

    public T[] Create(TArgs param)
    {
        _objects[0] = _factory.Create(param);
        return _objects;
    }
}
