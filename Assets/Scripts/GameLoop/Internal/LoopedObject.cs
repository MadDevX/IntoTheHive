using Relays;
using System;
using Zenject;

namespace GameLoop.Internal
{
    public abstract class LoopedObject<TLoop> : IInitializable, IDisposable
    {
        protected TLoop _gameLoop;
        protected IRelay _relay;

        protected virtual bool DefaultSubscribe { get; } = true;
        private bool _isSubscribed = false;

        [Inject]
        public void Construct(TLoop gameLoop, IRelay relay)
        {
            _gameLoop = gameLoop;
            _relay = relay;
        }


        public void Initialize()
        {
            if (DefaultSubscribe)
            {
                _relay.OnEnableEvt += SubscribeLoop;
                _relay.OnDisableEvt += UnsubscribeLoop;

                if (_relay.IsActive)
                {
                    SubscribeLoop();
                }
            }
        }

        public void Dispose()
        {
            if (DefaultSubscribe)
            {
                _relay.OnEnableEvt -= SubscribeLoop;
                _relay.OnDisableEvt -= UnsubscribeLoop;
            }

            UnsubscribeLoop();
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