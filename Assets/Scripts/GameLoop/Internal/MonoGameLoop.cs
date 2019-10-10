using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public class MonoGameLoop : MonoBehaviour, IGameLoop
    {
        public bool IsPaused { get; set; }

        private EventList<IUpdatable> _updatablesDirty = new EventList<IUpdatable>();
        private EventList<IFixedUpdatable> _fixedUpdatablesDirty = new EventList<IFixedUpdatable>();
        private EventList<ILateUpdatable> _lateUpdatablesDirty = new EventList<ILateUpdatable>();

        private EventList<IUpdatable> _updatables = new EventList<IUpdatable>();
        private EventList<IFixedUpdatable> _fixedUpdatables = new EventList<IFixedUpdatable>();
        private EventList<ILateUpdatable> _lateUpdatables = new EventList<ILateUpdatable>();

        public void Subscribe(IBaseUpdatable updatable)
        {
            switch(updatable)
            {
                case IUpdatable up1:
                    _updatablesDirty.Subscribe(up1);
                    break;
                case IFixedUpdatable up2:
                    _fixedUpdatablesDirty.Subscribe(up2);
                    break;
                case ILateUpdatable up3:
                    _lateUpdatablesDirty.Subscribe(up3);
                    break;
            }
        }

        public void Unsubscribe(IBaseUpdatable updatable)
        {
            switch (updatable)
            {
                case IUpdatable up1:
                    _updatablesDirty.Unsubscribe(up1);
                    break;
                case IFixedUpdatable up2:
                    _fixedUpdatablesDirty.Unsubscribe(up2);
                    break;
                case ILateUpdatable up3:
                    _lateUpdatablesDirty.Unsubscribe(up3);
                    break;
            }
        }

        private void Update()
        {
            if(IsPaused == false)
            {
                Utility.RefreshEventList(_updatablesDirty, _updatables);
                var deltaTime = Time.deltaTime;
                var subs = _updatables.Subscribers;

                for(int i = 0; i < subs.Count; i++)
                {
                    subs[i].OnUpdate(deltaTime);
                }

            }
        }
        private void FixedUpdate()
        {
            if (IsPaused == false)
            {
                Utility.RefreshEventList(_fixedUpdatablesDirty, _fixedUpdatables);
                var deltaTime = Time.fixedDeltaTime;
                var subs = _fixedUpdatables.Subscribers;

                for (int i = 0; i < subs.Count; i++)
                {
                    subs[i].OnFixedUpdate(deltaTime);
                }
            }
        }
        private void LateUpdate()
        {
            if (IsPaused == false)
            {
                Utility.RefreshEventList(_lateUpdatablesDirty, _lateUpdatables);
                var deltaTime = Time.deltaTime;
                var subs = _lateUpdatables.Subscribers;

                for (int i = 0; i < subs.Count; i++)
                {
                    subs[i].OnLateUpdate(deltaTime);
                }
            }
        }
    }
}