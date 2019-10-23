using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    /// <summary>
    /// See summary of <see cref="MonoLoopedObject{TLoop}"/>.
    /// </summary>
    public abstract class MonoGlobalLoopedObject : MonoLoopedObject<IGameLoop>
    {
    }
}