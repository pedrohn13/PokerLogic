using System;
using System.Collections.Generic;
using System.Text;

namespace PokerLogic
{
    public class Player
    {

        public String Name { get; set; }
        public int Position { get; set; }
        public Card[] Cards { get; set; }
        public PlayerState PlayerState { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
        public int Credits { get; set; }
        public int BetCredits { get; set; }

        public Player(String name, int credits)
        {
            Name = name;
            Credits = credits;
            Position = -1;
            Cards = new Card[2];
        }

        public void SitDown(int position)
        {
            Position = position;
            PlayerStatus = PlayerStatus.SEATED;
        }

        public void Standup()
        {
            Position = -1;
            PlayerStatus = PlayerStatus.STANDING;
        }

        public void Bet(int value)
        {
            if (Credits - BetCredits >= 0)
            {
                BetCredits = value;
            }
        }

        public void PayBet()
        {
            Credits -= BetCredits;
        }

        public void StartToPlay()
        {
            PlayerState = PlayerState.PLAYING;
        }

        public void GiveUp()
        {
            PlayerState = PlayerState.GIVEUP;
        }

        public Hand GetHighestHand(Card[] commonCards)
        {
            //FAZER CALCULO DA MÃO COM CARTAS COMUNITÁRIAS
            return Hand.HIGH_CARD;
        }

    }    
}
