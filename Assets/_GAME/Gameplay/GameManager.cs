using System.Collections.Generic;
using UnityEngine;

namespace _GAME.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private List<Room> m_roomPrefabs;
        [SerializeField] private int m_currentRoom;

        private Dictionary<int, Room> m_roomInstances;
        private Player m_playerInst;

        public void Start()
        {
            m_currentRoom = 0;
            m_roomInstances = new Dictionary<int, Room>();
            
            for(int i = 0; i < m_roomPrefabs.Count; i++)
            {
                LoadRoom(i);
            }

            m_playerInst = Instantiate(m_playerPrefab,
                new Vector3(-4.0f, 0.5f, 0.5f),
                Quaternion.Euler(0, 0, 0));
        }
        
        void LoadRoom(int roomIndex)
        {
            Room roomToLoad = m_roomPrefabs[roomIndex];
            
            Vector3 spawnPos = new Vector3();
            if (roomIndex == 0)
            {
                spawnPos = new Vector3(-4.0f, 0f, 0f);
            }
            else
            {
                spawnPos = m_roomInstances[roomIndex - 1].RoomConnector.position;
            }
            
            var inst = Instantiate<Room>(roomToLoad, spawnPos, Quaternion.identity);
            m_roomInstances.Add(roomIndex, inst);
        }
        
        void UnloadRoom(int roomIndex)
        {
            Room roomToUnload = m_roomInstances[roomIndex];
            if (roomToUnload != null)
            {
                Destroy(roomToUnload.gameObject);
                m_roomInstances.Remove(roomIndex);
            }
        }
    }
}