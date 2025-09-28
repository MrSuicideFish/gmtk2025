using System;
using System.Collections.Generic;
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
    
    private List<string> m_notifiedEntities = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        PhysInteractable physInteractable = other.GetComponent<PhysInteractable>();
        if (physInteractable != null)
        {
            bool found = false;
            foreach (var notifEvent in m_notifEvents)
            {
                if (notifEvent.Any 
                    || notifEvent.EntityName.Equals(physInteractable.gameObject.name))
                {
                    found = true;
                    notifEvent.OnNotify?.Invoke();
                    break;
                }
            }

            if (found)
            {
                m_notifiedEntities.Add(physInteractable.gameObject.name);
                if (m_notifiedEntities.Count == m_notifEvents.Length)
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
