using System.Collections.Generic;

namespace Blink.PoolSystem
{
    public class Pool<T> : IPool<T> where T : class, IPoolable
    {
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly IFactory<T> _factory;

        public Pool(IFactory<T> factory, int initialSize = 0)
        {
            _factory = factory;
            Initialize(initialSize);
        }

        public T Spawn()
        {
            T value = _pool.Count > 0 ? _pool.Dequeue() : Create();
            value.OnSpawned();
            return value;
        }

        public virtual bool Despawn(T value)
        {
            if (!_pool.Contains(value))
            {
                _pool.Enqueue(value);
                value.OnDespawned();
                return true;
            }
            return false;
        }

        protected virtual T Create() => _factory.Create();

        protected void Initialize(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                T value = Create();
                value.OnDespawned();
                _pool.Enqueue(value);
            }
        }
    }



}