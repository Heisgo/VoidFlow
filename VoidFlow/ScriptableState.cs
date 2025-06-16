using System;
using UnityEngine;

namespace VoidFlow
{
    /// <summary>
    /// A ScriptableObject wrapper to kick off a flow. Drag this asset into the Machine to set the start state
    /// Totally optional, but neat for inspectors
    /// </summary>
    [CreateAssetMenu(menuName = "VoidFlow/Start State")]
    public class ScriptableState : ScriptableObject
    {
        [Tooltip("Pick the FlowState type to start with. Use full namespace.ClassName.")]
        public string stateClassName;

        /// <summary>
        /// Returns the System.Type for the chosen class name, or null if not found.
        /// </summary>
        public Type GetStateType()
        {
            var t = Type.GetType(stateClassName);
            if (t == null)
                Debug.LogWarning($"[VoidFlow] State '{stateClassName}' not found.");
            return t;
        }
    }
}
