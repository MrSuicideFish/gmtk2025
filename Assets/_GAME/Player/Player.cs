using Unity.Cinemachine;
using UnityEngine;

namespace _GAME.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject m_fpsHud;
        [SerializeField] private CinemachineCamera m_fpsCamera;
        
        private void Start()
        {
            Instantiate(m_fpsHud);
        }

        private void TryInteract()
        {
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                
            }
        }
    }
}