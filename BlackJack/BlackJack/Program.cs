using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Casino;
using Casino.BlackJack;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            const string casinoName = "World Underground Gambling Ring";
            Console.WriteLine("Welcome to the {0}, What is your name?", casinoName);
            string playerName = Console.ReadLine();
            Console.WriteLine("Hello, {0} how much money did you bring today?", playerName);
            int bank = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("{0}, would you like to play a game of BlackJack? ", playerName);
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                Player player = new Player(playerName, bank);
                Game game = new BlackJackGame();
                game += player;
                player.isActivelyPlaying = true;
                while(player.isActivelyPlaying && player.Balance > 0)
                {
                    game.Play();
                }
                game -= player;
                Console.WriteLine("Thank you for playing!");
            }
            Console.WriteLine("Feel Free to look around the casino. Bye for now.");
            Console.Read();
        }

    }
}
