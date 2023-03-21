using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace CEGI.Biblioteca
{
    public class LivroEstante : MonoBehaviour, IPointerClickHandler, IGameplay
    {
        public bool IsEmpty = false;
        [SerializeField] private SpriteRenderer EmptySpriteRenderer = null;
        [SerializeField] private BookSpriteSelector SpritesData = null;
        [Space]
        [SerializeField] private TextMeshPro NumberField = null;
        [Space]
        [SerializeField] private SpriteRenderer Contorno = null;
        [SerializeField] private TextMeshPro Interrogacao = null;

        // Componentes
        private SpriteRenderer SpriteRenderer = null;
        private Collider2D Collider = null;
        private Camera m_Camera = null;
        private Animator m_Animator = null;
        private LevelController levelController = null;

        private int NumberRef = 0;
        private int numberToUse = 0;

        private float initialScale = 1f;
        private bool interact = false;

        public int BookNumber { get { return NumberRef; } }

        public event Action OnSpaceFilled = null;

        public bool IsWrong { get; private set; } = true;
        private LivroMesa LivroMesaRef = null;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<Collider2D>();
            m_Animator = GetComponent<Animator>();
            levelController = FindObjectOfType<LevelController>();

            m_Camera = Camera.main;

            initialScale = transform.localScale.x;

            UpdateVisibility();
        }

        public void UpdateVisibility()
        {
            SpriteRenderer.enabled = !IsEmpty;
            NumberField.enabled = !IsEmpty;

            Collider.enabled = IsEmpty || IsWrong;
            EmptySpriteRenderer.enabled = IsEmpty;
            Interrogacao.enabled = IsEmpty;

            NumberField.text = numberToUse.ToString("00");
        }

        private void SetWrong(bool wrong)
        {
            IsWrong = wrong;
            Contorno.enabled = wrong;
            m_Animator.SetBool("Wrong", IsWrong);
        }

        public void SetCorrectNumber(int number)
        {
            NumberRef = number;
            numberToUse = number;

            SetWrong(false);

            UpdateVisibility();
        }

        public void Hover()
        {
            if (!IsEmpty) return;

            transform.localScale = Vector3.one * initialScale * 1.25f;
        }

        public void Idle()
        {
            if (!IsEmpty) return;

            transform.localScale = Vector3.one * initialScale ;
        }

        private void FillSpace(Sprite bookSprite)
        {
            if (!IsEmpty) return;

            Idle();
            IsEmpty = false;

            SpriteRenderer.sprite = SpritesData.GetBookSpriteForShelf(bookSprite);
            UpdateVisibility();

            OnSpaceFilled?.Invoke();
        }

        public void FillSpaceAsCorrect(Sprite bookSprite)
        {
            numberToUse = NumberRef;
            SetWrong(false);
            FillSpace(bookSprite);
        }

        public void FillSpaceAsWrong(Sprite bookSprite, int number, LivroMesa livroMesa)
        {
            numberToUse = number;
            SetWrong(true);
            FillSpace(bookSprite);
            LivroMesaRef = livroMesa;
        }

        private void Update()
        {
            if (IsEmpty)
            {
                if (!interact)
                {
                    Idle();
                    return;
                }

                Vector3 cursorPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                if (Collider.OverlapPoint(cursorPos))
                    Hover();
                else
                    Idle();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Table Book"))
                interact = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Table Book"))
                interact = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Blocked)
                return;

            if (IsWrong)
            {
                IsEmpty = true;
                SetWrong(false);
                UpdateVisibility();

                if (LivroMesaRef != null)
                    LivroMesaRef.Activate();

                levelController.WrongBookRemoved();
            }
        }

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