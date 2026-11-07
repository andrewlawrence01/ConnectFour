
namespace ConnectFour;

public class GameState
{
    public enum WinState
    {
        No_Winner = 0,
        Player1_Wins = 1,
        Player2_Wins = 2,
        Tie = 3
    }

    // Connect Four has 7 columns and 6 rows.
    // 0 = empty, 1 = Player 1, 2 = Player 2.
    public List<int> TheBoard { get; private set; } = new();

    public int CurrentTurn => TheBoard.Count(piece => piece != 0);

    public int PlayerTurn => (CurrentTurn % 2) + 1;

    public GameState()
    {
        ResetBoard();
    }

    // Start a new game.
    public void ResetBoard()
    {
        TheBoard = Enumerable.Repeat(0, 42).ToList();
    }

    // Check whether a column is full.
    public bool IsColumnFull(int column)
    {
        if (column < 0 || column >= 7)
        {
            return true;
        }

        return TheBoard[column] != 0;
    }

    // Drop a piece into the selected column.
    public int PlayPiece(byte column)
    {
        if (column >= 7)
        {
            throw new ArgumentException("Invalid column.");
        }

        if (CheckForWin() != WinState.No_Winner)
        {
            throw new ArgumentException("The game is already over.");
        }

        if (IsColumnFull(column))
        {
            throw new ArgumentException("That column is full.");
        }

        int player = PlayerTurn;

        // Start at the bottom row and move upward.
        for (int row = 5; row >= 0; row--)
        {
            int index = (row * 7) + column;

            if (TheBoard[index] == 0)
            {
                TheBoard[index] = player;

                // Return 1-6 for the CSS drop animation.
                return row + 1;
            }
        }

        throw new ArgumentException("No available space.");
    }

    // Check horizontal, vertical, and diagonal wins.
    public WinState CheckForWin()
    {
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                int index = (row * 7) + col;
                int player = TheBoard[index];

                if (player == 0)
                {
                    continue;
                }

                // Horizontal check.
                if (col <= 3 &&
                    player == TheBoard[index + 1] &&
                    player == TheBoard[index + 2] &&
                    player == TheBoard[index + 3])
                {
                    return GetWinner(player);
                }

                // Vertical check.
                if (row <= 2 &&
                    player == TheBoard[index + 7] &&
                    player == TheBoard[index + 14] &&
                    player == TheBoard[index + 21])
                {
                    return GetWinner(player);
                }

                // Diagonal down-right check.
                if (row <= 2 && col <= 3 &&
                    player == TheBoard[index + 8] &&
                    player == TheBoard[index + 16] &&
                    player == TheBoard[index + 24])
                {
                    return GetWinner(player);
                }

                // Diagonal down-left check.
                if (row <= 2 && col >= 3 &&
                    player == TheBoard[index + 6] &&
                    player == TheBoard[index + 12] &&
                    player == TheBoard[index + 18])
                {
                    return GetWinner(player);
                }
            }
        }

        // If all 42 spaces are occupied, the game is a tie.
        if (TheBoard.All(piece => piece != 0))
        {
            return WinState.Tie;
        }

        return WinState.No_Winner;
    }

    private static WinState GetWinner(int player)
    {
        return player == 1
            ? WinState.Player1_Wins
            : WinState.Player2_Wins;
    }
}
