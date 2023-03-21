using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace CEGI.Biblioteca
{
    public class LivroMesa : MonoBehaviour, IBeginDragHandler, IDragHandler, 
        IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, 
        IPointerClickHandler, IGameplay, IPointerUpHandler
    {
        [SerializeField] private TextMeshPro NumberText = null;
        private int NumberRef = 0;

        private Vector3 StartPos = Vector3.zero;

        private Camera m_Camera = null;

        private SpriteRenderer spriteRenderer = null;
        private Animator animator = null;

        private bool IsDragging = false;
        private LevelController levelController = null;

        private AudioSource audioSource = null;
        [Header("Efeitos Sonoros")]
        [SerializeField] private AudioClip HoverClip = null;
        [SerializeField] private AudioClip ClickClip = null;
        [SerializeField] private AudioClip ReleaseClip = null;
        [SerializeField] private AudioClip RightPosClip = null;
        [SerializeField] private AudioClip WrongPosClip = null;
        [SerializeField] private AudioSource DraggingBookSource = null;

        public void SetNumber(int number)
        {
            NumberRef = number;
            NumberText.text = NumberRef.ToString("00");
            Activate();
        }

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            levelController = FindObjectOfType<LevelController>();

            NumberText.text = NumberRef.ToString("00");
            StartPos = transform.position;
            m_Camera = Camera.main;
        }

        private void SetPositionToCursor()
        {
            Vector3 targetPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = StartPos.z;

            // Aplica offset na nova posição
            transform.position = targetPos;// + new Vector3(Offset.x, Offset.y);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Blocked)
                return;

            /*IsDragging = true;

            SetPositionToCursor();
            DraggingBookSource.Play();

            spriteRenderer.sortingOrder = 15;
            NumberText.sortingOrder = 16;*/
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Blocked)
                return;

            if (!IsDragging)
                return;

            // Enquanto estiver fazendo drag, atualiza a posição do objeto no cenário
            SetPositionToCursor();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Blocked)
                return;

            CheckBook();

            ReturnToTable();
        }

        private void CheckBook()
        {
            Vector3 cursorPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            List<Collider2D> overlaps = new List<Collider2D>();
            overlaps.AddRange(Physics2D.OverlapPointAll(cursorPos));

            LivroEstante currentSpace = null;

            foreach(Collider2D coll in overlaps)
            {
                if (coll.CompareTag("Shelf Book"))
                    currentSpace = coll.GetComponent<LivroEstante>();

                if (currentSpace != null)
                    break;
            }

            if (currentSpace != null)
            {
                if (currentSpace.BookNumber == NumberRef)
                {
                    currentSpace.FillSpaceAsCorrect(spriteRenderer.sprite);
                    Deactivate();
                    audioSource.PlayOneShot(RightPosClip);
                }
                else
                {
                    currentSpace.FillSpaceAsWrong(spriteRenderer.sprite, NumberRef, this);
                    Deactivate();

                    // Posição errada!
                    levelController.WrongPosition();
                    audioSource.PlayOneShot(WrongPosClip);
                }
            }
            else
                audioSource.PlayOneShot(ReleaseClip);
        }

        public void Activate()
        {
            spriteRenderer.gameObject.SetActive(true);
        }
        private void Deactivate()
        {
            spriteRenderer.gameObject.SetActive(false);
        }

        #region Interações com mouse

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Blocked)
                return;

            animator.SetBool("Hover", true);
            audioSource.PlayOneShot(HoverClip);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Blocked)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
            {
                eventData.dragging = false;
                return;
            }

            IsDragging = true;
            SetPositionToCursor();
            DraggingBookSource.Play();

            spriteRenderer.sortingOrder = 15;
            NumberText.sortingOrder = 16;

            animator.SetBool("Press", true);

            audioSource.PlayOneShot(ClickClip);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (Blocked)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
            {
                eventData.dragging = false;
                return;
            }

            animator.SetBool("Press", false);
            ReturnToTable();
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (Blocked)
                return;

            if (IsDragging)
                return;

            animator.SetBool("Hover", false);
            animator.SetBool("Press", false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Blocked)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
            {
                eventData.dragging = false;
                return;
            }

            ReturnToTable();
        }

        public void ReturnToTable()
        {
            transform.position = StartPos;
            animator.CrossFade("Normal", 0);

            animator.SetBool("Hover", false);
            animator.SetBool("Press", false);
            animator.SetBool("Dragging", false);

            IsDragging = false;

            spriteRenderer.sortingOrder = 10;
            NumberText.sortingOrder = 11;

            DraggingBookSource.Stop();
        }


        #endregion

        private bool Blocked = false;
        public void BlockGameplay()
        {
            Blocked = true;
        }

        public void UnblockGameplay()
        {
            Blocked = false;
        }

    }
}