using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CEGI.Biblioteca
{
    public class LifeController : MonoBehaviour
    {
        [SerializeField] private Image[] LivesImages = null;
        [Space]
        [SerializeField] private Sprite lifeOn = null;
        [SerializeField] private Sprite lifeOff = null;
        [Space]
        [SerializeField] private ParticleSystem BreakHeartEffect = null;
        [Space]
        [SerializeField] private LevelController levelController = null;

        private int MaxLives = 3;
        private int CurrentLives = 3;

        public void Start()
        {
            CurrentLives = MaxLives;
            levelController = FindObjectOfType<LevelController>();
            UpdateLives();
        }

        public void RemoveLife()
        {
            CurrentLives--;
            UpdateLives();
            // Spawn break heart
            //Vector3 targetPos = Camera.main.ScreenToWorldPoint(LivesImages[CurrentLives].transform.position);
            Vector3 targetPos = LivesImages[CurrentLives].transform.position;
            targetPos.z = -2;

            BreakHeartEffect.transform.position = targetPos;
            BreakHeartEffect.gameObject.SetActive(true);
            BreakHeartEffect.Play(true);

            if (CurrentLives <= 0)
            {
                print("Current Lives: " + CurrentLives);
                print("Lose");
                levelController.LooseLevel();
            }

            // Se for zerada, aparecer mensagem de derrota
        }

        public void AddLife()
        {
            MaxLives++;
            if (MaxLives >= 6)
                MaxLives = 6;

            CurrentLives++;
            if (CurrentLives > MaxLives)
                CurrentLives = MaxLives;
            
            UpdateLives();
        }

        /// <summary>
        /// Atualiza visualização de vidas na fase
        /// </summary>
        private void UpdateLives()
        {
            for (int i = 0; i < LivesImages.Length; i++)
            {
                // Se for índice maior que máximo de vidas, deixar coração invisível
                if (i >= MaxLives)
                {
                    LivesImages[i].enabled = false;
                    continue;
                }

                // Torna imagem visível e em seguida verifica se tem a vida ou não
                LivesImages[i].enabled = true;
                LivesImages[i].sprite = i < CurrentLives ? lifeOn : lifeOff;
            }
        }
    }
}