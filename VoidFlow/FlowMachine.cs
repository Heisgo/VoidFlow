using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyData // This is just an example, you can make your own
{
    public float moveSpeed = 3f;
    public float viewDistance = 5f;
    public float forgetDistance = 7f;
    public Transform idleTarget;
}

namespace VoidFlow
{
    /// <summary>
    /// The boss component you slap on any GameObject to drive FlowStates. Transitions, updates, all that jazz
    /// </summary>
    public class FlowMachine : MonoBehaviour
    {
        [Tooltip("Start with this state when the game begins, if set.")] // Switch this from tooltip to TooltipExtended if u're using VoidAttributes ;D
        [SerializeField] private ScriptableState startState;

        [Header("Enemy Settings")]
        public EnemyData data = new();

        private FlowState current;
        private readonly Dictionary<Type, FlowState> cache = new();

        void Start()
        {
            if (startState != null)
                TransitionTo(startState.GetStateType());
        }

        void Update()
        {
            current?.OnUpdate();
        }

        /// <summary>
        /// Jump to a new state of type T. Lazy-inits and caches instances, no drama
        /// </summary>
        public void TransitionTo<T>() where T : FlowState, new() => TransitionTo(typeof(T));

        /// <summary>
        /// Core transition logic: Exit old, enter new, easy peasy
        /// </summary>
        public void TransitionTo(Type t)
        {
            if (current != null)
            {
                Debug.Log($"[Flow] Exiting {current.GetType().Name}");
                current.OnExit();
            }

            if (!cache.TryGetValue(t, out var next))
            {
                next = (FlowState)Activator.CreateInstance(t);
                next.SetMachine(this);
                cache[t] = next;
            }

            current = next;
            Debug.Log($"[Flow] Entering {t.Name}");
            current.OnEnter();
        }

        /// <summary>
        /// Peek at the current state's type—handy for UI or debug
        /// </summary>
        public Type GetCurrentState() => current?.GetType();

        /// <summary>
        /// Wanna chill and switch later? Schedules a transition after a delay—like an alarm clock for states
        /// </summary>
        public void TransitionAfter<T>(float seconds) where T : FlowState, new()
        {
            StartCoroutine(DelayedTransition(seconds, typeof(T)));
        }

        private IEnumerator DelayedTransition(float seconds, Type t)
        {
            yield return new WaitForSeconds(seconds);
            TransitionTo(t);
        }
    }
}
