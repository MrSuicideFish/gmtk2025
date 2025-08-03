using _GAME.Gameplay;
using UnityEngine;

public class CompletePuzzleZone : MonoBehaviour
{
    public Room TargetRoom;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TargetRoom.NotifyPuzzleComplete();
        }
    }
}
