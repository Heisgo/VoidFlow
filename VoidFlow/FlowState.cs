using System;
using System.Collections;
using UnityEngine;

namespace VoidFlow
{
    /// <summary>
    /// Your chill base class for states. Hook into OnEnter, OnUpdate, OnExit—like vibes for ur game logic
    /// </summary>
    public abstract class FlowState
    {
        /// <summary>
        /// The machine running this state. Use it to switch gears, peek info, or start timers
        /// </summary>
        protected FlowMachine machine;

        /// <summary>
        /// Internal setter for the machine. You don't gotta worry about this
        /// </summary>
        internal void SetMachine(FlowMachine m) => machine = m;

        /// <summary>Called once when the state kicks in—set up your stuff here</summary>
        public virtual void OnEnter() { }
        /// <summary>Called every frame while this state is active—do your thing</summary>
        public virtual void OnUpdate() { }
        /// <summary>Called once when leaving—clean up or fire off events</summary>
        public virtual void OnExit() { }

        /// <summary>
        /// Chill for a bit then fire the callback. Handy for delays inside a state
        /// </summary>
        protected void Wait(float seconds, Action callback)
        {
            machine.StartCoroutine(WaitRoutine(seconds, callback));
        }

        private IEnumerator WaitRoutine(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}
