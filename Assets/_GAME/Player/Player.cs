using UnityEngine;

namespace _GAME
{
    public class Player : MonoBehaviour
    {
        public delegate void PlayerInteractEventHandler(Interactable interactable);
        public static event PlayerInteractEventHandler OnHoverInteractable;
        public static event PlayerInteractEventHandler OnEndOverHinteractable;

        public static event PlayerInteractEventHandler OnBeginInteract;
        public static event PlayerInteractEventHandler OnEndInteract;
        
        [SerializeField] private GameObject m_fpsHud;
        [SerializeField] private LayerMask m_interactMask;
        [SerializeField] private float m_interactDistance = 1.0f;
        [SerializeField] private Rigidbody m_physInteractBody;

        
        private Ray m_interactRay;
        
        private Interactable m_hoveredInteractable;
        private Interactable m_currentInteractable;
        
        private void Start()
        {
            Vector3 localPos = m_physInteractBody.transform.localPosition;
            m_physInteractBody.transform.SetParent(Camera.main.transform, true);
            m_physInteractBody.transform.localPosition = localPos;
            
            Instantiate(m_fpsHud);
        }

        private void BeginHoverInteractable(Interactable interactable)
        {
            if (interactable == m_hoveredInteractable
                || interactable == m_currentInteractable)
            {
                return;
            }
            
            EndHoverInteractable();

            if (interactable != null)
            {
                m_hoveredInteractable = interactable;
                m_hoveredInteractable.BeginHover();
                OnHoverInteractable?.Invoke(m_hoveredInteractable);
            }
        }

        private void EndHoverInteractable()
        {
            if (m_hoveredInteractable == null)
            {
                return;
            }
            
            m_hoveredInteractable.EndHover();
            OnEndOverHinteractable?.Invoke(m_hoveredInteractable);
            m_hoveredInteractable = null;
        }

        private void BeginInteract(Interactable interactable)
        {
            if (interactable == m_currentInteractable)
            {
                return;
            }

            if (m_currentInteractable != null)
            {
                EndInteract();
            }

            EndHoverInteractable();
            m_currentInteractable = interactable;

            if (m_currentInteractable is PhysInteractable physObj)
            {
                // connect joint
                ConfigurableJoint joint = physObj.GetComponent<ConfigurableJoint>();
                if (joint == null)
                {
                    joint = physObj.gameObject.AddComponent<ConfigurableJoint>();
                }
                
                joint.connectedBody = m_physInteractBody;
                
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;
                
                joint.angularXMotion = ConfigurableJointMotion.Limited;
                joint.angularYMotion = ConfigurableJointMotion.Limited;
                joint.angularZMotion = ConfigurableJointMotion.Limited;
                
                joint.linearLimit = new SoftJointLimit { limit = 0.0f };
                joint.linearLimitSpring = new SoftJointLimitSpring { spring = 0.1f, damper = 0f };
                
                joint.angularYLimit = new SoftJointLimit { limit = 0f };
                joint.angularZLimit = new SoftJointLimit { limit = 0f };
                joint.angularXLimitSpring = new SoftJointLimitSpring { spring = 1, damper = 0.1f };
                joint.angularYZLimitSpring = new SoftJointLimitSpring { spring = 1, damper = 0.1f };
                
                joint.anchor = Vector3.zero;
                
                var xdrive = joint.xDrive;
                xdrive.useAcceleration = true;
                joint.xDrive = xdrive;
                
                var ydrive = joint.yDrive;
                ydrive.useAcceleration = true;
                joint.yDrive = ydrive;
                
                var zdrive = joint.zDrive;
                zdrive.useAcceleration = true;
                joint.zDrive = zdrive;
                
                joint.connectedAnchor = Vector3.zero;
                joint.autoConfigureConnectedAnchor = false;
                joint.breakForce = float.MaxValue;
                joint.breakTorque = float.MaxValue;
            }
            
            m_currentInteractable.OnDestroyInvoked += EndInteract;
            m_currentInteractable.BeginInteract(this);
            OnBeginInteract?.Invoke(m_currentInteractable);
        }

        private void EndInteract()
        {
            if (m_currentInteractable == null)
            {
                return;
            }
            
            if (m_currentInteractable is PhysInteractable physObj)
            {
                ConfigurableJoint joint = physObj.GetComponent<ConfigurableJoint>();
                if (joint != null)
                {
                    Destroy(joint);
                }
            }
            
            m_currentInteractable.EndInteract();
            OnEndInteract?.Invoke(m_currentInteractable);
            m_currentInteractable = null;
        }

        private void Update()
        {
            m_interactRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hovering = Physics.Raycast(m_interactRay, out RaycastHit hit, m_interactDistance, m_interactMask);
            if(hovering)
            {
                // If we hit an interactable object, begin hovering it
                Interactable hoverInteractable = hit.collider.GetComponent<Interactable>();
                if (hoverInteractable != null)
                {
                    BeginHoverInteractable(hoverInteractable);
                }
                else if (m_currentInteractable != null)
                {
                    EndHoverInteractable();
                }
            }
            else
            {
                EndHoverInteractable();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (m_hoveredInteractable != null)
                {
                    BeginInteract(m_hoveredInteractable);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                EndInteract();
            }
        }
    }
}