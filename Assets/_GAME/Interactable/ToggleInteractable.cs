using _GAME;
using UnityEngine;
using UnityEngine.Events;

public class ToggleInteractable : Interactable
{
    private static float s_lastToggleTime = 0f;
    
    public bool IsToggled = false;
    public bool CanBeToggledOff = true;
    public float ToggleDelay = 0.2f;

    public UnityEvent<bool> OnToggled;
    public UnityEvent ToggledOn;
    public UnityEvent ToggledOff;
    
    private void OnEnable()
    {
        OnBeginInteract.AddListener(OnInteract);
    }

    private void OnDisable()
    {
        OnBeginInteract.RemoveListener(OnInteract);
    }

    private void OnInteract()
    {
        Toggle();
    }

    public void Toggle(bool value)
    {
        if(CanBeToggledOff == false && value == false)
        {
            return;
        }

        IsToggled = value;
        OnToggled?.Invoke(value);
        s_lastToggleTime = Time.time;

        if (IsToggled)
        {
            ToggledOn?.Invoke();
        }
        else
        {
            ToggledOff?.Invoke();
        }
    }

    public void Toggle()
    {
        Toggle(!IsToggled);
    }
}
