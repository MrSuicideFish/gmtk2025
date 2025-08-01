using UnityEngine;

namespace _GAME.Gameplay
{
    public class Room : MonoBehaviour
    {
        public Transform RoomConnector;
        public Interactable EntranceDoor;
        public Interactable ExitDoor;
        public bool IsPuzzleComplete = false;
    }
}