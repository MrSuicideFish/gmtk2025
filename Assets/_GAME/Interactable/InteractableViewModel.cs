using TMPro;
using UnityEngine;

namespace _GAME
{
    public class InteractableViewModel : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_displayName;
        [SerializeField] private TMP_Text m_instructionText;

        public string DisplayName
        {
            get => m_displayName.text;
            set => m_displayName.text = value;
        }
        
        public string InstructionText
        {
            get => m_instructionText.text;
            set => m_instructionText.text = value;
        }
        
        private void OnEnable()
        {
            Player.OnHoverInteractable -= OnBeginHover;
            Player.OnHoverInteractable += OnBeginHover;
            
            Player.OnEndOverHinteractable -= OnEndHover;
            Player.OnEndOverHinteractable += OnEndHover;
            
            Clear();
        }
        
        private void OnDisable()
        {
            Player.OnHoverInteractable -= OnBeginHover;
            Player.OnEndOverHinteractable -= OnEndHover;
        }

        private void Clear()
        {
            DisplayName = string.Empty;
            InstructionText = string.Empty;
        }

        private void OnEndHover(Interactable interactable)
        {
            Clear();
        }

        private void OnBeginHover(Interactable interactable)
        {
            DisplayName = interactable.DisplayName;
            InstructionText = interactable.Instruction;
        }
    }
}