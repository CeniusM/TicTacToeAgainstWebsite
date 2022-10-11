using System.Drawing;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using System.Diagnostics;
using System.Collections.Generic;

// the places to look for white color if the peice is set
// XCoords[piece][0(x), 1(y)]
int[,] XCoords = {
{ 90, 90 }, { 287, 90 }, { 476, 90 },
{ 90, 287 }, { 287, 287 }, { 476, 287 },
{ 90, 476 }, { 287, 476 }, { 476, 476 } };

int[,] OCoords = {
{ 90 - 50, 90 }, { 287 - 60, 90 }, { 476 - 60, 90 },
{ 90 - 50, 287 }, { 287 - 60, 287 }, { 476 - 60, 287 },
{ 90 - 50, 476 }, { 287 - 60, 476 }, { 476 - 60, 476 } };

// Rectangle bounds = Screen.GetBounds(Point.Empty);
Rectangle bounds = new Rectangle(294, 236, 858 - 294, 796 - 236);

Bitmap screen = new Bitmap(bounds.Width, bounds.Height);
Graphics g = Graphics.FromImage(screen);
void UpdateScreen()
{
    g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
}
void GetBoardFromScreen(int[] board)
{
    for (int i = 0; i < 9; i++)
    {
        board[i] = 0;
        Color pixel = screen.GetPixel(XCoords[i, 0], XCoords[i, 1]);
        if (pixel.R > 100)
            board[i] = 1;
        pixel = screen.GetPixel(OCoords[i, 0], OCoords[i, 1]);
        if (pixel.R > 100)
            board[i] = 2;
    }
}
bool IsGameOver()
{
    for (int i = 0; i < 9; i++)
    {
        Color pixel = screen.GetPixel(XCoords[i, 0], XCoords[i, 1]);
        if (pixel.R < 200 && pixel.R > 100)
            return true;
        else
        {
            pixel = screen.GetPixel(OCoords[i, 0], OCoords[i, 1]);
            if (pixel.R < 200 && pixel.R > 100)
                return true;
        }
    }
    return false;
}
void PrintBoard(int[] board)
{
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            if (board[j + (i * 3)] == 1)
                Console.Write("[X]");
            else if (board[j + (i * 3)] == 2)
                Console.Write("[O]");
            else
                Console.Write("[ ]");
        }
        Console.WriteLine();
    }
}
void ClickPiece(int index)
{
    HELP.helper.LeftMouseClick(XCoords[index, 0] + 200, XCoords[index, 1] + 140);

    int x = Console.GetCursorPosition().Left, y = Console.GetCursorPosition().Top;
    Console.SetCursorPosition(0, 10);
    Console.WriteLine("                                 ");
    Console.SetCursorPosition(0, 10);
    Console.WriteLine((XCoords[index, 0] + 200) + " " + (XCoords[index, 1] + 140));
    Console.SetCursorPosition(x, y);
}
int AIMoveRandom(int[] board, int playerTurn) // returns index
{
    Random rand = new Random();
    int move = 0;
    int attemps = 0;
    while (true)
    {
        if (attemps > 100)
            return 0;
        else
            attemps++;
        move = rand.Next(0, 9);
        if (board[move] == 0)
            return move;
    }
}
int AIMove(int[] board, int playerTurn) // returns index
{
    float[] movesRating = new float[9];
    List<int> moves = new List<int>();
    for (int i = 0; i < 9; i++)
        if (board[i] == 0)
            moves.Add(i);

    int move = 0;
    for (int i = 0; i < moves.Count; i++)
    {
        move = moves[i];
        board[move] = playerTurn;
        movesRating[move] = MoveRating(playerTurn ^ 3);
        board[move] = 0;
    }

    float MoveRating(int turn)
    {
        float[] movesRating = new float[9];
        List<int> moves = new List<int>();
        for (int i = 0; i < 9; i++)
            if (board[i] == 0)
                moves.Add(i);
        if (moves.Count == 0)
            return 0;

        for (int i = 0; i < moves.Count; i++)
        {
            float val = 0;
            board[moves[i]] = turn;

            if (GameOverCheck())
                val = (turn == 1 ? float.MinValue : float.MaxValue);
            else
            {
                val = MoveRating(turn ^ 3);
            }

            board[moves[i]] = 0;
            movesRating[moves[i]] = val;
        }

        // no moves
        return 0;
    }
    bool GameOverCheck()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i] != 0 && board[i] == board[i + 3] && board[i + 3] == board[i + 6])
                return true;
        }

        for (int i = 0; i < 3; i++)
        {
            if (board[i * 3] != 0 && board[(i * 3)] == board[(i * 3) + 1] && board[(i * 3) + 1] == board[(i * 3) + 2])
                return true;
        }

        if (board[0] != 0 && board[0] == board[4] && board[4] == board[8])
            return true;

        if (board[2] != 0 && board[2] == board[4] && board[4] == board[6])
            return true;

        return false;
    }

    // gets the heights value if 1, the lowest if 0
    move = -1;
    if (playerTurn == 1)
    {
        float val = float.MinValue;
        for (int i = 0; i < moves.Count; i++)
            if (movesRating[moves[i]] > val)
            {
                move = moves[i];
                val = movesRating[moves[i]];
            }
    }
    else if (playerTurn == 2)
    {
        float val = float.MaxValue;
        for (int i = 0; i < moves.Count; i++)
            if (movesRating[moves[i]] < val)
            {
                move = moves[i];
                val = movesRating[moves[i]];
            }
    }
    return move;
}
void PrintCurentMousePos()
{
    Point pos = HELP.helper.GetCursorPosition();

    Console.SetCursorPosition(0, 12);
    Console.WriteLine("                                         ");
    Console.SetCursorPosition(0, 12);
    Console.WriteLine("mousePos: " + pos.X + " " + pos.Y);
}

// Stack<int[]> boards = new Stack<int[]>(100);

UpdateScreen();
int[] board = new int[9];
Stopwatch sw = new Stopwatch();
const long TimeBetweenMove = 500;//ms
long timeSpent = 0;
// Console.SetWindowPosition(1, 1);
// Console.SetWindowSize(1920 / 2, 1080);

Console.WriteLine("Press enter to start");
Console.ReadLine();
Console.Clear();
while (true)
{
    // PrintCurentMousePos();
    sw.Restart();
    Console.SetCursorPosition(0, 0);

    // AI Move
    if (timeSpent > TimeBetweenMove)
    {
        int move = AIMove(board, 1);
        ClickPiece(move);

        timeSpent = 0;
    }

    UpdateScreen();
    GetBoardFromScreen(board);
    PrintBoard(board);
    if (IsGameOver())
    {
        Console.WriteLine("GAME OVER");
        Console.WriteLine("Press enter to play again");
        HELP.helper.LeftMouseClick(1000, 500); // click on cmd
        Console.ReadLine();
        HELP.helper.LeftMouseClick(294 + 100, 236 + 100);
        Console.Clear();
        timeSpent = 0;
        board = new int[9];
    }
    // else
    //     Console.WriteLine("         ");
    Console.WriteLine(sw.ElapsedMilliseconds);
    timeSpent += sw.ElapsedMilliseconds;
}

Console.ReadLine();