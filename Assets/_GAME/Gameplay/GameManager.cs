using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace _GAME.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private List<Room> m_roomPrefabs;
        [SerializeField] private int m_currentRoom;
        [SerializeField] private Canvas m_settingsCanvas;
        [SerializeField] private GameObject m_fpsHud;
        
        public AudioSource BGMAudioSource;

        private Player m_playerInst;
        private bool m_gameHasStarted;

        public void Start()
        {
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
#endif
      
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 0));
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", -25f));
            mixer.SetFloat("SfxVolume", PlayerPrefs.GetFloat("SfxVolume", -9f));
            PlayerPrefs.Save();
            
            m_currentRoom = 0;
            
            // load first room
            Vector3 FirstSpawnPos = new Vector3(-4.0f, 0f, 0f);

            m_currentRoom = 0;
            Room firstRoom = LoadRoom(m_currentRoom, FirstSpawnPos, false);
            firstRoom.PrevRoom = null;
            firstRoom.PlayerEnterRoomZone.gameObject.SetActive(false);
            firstRoom.OnRoomPuzzleComplete += OnRoomPuzzleComplete;
            firstRoom.OnRoomPuzzleIncomplete += OnRoomPuzzleIncomplete;
            OnRoomEntryComplete(firstRoom); // immediately enter first room

            // Spawn player
            m_playerInst = Instantiate(m_playerPrefab,
                new Vector3(-4.0f, 0.5f, 0.5f),
                Quaternion.Euler(0, 0, 0));


            m_playerInst.GetComponent<PlayerController>().InputEnabled = false;
        }

        public void StartGame()
        {
            Instantiate(m_fpsHud);
            m_gameHasStarted = true;
            m_playerInst.GetComponent<PlayerController>().InputEnabled = true;
        }

        public void EndGame()
        {
            BGMAudioSource.Stop();
        }

        public void ToggleSettingsMenu(bool value)
        {
            m_settingsCanvas.gameObject.SetActive(value);
        }

        public void PauseGame()
        {
            if (m_gameHasStarted)
            {
                if (m_playerInst != null)
                {
                    m_playerInst.GetComponent<PlayerController>().InputEnabled = false;    
                }
                
                ToggleSettingsMenu(true);
                Time.timeScale = 0;    
            }
        }

        public void ResumeGame()
        {
            if (!m_gameHasStarted)
            {
                return;
            }
            
            if (m_playerInst != null)
            {
                m_playerInst.GetComponent<PlayerController>().InputEnabled = true;    
            }
            
            ToggleSettingsMenu(false);
            Time.timeScale = 1.0f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_settingsCanvas.gameObject.activeInHierarchy)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void OnRoomPuzzleIncomplete(Room room)
        {
            // remove old next room (the duplicate of itself)
            if (room.NextRoom != null)
            {
                Destroy(room.NextRoom.gameObject);
            }
            
            Room nextPuzzle = LoadRoom(room.RoomIndex, room.RoomConnector.position, true);
            if(nextPuzzle == null)
            {
                return;
            }
            nextPuzzle.PrevRoom = room;
            nextPuzzle.OnRoomEntryComplete -= OnRoomEntryComplete;
            nextPuzzle.OnRoomEntryComplete += OnRoomEntryComplete;
            
            nextPuzzle.OnRoomPuzzleComplete -= OnRoomPuzzleComplete;
            nextPuzzle.OnRoomPuzzleComplete += OnRoomPuzzleComplete;
            
            nextPuzzle.OnRoomPuzzleIncomplete -= OnRoomPuzzleIncomplete;
            nextPuzzle.OnRoomPuzzleIncomplete += OnRoomPuzzleIncomplete;
            room.NextRoom = nextPuzzle;
        }

        public void OnRoomPuzzleComplete(Room room)
        {
            if (room.AutoLoadNextPuzzle)
            {
                if (room.NextRoom != null)
                {
                    Destroy(room.NextRoom.gameObject);
                }
                
                LoadNextPuzzle();
            }
        }

        public void DestroyOldPuzzle()
        {
            StartCoroutine(DestroyOldPuzzle_Impl());
        }

        IEnumerator DestroyOldPuzzle_Impl()
        {
            if (m_currentRoomInst.NextRoom != null)
            {
                Destroy(m_currentRoomInst.NextRoom.gameObject);
            }

            yield return null;
        }

        public void LoadNextPuzzle()
        {
            Room nextPuzzle = LoadRoom(m_currentRoomInst.RoomIndex + 1, m_currentRoomInst.RoomConnector.position, true);
            if(nextPuzzle == null)
            {
                return;
            }
            
            nextPuzzle.PrevRoom = m_currentRoomInst;
            nextPuzzle.OnRoomEntryComplete -= OnRoomEntryComplete;
            nextPuzzle.OnRoomEntryComplete += OnRoomEntryComplete;
            
            nextPuzzle.OnRoomPuzzleComplete -= OnRoomPuzzleComplete;
            nextPuzzle.OnRoomPuzzleComplete += OnRoomPuzzleComplete;
            
            nextPuzzle.OnRoomPuzzleIncomplete -= OnRoomPuzzleIncomplete;
            nextPuzzle.OnRoomPuzzleIncomplete += OnRoomPuzzleIncomplete;
            m_currentRoomInst.NextRoom = nextPuzzle;
        }

        private Room m_currentRoomInst;
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

            m_currentRoomInst = room;
            
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

                if (prevRoom != null 
                    && prevRoom.gameObject != null)
                {
                    Destroy(prevRoom.gameObject);    
                }
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

        public AudioMixer mixer;
        public float minAudio;
        public float maxAudio;
        public VolumeProfile volumeProfile;

        public void SetGamma(float value)
        {
            if (volumeProfile.TryGet(typeof(Exposure), out VolumeComponent exposureComponent))
            {
                var exposure = (Exposure)exposureComponent;
                exposure.compensation.value = Mathf.Lerp(-2.4f, -1.6f, value);
            }
            else
            {
                Debug.LogWarning("Exposure component not found in volume profile.");
            }
        }
        
        public void SetMaster(float value)
        {
            float newValue = Mathf.Lerp(minAudio, maxAudio, value);
            mixer.SetFloat("MasterVolume", newValue);
            PlayerPrefs.SetFloat("MasterVolume", newValue);
            PlayerPrefs.Save();
        }
        
        public void SetBGM(float value)
        {
            float newValue = Mathf.Lerp(minAudio, maxAudio, value);
            mixer.SetFloat("MusicVolume", newValue);
            PlayerPrefs.SetFloat("MusicVolume", newValue);
            PlayerPrefs.Save();
        }

        public void SetSFX(float value)
        {
            float newValue = Mathf.Lerp(minAudio, maxAudio, value);
            mixer.SetFloat("SfxVolume", newValue);
            PlayerPrefs.SetFloat("SfxVolume", newValue);
            PlayerPrefs.Save();
        }
    }
}