using UnityEngine;
using UnityEngine.Events;

public class NotifyPlayerLeftZone : MonoBehaviour
{
    public UnityEvent OnPlayerEnteredZone;
    public UnityEvent OnPlayerLeftZone;
    
    public bool DestroyOnEnter = false;
    public bool DestroyOnExit= false;
    
    private bool m_playerPresent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_playerPresent = true;
            OnPlayerEnteredZone?.Invoke();

            if (DestroyOnEnter)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && m_playerPresent)
        {
            m_playerPresent = false;
            OnPlayerLeftZone?.Invoke();

            if (DestroyOnExit)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
