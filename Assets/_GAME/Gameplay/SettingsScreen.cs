using UnityEngine;
using UnityEngine.UI;

namespace _GAME.Gameplay
{
    public class SettingsScreen : MonoBehaviour
    {
        public Slider m_masterSlider;
        public Slider m_musicSlider;
        public Slider m_sfxSlider;
        
        private void OnEnable()
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();

            m_masterSlider.value = Mathf.InverseLerp(gameManager.minAudio, gameManager.maxAudio,
                PlayerPrefs.GetFloat("MasterVolume", 0));
            
            m_musicSlider.value = Mathf.InverseLerp(gameManager.minAudio, gameManager.maxAudio,
                PlayerPrefs.GetFloat("MusicVolume", -25f));
            
            m_sfxSlider.value = Mathf.InverseLerp(gameManager.minAudio, gameManager.maxAudio,
                PlayerPrefs.GetFloat("SfxVolume", -9f));
        }
    }
}