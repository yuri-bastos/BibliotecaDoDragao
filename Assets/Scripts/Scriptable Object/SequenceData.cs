using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEGI.Biblioteca
{
    [CreateAssetMenu]
    public class SequenceData : ScriptableObject
    {
        [SerializeField] private List<Prateleira> SequenceMatrix;
        [SerializeField] private int[] MissingBooks = { 3, 4, 3, 4 };

        public int GetMissingBooks(int index)
        {
            return MissingBooks[index];
        }

        public int GetMaxPrat()
        {
            return SequenceMatrix.Count;
        }

        public int[] GetPrateleiraSequence(int pratIndex)
        {
            if (pratIndex >= SequenceMatrix.Count)
                return null;

            return SequenceMatrix[pratIndex].sequence;
        }

        public int GetValue(int x, int y)
        {
            if (x >= 10 || y >= SequenceMatrix.Count)
                return -1;

            return SequenceMatrix[y].sequence[x];
        }
    }

    [System.Serializable]
    public class Prateleira
    {
        public int[] sequence; 
    }
}