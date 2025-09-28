using UnityEngine;

namespace _GAME.Gameplay
{
    public class Room : MonoBehaviour
    {
        public delegate void RoomEventHandler(Room room);
        public event RoomEventHandler OnRoomEntryComplete;
        public event RoomEventHandler OnRoomPuzzleComplete;
        public event RoomEventHandler OnRoomPuzzleIncomplete;
        
        public Transform RoomConnector;
        public ToggleInteractable EntranceDoor;
        public ToggleInteractable ExitDoor;
        public NotifyPlayerLeftZone PlayerEnterRoomZone;

        public bool AutoLoadNextPuzzle = true;

        [HideInInspector] public int RoomIndex;
        [HideInInspector] public Room PrevRoom;
        [HideInInspector] public Room NextRoom;

        private bool m_isPuzzleComplete = false;

        public void NotifyRoomEntry()
        {
            OnRoomEntryComplete?.Invoke(this);
        }

        public void NotifyPuzzleIncomplete()
        {
            if (m_isPuzzleComplete == false)
            {
                return;
            }
            
            OnRoomPuzzleIncomplete?.Invoke(this);
            m_isPuzzleComplete = false;
        }

        public void UnloadPreviousPuzzle()
        {
            var gamemanager = FindFirstObjectByType<GameManager>();
            gamemanager.DestroyOldPuzzle();
        }

        public void LoadNextPuzzle()
        {
            var gamemanager = FindFirstObjectByType<GameManager>();
            gamemanager.LoadNextPuzzle();
        }

        [ContextMenu("Complete Puzzle")]
        public void NotifyPuzzleComplete()
        {
            if (m_isPuzzleComplete)
            {
                return;
            }
            
            OnRoomPuzzleComplete?.Invoke(this);
            m_isPuzzleComplete = true;
        }
        
        public void SetRoomConnector(Transform connector)
        {
            RoomConnector = connector;
        }

        public GameObject RoomUI;
        public void EndGame()
        {
            var player = FindFirstObjectByType<PlayerController>();
            player.enabled = false;
            player.gameObject.SetActive(false);
            RoomUI.gameObject.SetActive(true);
            
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            gameManager.EndGame();
        }
    }
}