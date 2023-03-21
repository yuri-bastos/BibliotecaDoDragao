using System;
using UnityEngine;
using System.Collections.Generic;

namespace CEGI.Biblioteca
{
    public class Fileira : MonoBehaviour
    {
        [SerializeField] private LivroEstante[] Livros = null;
        [Space]
        [SerializeField] private ParticleSystem[] CompleteStars = null;

        private LevelController levelController = null;

        public bool Completed { get; private set; } = false;
        private AudioSource audioSource = null;

        public event Action OnFileiraComplete = null;

        private void Awake()
        {
            levelController = FindObjectOfType<LevelController>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            foreach (LivroEstante livro in Livros)
                livro.OnSpaceFilled += CheckFileira;
        }

        private void OnDisable()
        {
            foreach (LivroEstante livro in Livros)
                livro.OnSpaceFilled -= CheckFileira;
        }

        public void SetSequence(int[] sequence, int emptySpaces, SideType sideType)
        {
            for (int i = 0; i < Livros.Length && i < sequence.Length; i++)
            {
                Livros[i].SetCorrectNumber(sequence[i]);
                Livros[i].IsEmpty = false;
                Livros[i].UpdateVisibility();
            }

            foreach (ParticleSystem ps in CompleteStars)
            {
                ps.Stop(true);
                ps.Clear(true);
            }

            // Sortear os espaços que serão visíveis ou não
            bool side = true;
            while(emptySpaces > 0)
            {
                switch (sideType)
                {
                    case SideType.PreserveBothSides:
                        RandomEmpty(1, 9);
                        break;
                    case SideType.PreserveOneSide:

                        if (side)
                            RandomEmpty(0, 8);
                        else
                            RandomEmpty(1, 10);

                        side = !side;
                        break;
                    case SideType.DontPreserve:
                        RandomEmpty(0, 10);
                        break;
                    default:
                        break;
                }

                emptySpaces--;
            }
        }

        private void RandomEmpty(int min, int max)
        {
            Completed = false;  

            int i = UnityEngine.Random.Range(min, max);

            while(!levelController.AddMissingNumber(Livros[i].BookNumber))
                i = UnityEngine.Random.Range(min, max);

            Livros[i].IsEmpty = true;
            Livros[i].UpdateVisibility();
        }

        // Ao reiniciar fase, reseta os livros que estavam faltando
        public void ResetEmptyBooks(List<int> missing)
        {
            foreach (LivroEstante livro in Livros)
            {
                if (missing.Contains(livro.BookNumber))
                {
                    livro.IsEmpty = true;
                    livro.UpdateVisibility();
                }
            }
        }

        public void updateFileira()
        {
            Completed = false;
        }

        public void CheckFileira()
        {
            foreach (LivroEstante livro in Livros)
            {
                if (livro.IsEmpty || livro.IsWrong)
                    return;
            }

            Completed = true;

            foreach (ParticleSystem ps in CompleteStars)
                ps.Play(true);

            audioSource.Play();
            OnFileiraComplete?.Invoke();
        }
    }
}