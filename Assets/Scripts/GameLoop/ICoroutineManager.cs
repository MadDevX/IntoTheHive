
using System;

namespace GameLoop
{
    public interface ICoroutineManager
    {
        ICoroutine StartCoroutine(Action<float> action, float stopAfterTime);
        ICoroutine StartCoroutine(Action<float> action);
    }
}