using UnityEngine;
using UnityEngine.Events;

public class NotifyPlayerLeftZone : MonoBehaviour
{
    public UnityEvent OnPlayerEnteredZone;
    public UnityEvent OnPlayerLeftZone;
    private bool m_playerPresent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_playerPresent = true;
            OnPlayerEnteredZone?.Invoke();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && m_playerPresent)
        {
            m_playerPresent = false;
            OnPlayerLeftZone?.Invoke();
        }
    }
}
