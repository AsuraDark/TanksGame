using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.Unit;
using TanksGame.Tanks.Levels;
using static TanksGame.Tanks.TanksGameplayState;

namespace TanksGame.Tanks.Unit
{
    internal class Tank : IUnit
    {
        public enum TankType
        {
            Player,
            Enemy
        }

        private GameMap _gameMap;
        private UnitSystem _gameUnitSystem;

        public virtual TankType Type { get; private set; }
        public int Health;
        public virtual byte TankColor { get; protected set; }
        public Cell Position { get; set; }
        public TankDir Direction { get; set; }

        public virtual void Update(float deltaTime) { }

        public Tank(Cell startPosition, GameMap gameMap, UnitSystem gameUnitSystem, LevelSystem levelSystem, int startHealth)
        {
            Position = startPosition;
            _gameMap = gameMap;
            _gameUnitSystem = gameUnitSystem;
            Health = startHealth;
        }

        public void Draw(ConsoleRenderer renderer)
        {
            string[] tankShape = GetTankView();

            for (int i = 0; i < tankShape.Length; i++)
            {
                for (int j = 0; j < tankShape[i].Length; j++)
                {
                    int x = Position.X * Cell.Width + j;
                    int y = Position.Y * Cell.Height + i;
                    if (x >= 0 && x < renderer.width && y >= 0 && y < renderer.height)
                    {
                        renderer.SetPixel(x, y, tankShape[i][j].ToString(), TankColor);
                    }
                }
            }
        }

        public virtual void SetDirection(TankDir direction)
        {
            Direction = direction;
        }

        public void Shoot()
        {
            Thread.Sleep(50);
            var projectilePosition = Position;
            var projectile = new Projectile(this, projectilePosition, Direction, _gameMap, _gameUnitSystem.GetUnits(), _gameUnitSystem);
            _gameUnitSystem.AddUnit(projectile);
        }

        public void Move()
        {
            var newPosition = ShiftTo(Position, Direction);

            if (newPosition.X >= 0 && newPosition.X < _gameMap.Width && newPosition.Y >= 0 && newPosition.Y < _gameMap.Height)
            {
                if (_gameMap.IsWalkable(newPosition.X, newPosition.Y, _gameUnitSystem.GetUnits(), this))
                {
                    _gameMap.UpdateTankPosition(Position, newPosition);
                    Position = new Cell(newPosition.X, newPosition.Y, CellType.Tank);
                }
            }
        }

        private string[] GetTankView()
        {
            switch (Direction)
            {
                case TankDir.Up:
                    return TankView.Up;
                case TankDir.Down:
                    return TankView.Down;
                case TankDir.Left:
                    return TankView.Left;
                case TankDir.Right:
                    return TankView.Right;
                default:
                    return TankView.Up;
            }
        }
        public void TakeDamage(int damage)  
        {
            Health -= damage;
            if (Health <= 0)
            {
                Destroy();

            }
        }
        private void Destroy()
        {
            var map = _gameMap.Map;
            map[Position.X, Position.Y].Type = CellType.Road;
            _gameUnitSystem.RemoveUnit(this);
        }
    }
}
