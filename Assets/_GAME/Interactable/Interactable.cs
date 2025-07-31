using UnityEngine;

namespace _GAME
{
    public class Interactable : MonoBehaviour
    {
        public string DisplayName = "interactable";
        public string Instruction = string.Empty;
        
        public virtual void BeginHover()
        {
            
        }

        public virtual void EndHover()
        {
        }

        public virtual void BeginInteract()
        {
            
        }

        public virtual void EndInteract()
        {
            
        }
    }
}