using UnityEngine;

namespace _GAME
{
    public class Player : MonoBehaviour
    {
        public delegate void PlayerInteractEventHandler(Interactable interactable);
        public static event PlayerInteractEventHandler OnHoverInteractable;
        public static event PlayerInteractEventHandler OnEndOverHinteractable;
        
        [SerializeField] private GameObject m_fpsHud;
        [SerializeField] private LayerMask m_interactMask;
        [SerializeField] private float m_interactDistance = 1.0f;

        private Ray m_interactRay;
        private Interactable m_lastInteractable;
        
        private void Start()
        {
            Instantiate(m_fpsHud);
        }

        private void BeginHoverInteractable(Interactable interactable)
        {
            if (interactable == m_lastInteractable)
            {
                return;
            }
            
            EndHoverInteractable();

            if (interactable != null)
            {
                OnHoverInteractable?.Invoke(interactable);
                interactable.BeginHover();
                m_lastInteractable = interactable;    
            }
        }

        private void EndHoverInteractable()
        {
            if (m_lastInteractable == null)
            {
                return;
            }
            
            m_lastInteractable.EndHover();
            OnEndOverHinteractable?.Invoke(m_lastInteractable);
            m_lastInteractable = null;
        }

        private void Update()
        {
            m_interactRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool interact = Physics.Raycast(m_interactRay, out RaycastHit hit, m_interactDistance, m_interactMask);
            if(interact)
            {
                // If we hit an interactable object, begin hovering it
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    BeginHoverInteractable(interactable);
                }
                else if (m_lastInteractable != null)
                {
                    EndHoverInteractable();
                }
            }
            else
            {
                EndHoverInteractable();
            }

            if (m_lastInteractable != null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    m_lastInteractable.EndInteract();
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    m_lastInteractable.BeginInteract();
                }
            }
        }
    }
}