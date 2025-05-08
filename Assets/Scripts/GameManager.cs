using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Game Buttons
    public Button deal;
    public Button Stand;
    public Button Hit;
    public Button Bet;
    private int standClicks = 0;
    
    // Access the player/dealer's hand
    public PlayerScript playerScript;
    public PlayerScript dealerScript;
    
    // public Text to access and update - hud
    public Text scoreText;
    public Text dealerScoreText;
    public Text betsText;
    public Text cashText;
    public Text mainText;
    public Text standText;
    
    //Card hiding dealer's 2nd card
    public GameObject hideCard;
    
    // how much the bet is
    private int pot = 0;
    
    void Start()
    {
        // Click on the buttons to make sure they do their job
        deal.onClick.AddListener(() => DealClicked());
        Hit.onClick.AddListener(() => HitClicked());
        Stand.onClick.AddListener(() => StandClicked());
        Bet.onClick.AddListener(() => BetClicked());
    }

    private void DealClicked()
    {
        //Reset round
        playerScript.ResetHand();
        dealerScript.ResetHand();
        
        //Hide deal Hand score at start of deal
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        
        //Update the score
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        
        //Hide card
        hideCard.GetComponent<Renderer>().enabled = true;
        
        // Adjust buttons visibility
        deal.gameObject.SetActive(false);
        Hit.gameObject.SetActive(true);
        Stand.gameObject.SetActive(true);
        standText.text = "Stand";
        
        // Set standard pot size
        pot = 40;
        betsText.text = "Bets: $" + pot.ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = "$" + playerScript.GetMoney().ToString();
    }
    
    private void HitClicked()
    {
        // Check if there is room on the table to show card
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 21)
            {
                RoundOver();
            }
            
        }
    }
    
    private void StandClicked()
    {
        standClicks++;
        if(standClicks > 1) RoundOver();
        HitDealer();
        standText.text = "Call";

    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            //dealer Score
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    
    //check for winner and loser
    void RoundOver()
    {
        //Booleans
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        
        //If stand has been clicke less than twice, no 21s
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        
        //both bust, bets return
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        //if player busts, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer Wins!";
        }
        //if dealer bust/has less than player, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "Player Wins!";
            playerScript.AdjustMoney(pot);
        }
        
        //Wash, bets returned
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Wash: bets returned";
            playerScript.AdjustMoney(pot /2);
        }

        else
        {
            roundOver = false;
        }
        //Set up ui for next turn
        if (roundOver)
        {
            Hit.gameObject.SetActive(false);
            Stand.gameObject.SetActive(false);
            deal.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }

    void BetClicked()
    {
        Text newBet = Bet.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = "Bets: $" + pot.ToString();
    }
}
