using UnityEngine;

namespace PoolSystem
{
    public class UnityObjectFactory<T> : IFactory<T> where T : Component
    {
        private readonly T m_Prefab;
        protected readonly Transform m_Parent;

        public UnityObjectFactory(T prefab)
        {
            m_Prefab = prefab;
            m_Parent = new GameObject(typeof(T).Name + "Pool").transform;
        }

        public virtual T Create() => Object.Instantiate(m_Prefab, m_Parent);
    }
}