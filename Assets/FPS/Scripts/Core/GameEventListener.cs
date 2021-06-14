using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Core
{
    [System.Serializable]
    public class MyUnityGameEvent : UnityEvent<float[]>
    {
    }

    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] MyGameEvent _gameEvent;
        [SerializeField] MyUnityGameEvent _unityEvent;

        void OnEnable() => _gameEvent.AddListener(this);

        void OnDisable() => _gameEvent.RemoveListener(this);

        public void Invoke(float[] values)
        {
            _unityEvent?.Invoke(values);
        }
    }
}