using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 3;
    public const int gridCols = 6;
    public const float offsetX = 2.5f;
    public const float offsetY = 2.5f;

    private int score = 0;

    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;

    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    public void CardRevealed (MemoryCard card)
    {
        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (firstRevealed.Id == secondRevealed.Id)
        {
            score++;
            Debug.Log($"Score: {score}");
        }
        else {
            yield return new WaitForSeconds(.5f);

            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }
        firstRevealed = null;
        secondRevealed = null;
    }

    void Start()
    {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8};
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.x);
            }
        }
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
}
