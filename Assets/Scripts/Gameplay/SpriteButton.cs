using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CEGI.Biblioteca
{
    [RequireComponent(typeof(Animator))]
    public class SpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField] private UnityEvent OnClick = null;

        private Animator animator = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            animator.SetBool("Hover", true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            animator.SetBool("Press", true);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
            animator.SetBool("Press", false);
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetBool("Hover", false);
            animator.SetBool("Press", false);
        }



        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}