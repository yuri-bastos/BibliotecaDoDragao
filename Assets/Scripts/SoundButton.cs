using UnityEngine;
using UnityEngine.UI;

namespace CEGI.Biblioteca
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private Image SoundOnImage = null;
        [SerializeField] private Image SoundOffImage = null;

        private void Start()
        {
            UpdateSoundSprite();
        }

        private void UpdateSoundSprite()
        {
            SoundOnImage.enabled = AudioListener.volume == 1;
            SoundOffImage.enabled = AudioListener.volume == 0;
        }


        /// <summary>
        /// Troca a situação do som apra ligado ou desligado
        /// </summary>
        public void SwitchSound()
        {
            if (AudioListener.volume == 1)
                AudioListener.volume = 0;
            else
                AudioListener.volume = 1;

            UpdateSoundSprite();
        }
    }
}