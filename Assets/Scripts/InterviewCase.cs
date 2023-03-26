using System;
using Random = UnityEngine.Random;

public static class InterviewCase
{
    public static bool MatchExists(Board board, int index1, int index2)
    {
        (board.Grid[index1], board.Grid[index2]) = (board.Grid[index2], board.Grid[index1]);

        for (var i = 0; i < 2; i++)
        {
            if (
                CheckMatchHorizontal(board, index1 + i) 
                || CheckMatchVertical(board, index1 + i)
                || CheckMatchHorizontal(board, index2 + i) 
                || CheckMatchVertical(board, index2 + i)
                || CheckMatchHorizontal(board, index1 - i) 
                || CheckMatchVertical(board, index1 - i)
                || CheckMatchHorizontal(board, index2 - i) 
                || CheckMatchVertical(board, index2 - i)
                || CheckMatchHorizontal(board, index1 + board.Width) 
                || CheckMatchVertical(board, index1 + board.Width)
                || CheckMatchHorizontal(board, index2 + board.Width) 
                || CheckMatchVertical(board, index2 + board.Width)
                || CheckMatchHorizontal(board, index1 - board.Width) 
                || CheckMatchVertical(board, index1 - board.Width)
                || CheckMatchHorizontal(board, index2 - board.Width) 
                || CheckMatchVertical(board, index2 - board.Width)
                )
            {
                (board.Grid[index1], board.Grid[index2]) = (board.Grid[index2], board.Grid[index1]);
                return true;
            }
        }

        (board.Grid[index1], board.Grid[index2]) = (board.Grid[index2], board.Grid[index1]);
        return false;
    }

    public static Tuple<int, int>[] GetAllPossibleMatches(Board board)
    {
        var matches = Array.Empty<Tuple<int, int>>();

        for (var i = 0; i < board.Grid.Length - 1; i++)
        {
            if (i % board.Width != board.Width - 1)
            {
                if (MatchExists(board, i, i + 1))
                {
                    var newMatches = CopyToNewTuple(matches);
                    newMatches[^1] = new Tuple<int, int>(i, i + 1);

                    matches = newMatches;
                }
            }

            if (board.Grid.Length > i + board.Width)
            {
                if (MatchExists(board, i, i + board.Width))
                {
                    var newMatches = CopyToNewTuple(matches);
                    newMatches[^1] = new Tuple<int, int>(i, i + board.Width);

                    matches = newMatches;
                }
            }
        }

        return matches;
    }

    private static Tuple<int, int>[] CopyToNewTuple(Tuple<int, int>[] oldTuple)
    {
        var newTuple = new Tuple<int, int>[oldTuple.Length + 1];
        for (var i = 0; i < oldTuple.Length; i++)
        {
            newTuple[i] = oldTuple[i];
        }

        return newTuple;
    }

    public static void Shuffle(Board board)
    {
        var gridBeforeShuffle = (int[]) board.Grid.Clone();
        
        for (var i = 0; i < board.Grid.Length; i++)
        {
            var random = Random.Range(0, board.Grid.Length);
            (board.Grid[random], board.Grid[i]) = (board.Grid[i], board.Grid[random]);
        }

        if (IsIdenticalGrid(gridBeforeShuffle, board.Grid) ||
            IsAnyMatchExistsInBoard(board) ||
            GetAllPossibleMatches(board).Length <= 0)
        {
            SetGrid(gridBeforeShuffle, board.Grid);
            Shuffle(board);
        }
    }

    private static bool IsIdenticalGrid(int[] oldGrid, int[] newGrid)
    {
        for (var i = 0; i < oldGrid.Length; i++)
        {
            if (oldGrid[i] != newGrid[i])
                return false;
        }

        return true;
    }

    private static void SetGrid(int[] oldGrid, int[] newGrid)
    {
        for (var i = 0; i < oldGrid.Length; i++)
        {
            newGrid[i] = oldGrid[i];
        }
    }

    public static bool IsAnyMatchExistsInBoard(Board board)
    {
        for (var i = 0; i < board.Grid.Length; i++)
        {
            if (CheckMatchHorizontal(board, i) || CheckMatchVertical(board, i))
                return true;
        }

        return false;
    }

    private static bool CheckMatchHorizontal(Board board, int index)
    {
        if (index + 1 >= board.Grid.Length || index % board.Width == board.Width - 1 || index % board.Width == 0 ||
            index - 1 < 0) return false;

        return board.Grid[index] == board.Grid[index + 1] && board.Grid[index] == board.Grid[index - 1];
    }

    private static bool CheckMatchVertical(Board board, int index)
    {
        if (index + board.Width >= board.Grid.Length || index - board.Width < 0) return false;

        return board.Grid[index] == board.Grid[index + board.Width] &&
               board.Grid[index] == board.Grid[index - board.Width];
    }
}