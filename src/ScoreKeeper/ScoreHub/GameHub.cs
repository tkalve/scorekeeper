using ScoreKeeper.Models;

namespace ScoreHub
{
    public class GameHub
    {
        private static GameHub _instance;
        public static GameHub Instance => _instance ?? (_instance = new GameHub());

        private GameHub()
        {
            CurrentGame = new Game();
        }

        public Game CurrentGame { get; set; }
    }
}