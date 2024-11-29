using System.Runtime.CompilerServices;
using TanksGame.Map;
using TanksGame.Tanks.Unit;
using TanksGame.Tanks.Levels;
using static TanksGame.Tanks.TanksGameplayState;

namespace TanksGame.Tanks.Unit
{
    internal class EnemyTank : Tank
    {
        public override TankType Type => TankType.Enemy;
        public override byte TankColor => 4;
        private Random _random;

        private float shootInterval = 1/10f;
        private float timeSinceLastShot = 0.0f;
        private UnitSystem _gameUnitsSystem;
        private GameMap _gameMap;
        public EnemyTank(Cell startPosition, GameMap gameMap, UnitSystem gameUnitsSystem, LevelSystem levelSystem)
                    : base(startPosition, gameMap, gameUnitsSystem, levelSystem, 2)
        {
            _gameMap = gameMap;
            _gameUnitsSystem = gameUnitsSystem;
        }

        public override void Update(float deltaTime)
        {
            timeSinceLastShot += deltaTime;

            if (CanSeePlayer())
            {
                if (timeSinceLastShot >= shootInterval)
                {
                    Shoot();
                    timeSinceLastShot = 0.0f;
                }
            }
            else
            {
                Cell newPosition = ShiftTo(Position, Direction);
                if (_gameMap.IsWalkable(newPosition.X, newPosition.Y, _gameUnitsSystem.GetUnits(), this))
                {
                    Move();

                }
                else
                {
                    ChangeDirection();
                    Move();
                }
            }
        }

        private void ChangeDirection()
        {
            _random = new Random();
            var directions = Enum.GetValues(typeof(TankDir)).Cast<TankDir>().ToList();
            directions.Remove(Direction);
            Direction = directions[_random.Next(directions.Count)];
        }

        private bool CanSeePlayer()
        {
            var direction = Direction;
            var position = Position;

            while (true)
            {

                position = ShiftTo(position, direction);

                if (position.X < 0 || position.X >= _gameMap.Width || position.Y < 0 || position.Y >= _gameMap.Height)
                {
                    return false;
                }

                var obstacle = _gameMap.GetCellType(position.X, position.Y);

                if (obstacle == CellType.Wall || obstacle == CellType.BreakWall)
                {
                    return false;
                }

                if (obstacle == CellType.Tank)
                {
                    foreach (var entity in _gameUnitsSystem.GetUnits())
                    {
                        if (entity is Tank playerTank && playerTank.Type == TankType.Player)
                        {
                            if (playerTank.Position.X == position.X && playerTank.Position.Y == position.Y)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
    }
}
