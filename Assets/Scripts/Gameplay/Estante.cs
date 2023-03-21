using UnityEngine;
using System.Collections.Generic;

namespace CEGI.Biblioteca
{
    public enum SideType
    {
        PreserveBothSides, PreserveOneSide, DontPreserve
    }

    public class Estante : MonoBehaviour
    {
        [SerializeField] private SideType PreserveSideType = SideType.PreserveBothSides;
        [SerializeField] private Fileira[] Fileiras = null;

        private LevelController levelController = null;

        private void Start()
        {
            levelController = FindObjectOfType<LevelController>();
        }

        private void OnEnable()
        {
            foreach (Fileira f in Fileiras)
                f.OnFileiraComplete += CheckFileiras;
        }
        private void OnDisable()
        {
            foreach (Fileira f in Fileiras)
                f.OnFileiraComplete -= CheckFileiras;
        }

        public void FillEstante(SequenceData sequenceData)
        {
            for(int i=0; i < sequenceData.GetMaxPrat(); i++)
            {
                // Preenche os livros com o número correto da sequência
                Fileiras[i].SetSequence(sequenceData.GetPrateleiraSequence(i), 
                                        sequenceData.GetMissingBooks(i), 
                                        PreserveSideType);
            }
        }

        public void RestartEstante(List<int> missingBooks)
        {
            for (int i = 0; i < Fileiras.Length; i++)
            {
                Fileiras[i].ResetEmptyBooks(missingBooks);
                Fileiras[i].updateFileira();
            }
        }

        public void CheckFileiras()
        {
            foreach (Fileira f in Fileiras)
            {
                if (!f.Completed)
                    return;
            }

            levelController.CompleteLevel();
        }
    }
}