using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEGI.Biblioteca
{
    [CreateAssetMenu]
    public class BookSpriteSelector : ScriptableObject
    {
        [SerializeField] private List<Sprite> BookOnTable = new List<Sprite>();
        [SerializeField] private List<Sprite> BookOnShelf = new List<Sprite>();

        public Sprite GetBookSpriteForShelf(Sprite bookFromTable)
        {
            int i = BookOnTable.FindIndex(x => x == bookFromTable);

            return BookOnShelf[i];
        }

        public Sprite GetRandomTableSprite()
        {
            return BookOnTable[Random.Range(0, BookOnTable.Count)];
        }
        public Sprite GetRandomShelfSprite()
        {
            return BookOnShelf[Random.Range(0, BookOnShelf.Count)];
        }
    }
}