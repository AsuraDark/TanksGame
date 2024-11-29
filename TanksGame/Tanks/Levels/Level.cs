using TanksGame.Tanks.Unit;

namespace TanksGame.Tanks.Levels
{
    internal class Level
    {
        public int LevelNumber { get; }
        public Cell PlayerStartPosition { get; }
        public List<Cell> EnemiesStartPositions { get; }

        public Level(int levelNumber, Cell playerStartPosition, List<Cell> enemiesStartPositions)
        {
            LevelNumber = levelNumber;
            PlayerStartPosition = playerStartPosition;
            EnemiesStartPositions = enemiesStartPositions;
        }
        
    }
}