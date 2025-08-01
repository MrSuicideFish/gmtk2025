using UnityEngine;

namespace _GAME.Gameplay
{
    public class Room : MonoBehaviour
    {
        public delegate void RoomEventHandler(Room room);
        public event RoomEventHandler OnRoomEntryComplete;
        public event RoomEventHandler OnRoomPuzzleComplete;
        
        public int RoomIndex;
        public Transform RoomConnector;
        public ToggleInteractable EntranceDoor;
        public ToggleInteractable ExitDoor;
        public bool IsPuzzleComplete = false;

        [HideInInspector] public Room NextRoom;
        

        public void NotifyRoomEntry()
        {
            OnRoomEntryComplete?.Invoke(this);
        }

        public void NotifyPuzzleComplete()
        {
            OnRoomPuzzleComplete?.Invoke(this);
        }
    }
}