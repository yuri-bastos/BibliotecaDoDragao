using UnityEngine;

namespace CEGI.Biblioteca
{
    public enum BookType
    {
        Table, Shelf
    }

    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private BookSpriteSelector SpritesData = null;
        [SerializeField] private BookType bookType = BookType.Table;

        private void Start()
        {
            if(spriteRenderer == null)
            {
                Debug.LogError("Não há um sprite renderer associado! Selecione qual sprite renderer quer que altere o sprite");
                return;
            }

            // Sorteia um sprite entre os sprites
            spriteRenderer.sprite = bookType == BookType.Table ? SpritesData.GetRandomTableSprite() : SpritesData.GetRandomShelfSprite();
        }

    }
}