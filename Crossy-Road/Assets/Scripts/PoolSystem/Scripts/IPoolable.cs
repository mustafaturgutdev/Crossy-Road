namespace Blink.PoolSystem
{
    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
    }
}