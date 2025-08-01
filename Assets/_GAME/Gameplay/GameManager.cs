using System.Collections.Generic;
using UnityEngine;

namespace _GAME.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private List<Room> m_roomPrefabs;
        [SerializeField] private int m_currentRoom;

        private Player m_playerInst;

        private Room m_currentRoomInst;
        private Room m_nextRoomInst, m_prevRoomInst;

        public void Start()
        {
            m_currentRoom = 0;
            
            // load first room
            Vector3 FirstSpawnPos = new Vector3(-4.0f, 0f, 0f);
            m_currentRoomInst = LoadRoom(0, FirstSpawnPos);
            m_nextRoomInst = LoadRoom(0, m_currentRoomInst.RoomConnector.position);
            m_nextRoomInst.EntranceDoor.Hide();

            m_playerInst = Instantiate(m_playerPrefab,
                new Vector3(-4.0f, 0.5f, 0.5f),
                Quaternion.Euler(0, 0, 0));
        }

        private void ProgressPuzzle(int puzzle)
        {
            if (m_currentRoomInst == null || m_currentRoomInst.IsPuzzleComplete)
            {
                return;
            }

            // Check if the puzzle is complete
            if (puzzle == m_currentRoom && !m_currentRoomInst.IsPuzzleComplete)
            {
                m_currentRoomInst.IsPuzzleComplete = true;
                Debug.Log($"Puzzle {puzzle} completed in room {m_currentRoom}.");
                
                // Load next room if available
                if (m_nextRoomInst != null)
                {
                    m_prevRoomInst = m_currentRoomInst;
                    m_currentRoomInst = m_nextRoomInst;
                    m_nextRoomInst = LoadRoom(m_currentRoom + 1, m_currentRoomInst.RoomConnector.position);
                    m_currentRoom++;
                }
            }
        }
        
        Room LoadRoom(int roomIndex, Vector3 spawnPos)
        {
            Room roomToLoad = m_roomPrefabs[roomIndex];
            return Instantiate<Room>(roomToLoad, spawnPos, Quaternion.identity);
        }
    }
}