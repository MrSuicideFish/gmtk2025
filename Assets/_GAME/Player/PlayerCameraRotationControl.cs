using Unity.Cinemachine;
using UnityEngine;

namespace _GAME
{
    [AddComponentMenu("Cinemachine/Procedural/Rotation Control/Player Look")]
    [SaveDuringPlay]
    [DisallowMultipleComponent]
    [CameraPipeline(CinemachineCore.Stage.Aim)]
    public class PlayerCameraRotationControl : CinemachineComponentBase
    {
        public override bool IsValid => enabled && FollowTarget != null;
        public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Aim;

        [SerializeField] private float m_lookXSensitivity = 2f;
        [SerializeField] private float m_lookYSensitivity = 2f;
        
        private float m_mouseY;
        private float m_rotX;

        private void Update()
        {
            m_mouseY = Input.GetAxis("Mouse Y") * m_lookYSensitivity;
            if (m_mouseY != 0)
            {
                m_rotX += m_mouseY;
            }
        }

        public override void MutateCameraState(ref CameraState curState, float deltaTime)
        {
            m_rotX = Mathf.Clamp(m_rotX, -80f, 80f); // Clamp vertical rotation to prevent flipping
            curState.OrientationCorrection = Quaternion.Euler(-m_rotX, 0, 0);
        }
    }
}