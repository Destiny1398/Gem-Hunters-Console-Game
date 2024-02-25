using System;

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U':
                if (Position.Y > 0)
                    Position.Y--;
                break;
            case 'D':
                if (Position.Y < 5)
                    Position.Y++;
                break;
            case 'L':
                if (Position.X > 0)
                    Position.X--;
                break;
            case 'R':
                if (Position.X < 5)
                    Position.X++;
                break;
            default:
                Console.WriteLine("Invalid direction. Use U, D, L, or R.");
                break;
        }
    }
}

class Cell
{
    public string Occupant { get; set; }
}

class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell { Occupant = "-" };
            }
        }

        
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        
        Random random = new Random();
        for (int i = 0; i < 4; i++)
        {
            int gemX = random.Next(6);
            int gemY = random.Next(6);
            Grid[gemX, gemY].Occupant = "G";

            int obstacleX = random.Next(6);
            int obstacleY = random.Next(6);
            
            while (Grid[obstacleX, obstacleY].Occupant != "-")
            {
                obstacleX = random.Next(6);
                obstacleY = random.Next(6);
            }
            Grid[obstacleX, obstacleY].Occupant = "O";
        }
    }

    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write($"{Grid[i, j].Occupant} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U':
                newY--;
                break;
            case 'D':
                newY++;
                break;
            case 'L':
                newX--;
                break;
            case 'R':
                newX++;
                break;
            default:
                return false;
        }

        // Check if the new position is within the bounds and not an obstacle
        return newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && Grid[newX, newY].Occupant != "O";
    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
        }
    }
}

class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; set; }
    public int TotalTurns { get; set; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void Start()
    {
        do
        {
            Board.Display();
            Console.WriteLine($"{CurrentTurn.Name}'s turn. Enter U, D, L, or R to move: ");
            char direction = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (Board.IsValidMove(CurrentTurn, direction))
            {
                
                CurrentTurn.Move(direction);

                
                Board.CollectGem(CurrentTurn);

                
                SwitchTurn();

                
                TotalTurns++;
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
            }

        } while (!IsGameOver());

        Board.Display();
        AnnounceWinner();
    }

    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    public bool IsGameOver()
    {
        return TotalTurns >= 30; 
    }

    public void AnnounceWinner()
    {
        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine($"{Player1.Name} wins!");
        }
        else if (Player2.GemCount > Player1.GemCount)
        {
            Console.WriteLine($"{Player2.Name} wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}
class Program
{
    static void Main()
    {
        Game gemHuntersGame = new Game();
        gemHuntersGame.Start();
    }
}
