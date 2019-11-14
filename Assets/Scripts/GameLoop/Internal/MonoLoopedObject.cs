using UnityEngine;
using Zenject;


namespace GameLoop.Internal
{
    /// <summary>
    /// Purpose of this class is to make MonoBehaviours use custom GameLoop without the need of creating GameObjectContext.
    /// If you already use GameObjectContext - use c# class derived from <see cref="LoopedObject{TLoop}"/>.
    /// </summary>
    /// <typeparam name="TLoop"></typeparam>
    public abstract class MonoLoopedObject<TLoop> : MonoBehaviour
    {
        protected TLoop _gameLoop;

        protected virtual bool DefaultSubscribe { get; } = true;
        private bool _isSubscribed = false;

        [Inject]
        public void Construct(TLoop gameLoop)
        {
            _gameLoop = gameLoop;
        }

        protected virtual void OnEnable()
        {
            if (DefaultSubscribe)
            {
                SubscribeLoop();
            }
        }

        protected virtual void OnDisable()
        {
            if (DefaultSubscribe)
            {
                UnsubscribeLoop();
            }
        }

        protected void SubscribeLoop()
        {
            if (_isSubscribed == false)
            {
                SubscribeLoopInternal();
                _isSubscribed = true;
            }
        }

        protected void UnsubscribeLoop()
        {
            if (_isSubscribed)
            {
                UnsubscribeLoopInternal();
                _isSubscribed = false;
            }
        }

        protected abstract void SubscribeLoopInternal();
        protected abstract void UnsubscribeLoopInternal();
    }
}