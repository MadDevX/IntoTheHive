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
            if (updatable is IUpdatable) _updatablesDirty.Subscribe(updatable as IUpdatable);
            if (updatable is IFixedUpdatable) _fixedUpdatablesDirty.Subscribe(updatable as IFixedUpdatable);
            if (updatable is ILateUpdatable) _lateUpdatablesDirty.Subscribe(updatable as ILateUpdatable);
        }

        public void Unsubscribe(IBaseUpdatable updatable)
        {
            if (updatable is IUpdatable) _updatablesDirty.Unsubscribe(updatable as IUpdatable);
            if (updatable is IFixedUpdatable) _fixedUpdatablesDirty.Unsubscribe(updatable as IFixedUpdatable);
            if (updatable is ILateUpdatable) _lateUpdatablesDirty.Unsubscribe(updatable as ILateUpdatable);
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