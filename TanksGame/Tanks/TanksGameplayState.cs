using System.Drawing;
using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.Levels;
using TanksGame.Tanks.Unit;
using static System.Net.Mime.MediaTypeNames;

namespace TanksGame.Tanks
{
    internal class TanksGameplayState : BaseGameState
    {
        public enum TankDir
        {
            Up,
            Down,
            Right,
            Left
        }

        private const float MoveInterval = 1 / 4f;

        private float _timeToMove = 0f;
        private UnitSystem _gameUnitSystem;
        private LevelSystem _levelsSystem;
        public GameMap _gameMap;
        private TankDir _playerStartDirection = TankDir.Up;


        public PlayerTank _playerTank;
        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }
        public bool gameOver { get; private set; }
        public bool hasWon { get; private set; }

        public TanksGameplayState(UnitSystem gameUnitSystem, LevelSystem levelSystem, GameMap gameMap)
        {
            _gameUnitSystem = gameUnitSystem;
            _levelsSystem = levelSystem;
            _gameMap = gameMap;
        }

        public void SetDirection(TankDir direction)
        {
            if (_playerTank != null)
            {
                _playerTank.SetDirection(direction);
            }
        }

        public override void Reset()
        {
            gameOver = false;
            hasWon = false;



            _gameUnitSystem.Clear();
            if (_playerTank == null)
                _playerTank = new PlayerTank(_levelsSystem.GetPlayerStartPosition(), _gameMap, _gameUnitSystem, _levelsSystem);

            _playerTank.SetDirection(_playerStartDirection);
            _gameUnitSystem.AddUnit(_playerTank);

            foreach (Cell enemyPosition in _levelsSystem.GetEnemiesStartPositions())
            {
                var enemyTank = new EnemyTank(enemyPosition, _gameMap, _gameUnitSystem, _levelsSystem);
                _gameUnitSystem.AddUnit(enemyTank);
            }

            _timeToMove = 0f;
        }

        public override void Draw(ConsoleRenderer renderer)
        {
            _gameMap.Draw(renderer);

            renderer.DrawString($"Уовень: {_levelsSystem.GetCurrentLevel().LevelNumber}", _gameMap.Width * Cell.Width + 1, 0, ConsoleColor.White);
            renderer.DrawString($"Здоровье: {_playerTank.Health}", _gameMap.Width * Cell.Width + 1, 2, ConsoleColor.White);
            renderer.DrawString($"Враги: {_gameUnitSystem.GetUnits().OfType<EnemyTank>().Count()}", _gameMap.Width * Cell.Width + 1, 3, ConsoleColor.White);

            foreach (var obj in _gameUnitSystem.GetUnits())
            {
                obj.Draw(renderer);
            }
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;

            if (_timeToMove > 0f || gameOver)
                return;

            _timeToMove = MoveInterval;

            _gameUnitSystem.UpdateUnits(deltaTime);
            _gameUnitSystem.ProcessUnitsChanges();

            if (!_gameUnitSystem.GetUnits().Contains(_playerTank))
            {
                gameOver = true;
                _levelsSystem.ResetLevel();
                _playerTank.ResetHealth();
            }

            if (_gameUnitSystem.GetUnits().OfType<EnemyTank>().Count() <= 0)
            {
                hasWon = true;
                if (_levelsSystem.IsLastLevel())
                {
                    _levelsSystem.ResetLevel();
                }
                else
                {
                    _levelsSystem.NextLevel();
                    _playerTank.Position = _levelsSystem.GetPlayerStartPosition();
                }

            }
        }

        public static Cell ShiftTo(Cell from, TankDir dir)
        {
            int newX = from.X;
            int newY = from.Y;
            switch (dir)
            {
                case TankDir.Up:
                    newY -= 1;
                    break;
                case TankDir.Down:
                    newY += 1;
                    break;
                case TankDir.Right:
                    newX += 1;
                    break;
                case TankDir.Left:
                    newX -= 1;
                    break;
            }

            return new Cell(newX, newY, from.Type);
        }
        public override bool IsDone()
        {
            return gameOver || hasWon;
        }

    }
}
