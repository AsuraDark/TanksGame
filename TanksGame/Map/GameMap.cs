using System.Globalization;
using System.Text;
using TanksGame.Base;
using TanksGame.Tanks.Unit;



namespace TanksGame.Map
{
    internal class GameMap
    {
        private MapGenerator _generator;
        public static int _width;
        public static  int _height;
        private Cell[,] _map;

        public GameMap()
        {
            _generator = new MapGenerator();
            _width = _generator.Width;
            _height = _generator.Height;

        }

        public Cell[,] Map => _map;

        public int Width => _width;
        public int Height => _height;
        public void NewMap()
        {
            _generator.GenerateMap();
            _map = _generator.map;
        }
        public void Draw(ConsoleRenderer renderer)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Cell cell = _map[x, y];
                    string symbol = cell.GetSymbol();
                    byte color = _generator.GetCellColor(cell.Type);

                    for (int i = 0; i < Cell.Height; i++)
                    {
                        for (int j = 0; j < Cell.Width; j++)
                        {
                            int pixelX = x * Cell.Width + j;
                            int pixelY = y * Cell.Height + i;

                            if (pixelX < renderer.width && pixelY < renderer.height)
                            {
                                renderer.SetPixel(pixelX, pixelY, symbol, color);
                            }
                        }
                    }
                }
            }
        }

        public void BreakWall(int x, int y) => _generator.BreakWall(x, y);

        public CellType GetCellType(int x, int y)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                return _map[x, y].Type;
            }

            throw new ArgumentOutOfRangeException("Индекс за пределами карты.");
        }

        public void SetCellType(int x, int y, CellType type)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _map[x, y].Type = type;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Индекс за пределами карты.");
            }
        }


        public bool IsWalkable(int cellX, int cellY, List<IUnit> objects, Tank currentTank)
        {
            if (cellX < 0 || cellX >= _width || cellY < 0 || cellY >= _height)
            {
                return false;
            }

            if (_map[cellX, cellY].Type == CellType.Wall || _map[cellX, cellY].Type == CellType.BreakWall 
                || _map[cellX, cellY].Type == CellType.Water || _map[cellX, cellY].Type == CellType.Tank)
            {
                return false;
            }

            return true;
        }

        public void UpdateTankPosition(Cell oldPosition, Cell newPosition)
        {
            SetCellType(oldPosition.X, oldPosition.Y, CellType.Road);
            SetCellType(newPosition.X, newPosition.Y, CellType.Tank);
        }
    }
}
