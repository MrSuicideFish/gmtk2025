using UnityEngine;
using UnityEngine.Events;

public class EntitySlot : MonoBehaviour
{
    public delegate void EntitySlotEventHandler(PhysInteractable entity);
    public event EntitySlotEventHandler OnEntityAdded;
    public event EntitySlotEventHandler OnEntityRemoved;
    
    public string ValidEntityTag = "Untagged";
    public Transform SlotTransform;
    private PhysInteractable m_currentEntity;
    private bool m_isOccupied = false;
    
    public UnityEvent OnEntityAddedEvent;
    public UnityEvent OnEntityRemovedEvent;

    private void OnTriggerStay(Collider other)
    {
        if (m_isOccupied) return;
        PhysInteractable physInteractable = other.GetComponent<PhysInteractable>();
        if (physInteractable != null)
        {
            if (ValidEntityTag.Equals(physInteractable.tag))
            {
                physInteractable.Release();

                Rigidbody rb = physInteractable.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;

                physInteractable.transform.SetParent(SlotTransform, true);
                physInteractable.transform.localPosition = Vector3.zero;
                physInteractable.transform.localEulerAngles = Vector3.zero;

                m_currentEntity = physInteractable;
                m_isOccupied = true;
                OnEntityAdded?.Invoke(m_currentEntity);
                OnEntityAddedEvent?.Invoke();
                
                Debug.Log("Entity added to slot: " + physInteractable.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PhysInteractable physInteractable = other.GetComponent<PhysInteractable>();
        if (physInteractable != null)
        {
            if (physInteractable == m_currentEntity)
            {
                m_currentEntity = null;
                m_isOccupied = false;
                OnEntityRemoved?.Invoke(physInteractable);
                OnEntityRemovedEvent?.Invoke();
                
                Debug.Log("Entity removed from slot: " + physInteractable.name);
            }
        }
    }
}
