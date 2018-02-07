using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerLogic
{
    public class Table
    {
        public GameState GameState { get; set; }
        public Player[] Players { get; set; }
        public int CurrentPlayer { get; set; }
        public List<Card> Deck { get; set; }
        public Card[] CommonCards { get; set; }
        public int DealerPosition { get; set; }
        public int SmallBlind { get; set; }
        public int BigBlind { get; set; }
        public int CurrentMaxBet { get; set; }

        public Table(int playerLimit, int smallBlind)
        {
            GameState = GameState.CREATED;
            Players = new Player[playerLimit];
            CurrentPlayer = -1;
            Deck = CreateDeck();
            CommonCards = new Card[5];
            DealerPosition = -1;
            SmallBlind = smallBlind;
            BigBlind = 2 * SmallBlind;
        }



        public bool PlayerSitDown(Player player, int position)
        {
            if (Players[position] == null)
            {
                player.SitDown(position);
                Players[position] = player;
                SetDealer();
                return true;
            }
            return false;
        }

        public bool PlayerStandup(int position)
        {
            if (Players[position] != null)
            {
                Players[position] = null;
                return true;
            }
            return false;
        }

        public bool SetUpGame()
        {
            if (GetTotalPlayersSeated() >= 3)
            {
                GameState = GameState.SETUP;
                BetSBBB();
                DealCards();
                return true;
            }
            else
            {
                return false;
            }

        }

        private int GetTotalPlayersSeated()
        {
            return Players.Count(p => p != null);
        }

        private List<Card> CreateDeck()
        {
            List<Card> deck = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                for (int i = 1; i <= 13; i++)
                    deck.Add(new Card(i, suit));
            }
            SetDealer();
            return deck;

        }

        private void SetDealer()
        {
            if (GetTotalPlayersSeated() == 1)
            {
                DealerPosition = Array.FindIndex(Players, i => i != null);
            }
        }

        private void BetSBBB()
        {
            int sbPosition = getNextPlayerPosition(DealerPosition);
            int bbPosition = getNextPlayerPosition(sbPosition);
            Players[sbPosition].Bet(SmallBlind);
            Players[bbPosition].Bet(BigBlind);
        }

        private void DealCards()
        {
            ShuffleCards();
            DealOneCardToEachPlayer(0);
            DealOneCardToEachPlayer(1);
        }

        private void DealOneCardToEachPlayer(int index)
        {
            int currentDealPosition = DealerPosition + 1;
            while (true)
            {
                // Verify the end of Array
                if (currentDealPosition == Players.Length)
                {
                    currentDealPosition = 0;
                }

                // Verify not empty chair
                if (Players[currentDealPosition] != null)
                {
                    Players[currentDealPosition].Cards[index] = getNextCard();
                }

                // Verify finish the circle
                if (currentDealPosition == DealerPosition)
                {
                    break;
                }
                currentDealPosition++;
            }
        }

        private Card getNextCard()
        {
            Card card = Deck[0];
            Deck.RemoveAt(0);
            return card;
        }

        private void ShuffleCards()
        {
            Random rng = new Random();
            int n = Deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = Deck[k];
                Deck[k] = Deck[n];
                Deck[n] = card;
            }
        }

        private int getNextPlayerPosition(int currentPosition)
        {
            currentPosition++;
            while (Players[currentPosition] == null)
            {
                currentPosition++;
                if (currentPosition == Players.Length)
                {
                    currentPosition = 0;
                }
            }
            return currentPosition;
        }

    }
}
