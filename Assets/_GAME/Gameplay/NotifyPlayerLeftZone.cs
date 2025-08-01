using UnityEngine;
using UnityEngine.Events;

public class NotifyPlayerLeftZone : MonoBehaviour
{
    public UnityEvent OnPlayerLeftZone;
    
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerLeftZone?.Invoke();
        }
    }
}
