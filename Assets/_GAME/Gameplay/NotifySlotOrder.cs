using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotifySlotOrder : MonoBehaviour
{
    [SerializeField] private List<EntitySlot> m_slots = new List<EntitySlot>();
    [SerializeField] private List<string> m_ordering = new List<string>();
    
    public UnityEvent OnOrderSuccess;

    private void OnEnable()
    {
        foreach (var slot in m_slots)
        {
            slot.OnSlotChanged += OnSlotChanged;
        }
    }
    
    private void OnSlotChanged()
    {
        for (int i = 0; i < m_slots.Count; i++)
        {
            if (m_slots[i].CurrentEntity == null) return;
            if (!m_slots[i].CurrentEntity.gameObject.name.Equals(m_ordering[i])) return;
        }

        OnOrderSuccess?.Invoke();
    }
    
}
