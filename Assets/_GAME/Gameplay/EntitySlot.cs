using UnityEngine;
using UnityEngine.Events;

public class EntitySlot : MonoBehaviour
{
    public delegate void EntitySlotEventHandler();
    public event EntitySlotEventHandler OnSlotChanged;
    
    public string ValidEntityTag = "Untagged";
    public Transform SlotTransform;
    private PhysInteractable m_currentEntity;
    private bool m_isOccupied = false;
    
    public UnityEvent OnEntityAddedEvent;
    public UnityEvent OnEntityRemovedEvent;
    
    public PhysInteractable CurrentEntity
    {
        get { return m_currentEntity; }
    }

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
                OnSlotChanged?.Invoke();
                OnEntityAddedEvent?.Invoke();
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
                OnSlotChanged?.Invoke();
                OnEntityRemovedEvent?.Invoke();
            }
        }
    }
}
