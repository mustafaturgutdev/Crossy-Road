namespace PoolSystem
{
    public class Factory<T> : IFactory<T> where T : class, new()
    {
        public virtual T Create() => new();
    }



}