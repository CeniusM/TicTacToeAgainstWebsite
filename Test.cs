using System.Diagnostics;

namespace Test;

class Program
{
    static int AIMove(int[] board, int playerTurn) // returns index
    {
        float MoveRating(int turn)
        {
            float[] currentMovesRating = new float[9];
            if (playerTurn == 1)
                for (int i = 0; i < 9; i++)
                    currentMovesRating[i] = float.MinValue;
            else
                for (int i = 0; i < 9; i++)
                    currentMovesRating[i] = float.MaxValue;
            List<int> moves = new List<int>();
            for (int i = 0; i < 9; i++)
                if (board[i] == 0)
                    moves.Add(i);
            if (moves.Count == 0)
                return 0;

            float val = 0;
            for (int i = 0; i < moves.Count; i++)
            {
                val = 0;
                board[moves[i]] = turn;

                if (GameOverCheck())
                    val = (turn == 1 ? float.MaxValue : float.MinValue);
                else
                {
                    val = MoveRating(turn ^ 3);
                }

                board[moves[i]] = 0;
                currentMovesRating[moves[i]] = val;
            }

            val = (turn == 1 ? float.MinValue : float.MaxValue);
            if (turn == 1)
            {
                for (int i = 0; i < moves.Count; i++)
                    if (currentMovesRating[moves[i]] > val)
                    {
                        val = currentMovesRating[moves[i]];
                    }
            }
            else if (turn == 2)
            {
                for (int i = 0; i < moves.Count; i++)
                    if (currentMovesRating[moves[i]] < val)
                    {
                        val = currentMovesRating[moves[i]];
                    }
            }
            return val;
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


        // Console.Clear();
        // for (int i = 0; i < 9; i++)
        //     Console.Write(board[i]);
        // Console.WriteLine();

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
            if (GameOverCheck())
                movesRating[move] = (playerTurn == 1 ? float.MaxValue : float.MinValue);
            else
                movesRating[move] = MoveRating(playerTurn ^ 3);
            board[move] = 0;
        }

        // for (int i = 0; i < moves.Count; i++)
        //     Console.WriteLine("Move: " + moves[i] + " Rating: " + movesRating[moves[i]]);
        // for (int i = 0; i < 9; i++)
        //     Console.Write(board[i]);
        // Console.WriteLine();

        // gets the heights value if 1, the lowest if 2
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

        // if (move == -1)
        // {
        //     for (int i = 0; i < 3; i++)
        //     {
        //         for (int j = 0; j < 3; j++)
        //         {
        //             if (board[j + (i * 3)] == 1)
        //                 Console.Write("[X]");
        //             else if (board[j + (i * 3)] == 2)
        //                 Console.Write("[O]");
        //             else
        //                 Console.Write("[ ]");
        //         }
        //         Console.WriteLine();
        //     }

        //     for (int i = 0; i < moves.Count; i++)
        //         Console.WriteLine("Move: " + moves[i] + " Rating: " + movesRating[i]);

        //     Console.ReadLine();
        // }
        // for (int i = 0; i < moves.Count; i++)
        //     Console.WriteLine("Move: " + moves[i] + " Rating: " + movesRating[i]);
        return move;
    }
    public static void Test()
    {
        // int[] test = {
        //     0,1,0,
        //     0,2,1,
        //     0,0,2
        // };
        // int[] test = {
        //     1,1,0,
        //     0,2,0,
        //     2,0,0
        // };
        int[] test = new int[9];

        int move = AIMove(test, 1);
        const int Samples = 100;
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < Samples; i++)
        {
            move = AIMove(test, 1);
        }
        Console.WriteLine(sw.ElapsedMilliseconds / Samples + "ms AVG");

        Console.WriteLine("Move: " + move);
        Console.ReadLine();
    }
}