using TanksGame.Base;
using TanksGame;

internal class MapGenerator
{
    public Cell[,]? map { get; private set; }
    public int Width = 13;
    public int Height = 13;
    public string startMap =
        "▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" +
        "▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓                            ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓                            ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓▓▓▓▓████    ▓▓▓▓    ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓▓▓▓▓████    ▓▓▓▓    ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓            ▓▓▓▓            ▓▓▓▓            ▓▓▓▓" +
        "▓▓▓▓            ▓▓▓▓            ▓▓▓▓            ▓▓▓▓" +
        "▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓████    ████████████▓▓▓▓▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓████    ████████████▓▓▓▓▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓            ▓▓▓▓            ▓▓▓▓    ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓            ▓▓▓▓            ▓▓▓▓    ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓▓▓▓▓████    ▓▓▓▓    ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓▓▓▓▓████    ▓▓▓▓    ▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓                    ▓▓▓▓            ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓                    ▓▓▓▓            ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓                            ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓    ▓▓▓▓                            ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓    ▓▓▓▓" +
        "▓▓▓▓                    ▓▓▓▓                    ▓▓▓▓" +
        "▓▓▓▓                    ▓▓▓▓                    ▓▓▓▓" +
        "▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" +
        "▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓";

    public void GenerateMap()
    {
        
        int count = 0;
        map = new Cell[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++, count+=4)
            {
                CellType type;
                
                if (startMap[count] == '▓')
                    type = CellType.Wall;

                else if (startMap[count] == ' ')
                    type = CellType.Road; 

                else
                    type = CellType.Water;

                map[x, y] = new Cell(x, y, type);
                if (count % 52 == 0)
                {
                    count += 52;
                }

            }
            
            
        }
    }

    public byte GetCellColor(CellType type)
    {
        switch (type)
        {
            case CellType.Road:
                return 3;
            case CellType.Wall:
                return 0;
            case CellType.BreakWall:
                return 0;
            case CellType.Water:
                return 1;
            case CellType.Tank:
                return 3;
            default:
                return 0;
        }
    }

    

    public void BreakWall(int x, int y)
    {
        if (map == null) return;

        if (map[x, y].Type == CellType.Wall)
        {
            map[x, y].Type = CellType.BreakWall;
        }
        else if (map[x, y].Type == CellType.BreakWall)
        {
            map[x, y].Type = CellType.Road;
        }
    }
}
