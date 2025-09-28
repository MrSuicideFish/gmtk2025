using UnityEngine;
using UnityEngine.Events;

namespace _GAME.Gameplay
{
    public class NotifyLookAt : MonoBehaviour
    {
        public float LookAtAngleThreshold = 30.0f;
        
        public UnityEvent OnBeginLookAt;
        public UnityEvent OnEndLookAt;

        private bool m_isLooking = false;
        
        private void LateUpdate()
        {
            float angle = Vector3.Angle(Camera.main.transform.forward, transform.position - Camera.main.transform.position);
            if (angle < LookAtAngleThreshold)
            {
                if (!m_isLooking)
                {
                    Debug.Log("IS LOOKING");
                    m_isLooking = true;
                    OnBeginLookAt?.Invoke();    
                }
            }
            else if(m_isLooking)
            {
                Debug.Log("STOPPED LOOKNIG");
                m_isLooking = false;
                OnEndLookAt?.Invoke();
            }
        }
    }
}