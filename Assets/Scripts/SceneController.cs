using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 3;
    public const int gridCols = 6;
    public const float offsetX = 2.5f;
    public const float offsetY = 2.5f;

    private int score = 0;

    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;
    [SerializeField] TMP_Text scoreLabel;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;

    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    void Start()
    {
        Vector3 startPos = originalCard.transform.position;

        //make array 0-21 for each sprite
        int[] numbers = new int[22];
        for (int i = 0; i < 22; i++)
        {
            numbers[i] = i;
        }

        //shuffle and get two copies of the first 9
        numbers = ShuffleArray(numbers);
        int[] selectedNumbers = new int[18];
        for (int i = 0; i < 18; i += 2)
        {
            selectedNumbers[i] = numbers[i];
            selectedNumbers[i + 1] = numbers[i];
        }

        //shuffle again
        selectedNumbers = ShuffleArray(selectedNumbers);

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
                int id = selectedNumbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.x);
            }
        }
    }

    public void CardRevealed(MemoryCard card)
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
            scoreLabel.text = $"Score: {score}";
        }
        else
        {
            yield return new WaitForSeconds(.5f);

            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }
        firstRevealed = null;
        secondRevealed = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene");
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
