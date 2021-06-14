using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Core
{
    [CreateAssetMenu]
    public class MyGameEvent : ScriptableObject
    {
        List<GameEventListener> _listeners;

        public void AddListener(GameEventListener listener) => _listeners.Add(listener);
        public void RemoveListener(GameEventListener listener) => _listeners.Remove(listener);

        public void Raise(float[] values)
        {
            foreach (var listener in _listeners)
            {
                listener?.Invoke(values);
            }
        }
    }
}