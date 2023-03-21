using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CEGI.Biblioteca
{
    public class LevelController : MonoBehaviour
    {
        // Todas as possíveis sequências que pdoems er escolhidas pelo sistema para a fase
        [SerializeField] private SequenceData[] SequenceLevels = null;
        [Space]
        [SerializeField] private Animator CameraAnimator = null;

        // Armazena sequências que já foram sorteadas anteriormente
        private List<SequenceData> usedSequences = new List<SequenceData>();
        private Estante Shelf = null;

        private List<int> missingValues = new List<int>();
        private LivroMesa[] LivrosMesa = null;

        private LifeController lifeController = null;
        private bool firstError = false;

        // Referências de fala e dragão
        private BalloonController ballon = null;
        private DragaoController dragao = null;
        [Space]
        [SerializeField] private Animator m_TransitionAnimator = null;
        [SerializeField] private Text CompletedField = null;
        [Space]
        [SerializeField] private UnityEvent OnCompleteLevel = null;
        [SerializeField] private UnityEvent OnLooseLevel = null;
        [Space]
        [SerializeField] private UnityEvent OnEstanteCompleted = null;

        private int stepsCompleted = 0;
        private IEnumerable<IGameplay> Gameplays = null;
        private List<Selectable> AllButtons = new List<Selectable>();

        private void Start()
        {
            Shelf = FindObjectOfType<Estante>();
            LivrosMesa = FindObjectsOfType<LivroMesa>();
            lifeController = FindObjectOfType<LifeController>();

            ballon = FindObjectOfType<BalloonController>();
            dragao = FindObjectOfType<DragaoController>();

            // Todos gameplay itnerface
            Gameplays = FindObjectsOfType<MonoBehaviour>().OfType<IGameplay>();
            Selectable[] selectables = FindObjectsOfType<Selectable>();
            foreach(Selectable s in selectables)
            {
                if (s.CompareTag("ButtonCanBeBlocked"))
                    AllButtons.Add(s);
            }

            InitializeLevel();
        }

        //Ao interagir com botão tutorial
        public void Tutorial()
        {
            dragao.Speak();
            ballon.Speak("COLOQUE OS LIVROS DA MESA NA ESTANTE ARRASTANDO-OS. COMPLETE AS FILEIRAS SEGUINDO A ORDEM CORRESPONDENTE. AO COLOCAR O LIVRO NO LUGAR INCORRETO, VOCÊ PERDERÁ UMA VIDA.");
            return;

        }

        #region Controla inicialização de fase e reinício de fases

        /// <summary>
        /// Chamado toda vez que uma nova sequência deve ser preenchida
        /// </summary>
        private void InitializeLevel()
        {
            missingValues.Clear();

            RandomAndSetSequence();

            SetBooksNumberOnTable();
        }

        /// <summary>
        /// Sorteia uma sequência para ser utilizada
        /// </summary>
        private void RandomAndSetSequence()
        {
            if (Shelf == null)
                return;

            int i = Random.Range(0, SequenceLevels.Length);

            // Continua sorteando até encontrar uma sequência não utilizada
            while(usedSequences.Contains(SequenceLevels[i]))
                i = Random.Range(0, SequenceLevels.Length);

            // Envia informação da sequência apra a estante
            Shelf.FillEstante(SequenceLevels[i]);

            // Adiciona a sequência na lista de sequências já utilizadas
            usedSequences.Add(SequenceLevels[i]);

            // Reseta a lista de já utilizados, caso todos já tenham sido utilizados
            if (usedSequences.Count == SequenceLevels.Length)
                usedSequences.Clear();
        }

        private void SetBooksNumberOnTable()
        {
            int j = 0;
            List<int> valuesToUse = new List<int>(missingValues);
            while(valuesToUse.Count > 0 && j < LivrosMesa.Length)
            {
                int i = Random.Range(0, valuesToUse.Count);
                LivrosMesa[j].SetNumber(valuesToUse[i]);
                valuesToUse.RemoveAt(i);
                j++;
            }
        }

        public bool AddMissingNumber(int number)
        {
            if (missingValues.Contains(number))
                return false;

            missingValues.Add(number);
            return true;
        }

        public void RestartLevel()
        {

            // Retorna livros para mesa
            for (int i = 0; i < LivrosMesa.Length; i++)
                LivrosMesa[i].Activate();

            // Reseta Estante
            Shelf.RestartEstante(missingValues);

            // Reseta Vida
            lifeController.Start();

            //////////////////////////
            SetBooksNumberOnTable();


        }

        #endregion

        #region Condições de vitória e derrota

        public void CompleteLevel()
        {
            stepsCompleted++;
            CompletedField.text = stepsCompleted + "/5";
            if(stepsCompleted >= 5)
            {
                CompleteDificulty();
                return;
            }

            dragao.Celebrate();
            OnEstanteCompleted.Invoke();
            m_TransitionAnimator.SetTrigger("Show");
            Invoke("NextLevel", 5.5f);

            //OnCompleteLevel.Invoke();
        }

        private void NextLevel()
        {
            InitializeLevel();
            m_TransitionAnimator.SetTrigger("Hide");
            lifeController.AddLife();
        }

        public void LooseLevel()
        {
            BlockGameplay();
            OnLooseLevel.Invoke();
        }

        public void CompleteDificulty()
        {
            BlockGameplay();
            OnCompleteLevel.Invoke();
        }

        public void BlockGameplay()
        {
            foreach (IGameplay obj in Gameplays)
                obj.BlockGameplay();

            foreach (Selectable s in AllButtons)
                s.interactable = false;
        }

        public void UnblockGameplay()
        {
            foreach (IGameplay obj in Gameplays)
                obj.UnblockGameplay();

            foreach (Selectable s in AllButtons)
                s.interactable = true;
        }

        #endregion

        public void WrongPosition()
        {
            // Bloquear inserção de novos livros
            foreach(LivroMesa l in LivrosMesa)
                l.enabled = false;
            

            if (!firstError)
            {
                firstError = true;

                // Mostrar balão de fala
                dragao.Speak();
                ballon.Speak("O LIVRO NÃO FOI COLOCADO NA POSIÇÃO CORRETA. PARA CONTINUAR, CLIQUE NO LIVRO PARA REMOVÊ-LO DA ESTANTE.");

                return;
            }

            lifeController.RemoveLife();
            CameraAnimator.SetTrigger("Shake");
        }

        public void WrongBookRemoved()
        {
            // Bloquear inserção de novos livros
            foreach (LivroMesa l in LivrosMesa)
                l.enabled = true;

        }
    }
}