﻿namespace Blink.PoolSystem
{
    public interface IFactory<T> where T : class
    {
        T Create();
    }



}