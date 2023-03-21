using UnityEngine;

namespace CEGI.Biblioteca
{
    public class DragaoController : MonoBehaviour
    {
        private Animator m_Animator = null;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        public void Speak()
        {
            m_Animator.SetTrigger("Speak");
        }

        public void StopSpeak()
        {
            m_Animator.SetTrigger("Idle");
        }

        public void Celebrate()
        {
            m_Animator.SetTrigger("Cheer");
        }
    }
}