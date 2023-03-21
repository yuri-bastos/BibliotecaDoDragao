using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CEGI
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_FadeCanvasGroup = null;          // Referência ao grupo que faz transição preta entre cenas
        [SerializeField] private float m_TransitionDuration = 1f;               // Duração da transição do fade preto


        // Variáveis internas de controle
        private bool IsFading = false;                      // Controla se o sistema já está fazendo transição de cena, evitando duplo carregamento

        // Delegates events para transição de cenas
        public event Action BeforeLoad, AfterLoad; // Todos esses eventos poderão ser úteis para possíveis salvamentos futuros

        // --------------------------------------------
        // Variáveis para serem acessadas externamente
        // ---------------------------------------------

        public static SceneController Instance;             // Singleton para possibilitar rápido carregamento de nvoas cenas

        private float DeltaTime { get { return Time.timeScale < 0.1f ? Time.unscaledDeltaTime : Time.deltaTime; } }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                DestroyImmediate(gameObject);
        }

        private void Start()
        {
            m_FadeCanvasGroup.alpha = 1f;
            StartCoroutine(Fade(0));

        }

        /// <summary>
        /// Carrega uma cena nova. Se houver cena carregada, descarrega a cena atual primeiramente
        /// </summary>
        /// <param name="targetScene"></param>
        public void LoadScene(string targetScene)
        {
            if (!IsFading)
                StartCoroutine(LoadSceneAdditive(targetScene));

        }

        public void RestartScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Co-rotina que carrega uma nova cena, e caso haja uma carregada, descarrega a mesma.
        /// </summary>
        /// <param name="targetScene"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAdditive(string sceneName)
        {
            // Bloqueia novas transições até que esta finalize
            IsFading = true;

            // Escurece o jogo para fazer a transição
            yield return Fade(1f);

            BeforeLoad?.Invoke();

            SceneManager.LoadScene(sceneName);

            AfterLoad?.Invoke();


            yield return new WaitForSeconds(0.1f);


            // Remover fade preto
            yield return StartCoroutine(Fade(0f));

            // Libera sistema para novas transições
            IsFading = false;
        }

        /// <summary>
        /// Usado para realizar o fade preto tanto para escurecer como para clarear
        /// </summary>
        /// <param name="targetAlpha">Alpha desejado para o fade (1: escurece, 0:clareia)</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)
        {
            // Calcula a quantidade de alpha para ser alterado por frame
            float alphaStep = Mathf.Abs(targetAlpha - m_FadeCanvasGroup.alpha) / m_TransitionDuration;
            m_FadeCanvasGroup.blocksRaycasts = true;    // Bloquei cliques em botões

            while (!Mathf.Approximately(m_FadeCanvasGroup.alpha, targetAlpha))
            {
                // Alterando alpha utilizando unscaled time para evitar que a transição funcione mesmo com o jogo pausado
                m_FadeCanvasGroup.alpha = Mathf.MoveTowards(m_FadeCanvasGroup.alpha, targetAlpha,
                    alphaStep * DeltaTime);

                yield return null;
            }

            m_FadeCanvasGroup.blocksRaycasts = false;   // Permite cliques em botões novamente
        }

    }
}