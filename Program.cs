using System;
using System.Collections.Generic;

namespace bs_game
{
    class Program
    {
        public static List<Player> players = new List<Player>();

        public static Deck mainDeck;

        public static int cardTypeToMatch = 1;
        public static bool gameover;
        static void Main(string[] args)
        {
            Program.gameover = false;
            Console.WriteLine("Welcome!");

            int playerCount;
            System.Console.WriteLine("How many players?");
            playerCount = int.Parse(System.Console.ReadLine());

            for (int i=0; i<playerCount; i++){
                System.Console.WriteLine("Enter player #" +(i+1).ToString()+"'s name.");
                players.Add(new Player(System.Console.ReadLine()));
            }

            mainDeck = new Deck();

            int currentPlayer = 0;
            string cardId="";
            int countOfCards = 1;
            int[] cardsToPlay;
            string cardPlayInput;
            string cardsToMatchPretty;

            while(!gameover){
                cardsToMatchPretty = cardTypeToMatch.ToString();
                if(cardTypeToMatch > 10){
                    if(cardTypeToMatch == 11){
                        cardsToMatchPretty = "Jack";
                    }
                    else if(cardTypeToMatch == 12){
                        cardsToMatchPretty = "Queen";
                    }
                    else if(cardTypeToMatch == 13){
                        cardsToMatchPretty = "King";
                    }
                }
                else if(cardTypeToMatch ==1){
                    cardsToMatchPretty = "Ace";
                }
                System.Console.WriteLine($"\n{Program.players[currentPlayer].Name} it's your turn.\n");
                System.Console.WriteLine($"You are expected to play a {cardsToMatchPretty}\nView hand? y or n...");
               string input = Console.ReadLine();
               if(input.ToUpper() == "Y"){
                   Program.players[currentPlayer].ViewHand();
               }
        
               System.Console.WriteLine($"You must play a {cardsToMatchPretty}\nEnter the card numbers you would like to play...");
               System.Console.WriteLine("Example: 1,4,8");
               cardPlayInput = System.Console.ReadLine();
               countOfCards = 1;
               for (int idx=0; idx<cardPlayInput.Length; idx++){
                   if (cardPlayInput[idx] == ','){
                       ++countOfCards;
                   }
               }
               cardId="";
               cardsToPlay = new int[countOfCards];
               int toPlayIdx=0;
               for (int inputIdx=0; inputIdx<cardPlayInput.Length; inputIdx++){
                   if (cardPlayInput[inputIdx] != ','){
                       cardId += cardPlayInput[inputIdx];
                   }
                   else{
                       cardsToPlay[toPlayIdx] = int.Parse(cardId);
                       toPlayIdx++;
                       cardId = "";
                   }
               }

                cardsToPlay[cardsToPlay.Length-1] = int.Parse(cardId);

               Program.players[currentPlayer].PlayCards(cardsToPlay);
               
               Program.players[currentPlayer].lieReset();
               if(currentPlayer < Program.players.Count-1){
                   currentPlayer += 1;
               }
               else{
                   currentPlayer =0;
               }
               if(cardTypeToMatch < 13){
                   cardTypeToMatch++;
               }
               else{
                   cardTypeToMatch = 1;
               }
            }
        }
    }
    public class Card
    {
        public string Face;
        public string Suit;
        public int Val;
        public Card(string face, string suit, int value)
        {
            Face = face;
            Suit = suit;
            Val = value;
        }
    }
    public class Deck
    {
        public List<Card> cards;

        public Deck(){
            buildDeck();
            shuffle();
            deal();
        }

        private void shuffle(){
            for (int i =0; i<1000; i++){
                Random rand = new Random();

                int Idx1 = rand.Next(0,cards.Count);
                int Idx2 = rand.Next(0,cards.Count);

                Card temp = cards[Idx1];
                cards[Idx1] = cards[Idx2];
                cards[Idx2] = temp; 
            }
        }
        
        public void buildDeck()
        {
            cards = new List<Card>();
            string[] suit = {"Hearts", "Spades", "Diamonds", "Clubs"};
            string[] face = {"Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King"};
            foreach(string eachsuit in suit)
            {
                for ( int i = 0; i < face.Length; i++)
                {
                    Card one = new Card(face[i], eachsuit, i+1);
                    cards.Add(one);
                }
            }
            
        }
        public void flush(Player victim)
        {
            int total = cards.Count-1;
            for (int i = total; i>=0; i--){
                Card dealt = cards[i];
                victim.Hand.Add(dealt);
                cards.RemoveAt(i);
            }
        }
        
        public void deal()
        {
            int total = cards.Count;
            int playerToDealTo = 0;
            for (int i=total-1; i>=0; i--){

                Program.players[playerToDealTo].Hand.Add(cards[i]);
                cards.RemoveAt(i);
                if(playerToDealTo < Program.players.Count-1){
                    playerToDealTo++;
                }
                else{
                    playerToDealTo = 0;
                }
            }
        }
    }
    public class Player
    {
        private static List<Player> playerLog = new List<Player>();
        private static int playerCount;
        public List<Card> Hand;
        public string Name;
        private bool lied;
        public int PlayerCount;

        public static int getPlayerCount(){
            return playerCount;
        }
        public Player(string name)
        {
            Player.playerLog.Add(this);
            playerCount++;
            Name = name;
            Hand = new List<Card>();
            lied = false;
            //SAMPLE CARDS REMOVE LATER
            // Hand.Add(new Card("Queen","Hearts",12));
            // Hand.Add(new Card("Ace","Spades",1));
        }

        private static Player findByName(string name){
            foreach (Player person in Player.playerLog){
                if(person.Name == name){
                    return person;
                }
            }
            return null;
        }

        public void lieReset(){
            lied = false;
        }
        public void ViewHand()
        {
            System.Console.WriteLine($"Everyone except for {Name}, look away!!!\nPress any key once you're ready to view...");
            Console.ReadKey();
            string prettier_message = "Your hand - ";
            int idx = 0;
            foreach (Card card in Hand)
            {
                prettier_message += $"\nCard {idx}: {card.Face} of {card.Suit}";
                idx++;
            }
            System.Console.WriteLine(prettier_message);
            System.Console.WriteLine("\nOnce you're finished viewing your hand, press any key to continue...");
            Console.ReadKey();
            string wall = "_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__\n_|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|\n___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__";
            System.Console.WriteLine(wall);
        }

        public void Accuse(Player accuser, Player victim){
            if(victim.lied == true){
                System.Console.WriteLine($"You were right, {victim.Name} lied!");
                Program.mainDeck.flush(victim);
            }
            else{
                System.Console.WriteLine($"You were wrong, {victim.Name} did not lie!");
                Program.mainDeck.flush(accuser);
                if(victim.Hand.Count==0){
                    System.Console.WriteLine($"{this.Name} won!");
                    Program.gameover = true;
                }
            }
        }
        public void PlayCards(int[] cardIdArr)
        {
            foreach (int id in cardIdArr)
            {
                Card selected = Hand[id];
                Program.mainDeck.cards.Add(selected);
                Hand.RemoveAt(id);
                if (selected.Val != Program.cardTypeToMatch) 
                {
                    lied = true;
                }
            }
            System.Console.WriteLine($"{Name} played {cardIdArr.Length} cards.");
            System.Console.WriteLine("Do your believe it? y or n");
            string bs = System.Console.ReadLine();
            if(bs.ToUpper() != "Y")
            {
                System.Console.WriteLine($"Who is accusing {Name}?");
                string accuserName = System.Console.ReadLine();
                Player accuser = Player.findByName(accuserName);
                Accuse(accuser, this);
            }
            else{
                if(Hand.Count==0){
                    System.Console.WriteLine($"{this.Name} won!");
                    Program.gameover = true;
                }
            }
        }
    }
}
