namespace PoolSystem
{
    public interface IPool<T> where T : IPoolable
    {
        T Spawn();
        bool Despawn(T value);
    }



}