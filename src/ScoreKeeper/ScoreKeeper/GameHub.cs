using ScoreKeeper.Models;

namespace ScoreKeeper
{
    public class GameHub
    {
        private static GameHub _instance;
        public static GameHub Instance => _instance ?? (_instance = new GameHub());

        private GameHub()
        {
            CurrentGame = new GameModel();
        }

        public GameModel CurrentGame { get; set; }
        public bool Whiteout { get; set; }
        public bool Blackout { get; set; }

    }
}