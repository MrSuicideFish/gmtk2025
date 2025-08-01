using UnityEngine;

namespace _GAME.Gameplay
{
    public class Room : MonoBehaviour
    {
        public Transform RoomConnector;
        public ToggleInteractable EntranceDoor;
        public ToggleInteractable ExitDoor;
        public bool IsPuzzleComplete = false;
    }
}