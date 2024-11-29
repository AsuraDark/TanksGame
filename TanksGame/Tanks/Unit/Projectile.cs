using System.Text;
using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.Unit;
using static TanksGame.Tanks.TanksGameplayState;

namespace TanksGame.Tanks.Unit
{
    internal class Projectile : IUnit
    {
        public Cell Position { get; private set; }
        public TankDir Direction { get; private set; }
        public Tank Owner { get; private set; }
        private string _bulletView = "•";

        private GameMap _gameMap;
        private List<IUnit> _units;

        private UnitSystem _gameUnitsSystem;

        public Projectile(Tank owner, Cell startPosition, TankDir direction, GameMap map, List<IUnit> objects, UnitSystem gameObjectsSystem)
        {
            Owner = owner;
            Position = startPosition;
            Direction = direction;
            _gameMap = map;
            _units = objects;
            _gameUnitsSystem = gameObjectsSystem;
        }

        public void Update(float deltaTime)
        {
            Move();
        }

        public void Draw(ConsoleRenderer renderer)
        {
            if (Position.X >= 0 && Position.X < renderer.width && Position.Y >= 0 && Position.Y < renderer.height)
            {
                Console.OutputEncoding = Encoding.Unicode;
                renderer.SetPixel(Position.X * Cell.Width + 1, Position.Y * Cell.Height+1, _bulletView, Owner.TankColor);
            }
        }

        public void Move()
        {
            if (Position.X >= _gameMap.Width || Position.Y >= _gameMap.Height
                || Position.X < 0 || Position.Y < 0)
            {
                _gameUnitsSystem.RemoveUnit(this);
            }
            Cell newPosition = ShiftTo(Position, Direction);
            if (newPosition.X < _gameMap.Width && newPosition.Y < _gameMap.Height
                && newPosition.X >= 0 && newPosition.Y >= 0)
            {
                var obstacle = _gameMap.GetCellType(newPosition.X, newPosition.Y);

                if (obstacle == CellType.Wall || obstacle == CellType.BreakWall)
                {
                    _gameMap.BreakWall(newPosition.X, newPosition.Y);
                    _gameUnitsSystem.RemoveUnit(this);
                    return;
                }


                else if (obstacle == CellType.Tank)
                {
                    _gameUnitsSystem.RemoveUnit(this);

                    foreach (var entity in _units)
                    {
                        if (entity is Tank tank && tank.Position.X == newPosition.X && tank.Position.Y == newPosition.Y)
                        {
                            if (tank != Owner)  // Проверка на владение снарядом (т.е. что танк не является владельцем снаряда)
                            {
                                tank.TakeDamage(1);
                            }
                            _gameUnitsSystem.RemoveUnit(this);
                            break;
                        }
                    }
                    return;
                }
            }
            else
            {
                _gameUnitsSystem.RemoveUnit(this);
            }

            Position = newPosition;
        }
    }
}
