using UnityEngine;
using UnityEngine.Events;

namespace _GAME
{
    public class Interactable : MonoBehaviour
    {
        public bool IsInteractable = true;
        public string DisplayName = "interactable";
        
        [TextArea(2,2)]
        public string Instruction = string.Empty;
        
        [TextArea(2,2)]
        public string DisabledInstruction = string.Empty;
        
        public delegate void InteractableEventHandler();
        public event InteractableEventHandler OnDestroyInvoked;

        public ToggleInteractable RequireToggle;

        public UnityEvent OnBeginInteract;
        public UnityEvent OnEndInteract;

        public UnityEvent OnInteractEnabled;
        public UnityEvent OnInteractDisabled;
        
        public virtual void BeginHover()
        {
            
        }

        public virtual void EndHover()
        {
        }

        public virtual void BeginInteract(Player player)
        {
            if (IsInteractable)
            {
                if(RequireToggle != null 
                   && RequireToggle.IsToggled == false)
                {
                    return;
                }
                
                OnBeginInteract?.Invoke();    
            }
        }

        public virtual void EndInteract()
        {
            OnEndInteract?.Invoke();
        }

        public void ToggleInteractable(bool enable)
        {
            IsInteractable = enable;
            if (enable)
            {
                OnInteractEnabled?.Invoke();    
            }
            else
            {
                OnInteractDisabled?.Invoke();
            }
        }
        public void EnableInteractable()
        {
            IsInteractable = true;
            OnInteractEnabled?.Invoke();
        }
        
        public void DisableInteractable()
        {
            IsInteractable = false;
            OnInteractDisabled?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Release()
        {
            OnDestroyInvoked?.Invoke();
        }
        
        public void ReleaseAndDestroy()
        {
            OnDestroyInvoked?.Invoke();
            GameObject.Destroy(gameObject);
        }
    }
}