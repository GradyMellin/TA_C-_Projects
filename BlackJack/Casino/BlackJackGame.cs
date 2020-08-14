using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BlackJack
{
    public class BlackJackGame : Game, IWalkAway
    {
        public BlackJackDealer Dealer { get; set; }

        public override void Play()
        {
            Dealer = new BlackJackDealer();
            foreach (Player player in Players)
            {
                player.Hand = new List<Card>();
                player.Stay = false;

            }
            Dealer.Hand = new List<Card>();
            Dealer.Stay = false;
            Dealer.Deck = new Deck();
            Dealer.Deck.Shuffle();

            foreach (Player player in Players)
            {
                bool validAnswer = false;
                int bet = 0;
                while (!validAnswer)
                {
                    Console.WriteLine("Place your bet!");
                    validAnswer = int.TryParse(Console.ReadLine(), out bet );
                    if (!validAnswer) { Console.WriteLine("Please enter digits only, no decimals."); }
                }
                if (bet<0)
                {
                    throw new FraudException("Security! Kick this person OUT!");
                }
                bool successfullyBet = player.Bet(bet);
                if (!successfullyBet)
                {
                    return;  
                }
                Bets[player] = bet;
            }
            for(int i = 0; i < 2; i++)
            {
                Console.WriteLine("Dealing....");
                foreach(Player player in Players)
                {
                    Console.Write("{0}: ", player.Name);
                    Dealer.Deal(player.Hand);
                    if (i == 1)
                    {
                        bool blackJack = BlackJackRules.CheckForBlackJack(player.Hand);
                        if(blackJack)
                        {
                            player.Balance += Convert.ToInt32((Bets[player] * 1.5) + Bets[player]);
                            Console.WriteLine("BlackJack! {0} wins {1}, your balance is now {2}.", player.Name, Bets[player], player.Balance);
                            WalkAway(player);
                            return;
                        }
                    }
                }
                Console.Write("Dealer: ");
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {
                    bool blackJack = BlackJackRules.CheckForBlackJack(Dealer.Hand);
                    if (blackJack)
                    {
                        Console.WriteLine("Dealer has BlackJack! Everyone loses!");
                        foreach(KeyValuePair<Player, int> entry in Bets)
                        {
                            Dealer.Balance += entry.Value;
                        }
                        foreach (Player player in Players)
                        {
                            Console.WriteLine("{0}, your balance is now {2}.", player.Name, Bets[player], player.Balance);
                            WalkAway(player);
                            return;
                        }
                    }
                }
            }
            foreach(Player player in Players)
            {
                while (!player.Stay)
                {
                    Console.WriteLine("Your cards are: ");
                    foreach(Card card in player.Hand)
                    {
                        Console.Write("{0} ", card.ToString());
                    }
                    Console.WriteLine("\n\nHit Or Stay? ");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "stay")
                    {
                        player.Stay = true;
                        break;
                    }
                    else if (answer == "hit")
                    {
                        Dealer.Deal(player.Hand);
                        bool blackJack = BlackJackRules.CheckForBlackJack(Dealer.Hand);
                    }
                    bool busted = BlackJackRules.IsBusted(player.Hand);
                    if (busted)
                    {
                        Dealer.Balance += Bets[player];
                        Console.WriteLine("{0} Busted! You lose your bet of {1}. Your balance is now {2}.", player.Name, Bets[player], player.Balance);
                        player.Stay = false;
                        WalkAway(player);
                        return;
                    }
                }
            }
            Dealer.isBusted = BlackJackRules.IsBusted(Dealer.Hand);
            Dealer.Stay = BlackJackRules.ShouldDealerStay(Dealer.Hand);
            while(!Dealer.Stay && !Dealer.isBusted)
            {
                Console.WriteLine("Dealer is hitting...");
                Dealer.Deal(Dealer.Hand);
                Dealer.isBusted = BlackJackRules.IsBusted(Dealer.Hand);
                Dealer.Stay = BlackJackRules.ShouldDealerStay(Dealer.Hand);
            }
            if(Dealer.Stay)
            {
                Console.WriteLine("Dealer is staying. ");
            }
            else if (Dealer.isBusted)
            {
                Console.WriteLine("Dealer Busted!");
                foreach(KeyValuePair<Player, int> entry in Bets)
                {
                    Console.WriteLine("{0} won {1}!", entry.Key.Name, entry.Value);
                    Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2);
                    Dealer.Balance -= entry.Value;
                }
                foreach(Player player in Players) { WalkAway(player); }
                return;
            }
            foreach(Player player in Players)
            {
                bool? playerWon = BlackJackRules.CompareHands(player.Hand, Dealer.Hand);
                if (playerWon == null)
                {
                    player.Balance += Bets[player];
                    Console.WriteLine("Push! No one wins. Your balance is now {0}.", player.Balance);
                }
                else if (playerWon == true)
                {
                    player.Balance += (Bets[player] * 2);
                    Dealer.Balance -= Bets[player];
                    Console.WriteLine("{0} won {1}. Your balance is now {2}.", player.Name, Bets[player], player.Balance);
                }
                else
                {
                    Dealer.Balance += Bets[player];
                    Console.WriteLine("Dealer won {0}! Your balance is now {1}.", Bets[player], player.Balance);
                }               
                WalkAway(player);
            }
        }

        public override void ListPlayers()
        {
            Console.WriteLine("BlackJack Players: ");
            base.ListPlayers();
        }
        public void WalkAway(Player player)
        {
            Console.WriteLine("Would you like to play another hand? ");
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                player.isActivelyPlaying = true;
                return;
            }
            else
            {
                player.isActivelyPlaying = false;
                return;
            }
        }
    }
}
