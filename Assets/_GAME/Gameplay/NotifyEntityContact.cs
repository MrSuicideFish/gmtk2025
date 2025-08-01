using System;
using UnityEngine;
using UnityEngine.Events;

public class NotifyEntityContact : MonoBehaviour
{
    [Serializable]
    public class NotifEvent
    {
        public bool Any;
        public string EntityName;
        public UnityEvent OnNotify;
    }

    [SerializeField] private bool m_destoryEncroacher = true;
    [SerializeField] private UnityEvent m_onAllNotified;
    [SerializeField] private NotifEvent[] m_notifEvents;
    private byte m_notifCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        PhysInteractable physInteractable = other.GetComponent<PhysInteractable>();
        if (physInteractable != null)
        {
            bool found = false;
            foreach (var notifEvent in m_notifEvents)
            {
                if (notifEvent.Any 
                    || notifEvent.EntityName == physInteractable.gameObject.name)
                {
                    found = true;
                    m_notifCount++;
                    notifEvent.OnNotify?.Invoke();
                    break;
                }
            }

            if (found)
            {
                if (m_notifCount == m_notifEvents.Length)
                {
                    m_onAllNotified?.Invoke();
                }

                if (m_destoryEncroacher)
                {
                    physInteractable.ReleaseAndDestroy();    
                }
            }
        }
    }
}
