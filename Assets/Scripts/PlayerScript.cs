using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Console;

public class PlayerScript : MonoBehaviour
{
    // --- Used both by the dealer and player
    
    //Getting other scripts/code
    public CardScript cardScript;
    public DeckScript deckScript;
    
    //Total value of hand
    public int handValue = 0;
    
    // Betting money
    private int money = 1000;
    
    // Array of card objects on the table
    public GameObject[] hand;
    
    //Index of next card to be turned over
    public int cardIndex = 1;
    
    //Since aces can both be 1 and 11 which can change at any moment this is needed
    private List<CardScript> aceList = new List<CardScript>();
    
    public void StartHand()
    {
        GetCard();
        GetCard();
    }
    
    //add a card to the player/dealer's hand
    public int GetCard()
    {
        //Gets card value and sprite
        Debug.Log(cardIndex);
        int cardValue = deckScript.dealCard(hand[cardIndex].GetComponent<CardScript>());
        
        //shows card
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        //Adds card to running total in one's hand
        handValue += cardValue;
        if (cardValue == 1)
        {
            aceList.Add(hand[cardValue].GetComponent<CardScript>());
        }

        AceCheck();
        cardIndex++;
        return handValue;
    }

    public void AceCheck()
    {
        foreach (CardScript ace in aceList)
        {
            if (handValue + 10 < 22 && ace.GetValueofCard() == 1)
            {
               ace.SetValue(11);
               handValue += 10;
            }
            else if (handValue > 21 && ace.GetValueofCard() == 11)
            {
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }
    
    //For bets
    public void AdjustMoney(int amount)
    {
        money += amount;
    }
    
    //output player current money amount
    public int GetMoney()
    {
        return money;
    }

    public void ResetHand()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }
}
