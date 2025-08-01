using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport.PostProcessing;

namespace _GAME.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private List<Room> m_roomPrefabs;
        [SerializeField] private int m_currentRoom;
        [SerializeField] private Canvas m_gameOverCanvas;

        private Player m_playerInst;

        public void Start()
        {
            m_currentRoom = 0;
            
            // load first room
            Vector3 FirstSpawnPos = new Vector3(-4.0f, 0f, 0f);

            m_currentRoom = 0;
            Room firstRoom = LoadRoom(m_currentRoom, FirstSpawnPos, false);
            firstRoom.PrevRoom = null;
            firstRoom.PlayerEnterRoomZone.gameObject.SetActive(false);
            firstRoom.OnRoomPuzzleComplete += OnRoomPuzzleComplete;
            OnRoomEntryComplete(firstRoom); // immediately enter first room

            // Spawn player
            m_playerInst = Instantiate(m_playerPrefab,
                new Vector3(-4.0f, 0.5f, 0.5f),
                Quaternion.Euler(0, 0, 0));
        }

        public void OnRoomPuzzleComplete(Room room)
        {
            // remove old next room (the duplicate of itself)
            if (room.NextRoom != null)
            {
                Destroy(room.NextRoom.gameObject);
            }

            Room nextPuzzle = LoadRoom(room.RoomIndex + 1, room.RoomConnector.position, true);
            if(nextPuzzle == null)
            {
                m_gameOverCanvas.gameObject.SetActive(true);
                return;
            }
            
            nextPuzzle.PrevRoom = room;
            nextPuzzle.OnRoomEntryComplete += OnRoomEntryComplete;
            nextPuzzle.OnRoomPuzzleComplete += OnRoomPuzzleComplete;
            room.NextRoom = nextPuzzle;
        }

        private async void OnRoomEntryComplete(Room room)
        {
            StartCoroutine(RoomEntryAsync(room));
        }
        
        private IEnumerator RoomEntryAsync(Room room)
        {
            if (room.NextRoom != null)
            {
                Destroy(room.NextRoom.gameObject);
            }
            
            // load the same room for next (for now)
            room.NextRoom = LoadRoom(room.RoomIndex, room.RoomConnector.position, true);
            room.NextRoom.PrevRoom = room;
            
            Room prevRoom = room.PrevRoom;
            if (prevRoom != null && prevRoom.ExitDoor != null)
            {
                room.PrevRoom.ExitDoor.Toggle(false);
                room.PrevRoom.ExitDoor.ToggleInteractable(false);

                if (room.EntranceDoor != null)
                {
                    room.EntranceDoor.ToggleInteractable(false);    
                }
                
                yield return new WaitForSeconds(3);
                
                if (room.EntranceDoor != null)
                {
                    room.EntranceDoor.Show();    
                }
                
                Destroy(prevRoom.gameObject);
            }
            
            room.NextRoom.OnRoomEntryComplete += OnRoomEntryComplete;
            room.NextRoom.OnRoomPuzzleComplete += OnRoomPuzzleComplete;
            
        }

        Room LoadRoom(int roomIndex, Vector3 spawnPos, bool hideEntrance)
        {
            if( roomIndex < 0 || m_roomPrefabs.Count <= roomIndex)
            {
                return null;
            }
            
            Room room = m_roomPrefabs[roomIndex];
            room = Instantiate<Room>(room, spawnPos, Quaternion.identity);
            room.RoomIndex = roomIndex;

            if (hideEntrance && room.EntranceDoor != null) 
            {
                room.EntranceDoor.Hide();    
            }
            
            return room;
        }
    }
}