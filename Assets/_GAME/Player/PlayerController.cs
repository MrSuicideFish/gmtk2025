using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    
    [SerializeField] private float m_speed = 5f;
    
    [SerializeField] private float m_lookXSensitivity = 2f;
    [SerializeField] private float m_lookYSensitivity = 2f;
    
    [SerializeField] private float m_clampMinX = -80f;
    [SerializeField] private float m_clampMaxX = 80f;
    
    private float m_mouseX;
    private Vector3 m_moveDirection;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(m_rigidbody == null)
        {
            Debug.LogError("Camera or Rigidbody is not assigned.");
            return;
        }

        // Get input for movement
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        // Calculate movement direction based on camera orientation
        Vector3 cameraForward = transform.forward;
        cameraForward.y = 0; // Ignore vertical component
        Vector3 cameraRight = transform.right;
        cameraRight.y = 0; // Ignore vertical component
        m_moveDirection = (cameraForward * input.y + cameraRight * input.x).normalized;
        
        // Look around w/ mouse
        m_mouseX = Input.GetAxis("Mouse X") * m_lookXSensitivity;
        
        if (m_mouseX != 0)
        {
            // Rotate the player based on mouse input
            Vector3 rotation = new Vector3(0, m_mouseX, 0);
            transform.Rotate(rotation);
        }
    }

    private void FixedUpdate()
    {
        m_rigidbody.linearVelocity =
            new Vector3(m_moveDirection.x, m_rigidbody.linearVelocity.y, m_moveDirection.z) * m_speed;
    }
}
