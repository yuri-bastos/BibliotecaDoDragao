using UnityEngine;
using TMPro;

namespace CEGI.Biblioteca
{
    public class BalloonController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro m_SpeechText = null;

        private Animator m_Animator = null;
        private LevelController levelController = null;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            levelController = FindObjectOfType<LevelController>();
        }

        public void Speak(string message)
        {
            m_Animator.SetTrigger("Show");
            m_SpeechText.text = message;
            levelController.BlockGameplay();
        }

        public void CloseBallon()
        {
            m_Animator.SetTrigger("Hide");
            levelController.UnblockGameplay();
        }
    }
}