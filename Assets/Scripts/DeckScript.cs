using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53]; 
    int currentIndex = 1;
    void Start()
    {
        GetCardValues();
    }

    void GetCardValues()
    {
        int num = 0;
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            //counts up to 52 to get the amount of cards in the deck split into suites
            num %= 13;
            //if there is a remander after num/13 
            //then the remander is the number assinged to the card
            //as its value due to not wanting a card to be 46 if the
            //index of the card is equal to 46
            if (num > 10 || num == 0)
            {
                num = 10;
            }

            cardValues[i] = num++;
        }
        
    }

    public void shuffle()
    {
        // Array swap method
        
        for (int i = cardSprites.Length - 1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f)* cardSprites.Length - 1) + 1;
            Sprite Face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = Face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }
        currentIndex = 1;
    }

    public int dealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex]);
        currentIndex++;
        return cardScript.GetValueofCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
