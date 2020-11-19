using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluator 
{
    private static double[,] pawnEvalWhite =
    {
        {0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0},
        {5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0},
        {1.0,  1.0,  2.0,  3.0,  3.0,  2.0,  1.0,  1.0},
        {0.5,  0.5,  1.0,  2.5,  2.5,  1.0,  0.5,  0.5},
        {0.0,  0.0,  0.0,  2.0,  2.0,  0.0,  0.0,  0.0},
        {0.5, -0.5, -1.0,  0.0,  0.0, -1.0, -0.5,  0.5},
        {0.5,  1.0, 1.0,  -2.0, -2.0,  1.0,  1.0,  0.5},
        {0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0}
    };

    private static double[,] pawnEvalBlack;

    private static double[,] knightEval =
        {
            {-5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0},
            {-4.0, -2.0,  0.0,  0.0,  0.0,  0.0, -2.0, -4.0},
            {-3.0,  0.0,  1.0,  1.5,  1.5,  1.0,  0.0, -3.0},
            {-3.0,  0.5,  1.5,  2.0,  2.0,  1.5,  0.5, -3.0},
            {-3.0,  0.0,  1.5,  2.0,  2.0,  1.5,  0.0, -3.0},
            {-3.0,  0.5,  1.0,  1.5,  1.5,  1.0,  0.5, -3.0},
            {-4.0, -2.0,  0.0,  0.5,  0.5,  0.0, -2.0, -4.0},
            {-5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0}
        };

    private static double[,] bishopEvalWhite = {
        { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0},
        { -1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0},
        { -1.0,  0.0,  0.5,  1.0,  1.0,  0.5,  0.0, -1.0},
        { -1.0,  0.5,  0.5,  1.0,  1.0,  0.5,  0.5, -1.0},
        { -1.0,  0.0,  1.0,  1.0,  1.0,  1.0,  0.0, -1.0},
        { -1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0, -1.0},
        { -1.0,  0.5,  0.0,  0.0,  0.0,  0.0,  0.5, -1.0},
        { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0}
    };

    private static double[,] bishopEvalBlack;

    private static double[,] rookEvalWhite = {
        {  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0},
        {  0.5,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5},
        { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
        { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
        { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
        { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
        { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
        {  0.0,   0.0, 0.0,  0.5,  0.5,  0.0,  0.0,  0.0}
    };

    private static double[,] rookEvalBlack;

    private static double[,] evalQueen =
        {
        { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0},
        { -1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0},
        { -1.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0},
        { -0.5,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5},
        {  0.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5},
        { -1.0,  0.5,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0},
        { -1.0,  0.0,  0.5,  0.0,  0.0,  0.0,  0.0, -1.0},
        { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0}
    };

    private static double[,] kingEvalWhite = {

        { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
        { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
        { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
        { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
        { -2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0},
        { -1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0},
        {  2.0,  2.0,  0.0,  0.0,  0.0,  0.0,  2.0,  2.0 },
        {  2.0,  3.0,  1.0,  0.0,  0.0,  1.0,  3.0,  2.0 }
    };

    private static double[,] kingEvalBlack;

    public static void Init()
    {
        pawnEvalBlack = ReverseArray(pawnEvalWhite);
        bishopEvalBlack = ReverseArray(bishopEvalWhite);
        rookEvalBlack = ReverseArray(rookEvalWhite);
        kingEvalBlack = ReverseArray(kingEvalWhite);
    }

    private static double[,] ReverseArray( double[,] array )
    {
        double[,] revArray = new double[8,8];

        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                revArray[i, j] = array[7 - i, j];
            }
        }

        return revArray;
    }

   public static float Evaluate( int[,] board )
   {
       double sum = 0.0;

        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int piece = board[j, i];

                int k = piece > 0 ? 1 : -1;

                piece *= k;

                if (piece == 5)
                {
                    sum += 90.0 + evalQueen[i, j];
                }
                else if (piece == 3)
                {
                    sum += 30.0 + knightEval[i, j];
                }
                else 
                {
                    if( k == 1)
                    {
                        if (piece == 1)
                            sum +=  (10.0 + pawnEvalWhite[i, j]);

                        else if (piece == 2)
                            sum +=  (30.0 + bishopEvalWhite[i, j]);

                        else if (piece == 4)
                            sum +=  (50.0 + rookEvalWhite[i, j]);             

                        else if (piece == 6)
                            sum += 900.0 + kingEvalWhite[i, j];
                    }
                    else 
                    {
                        if (piece == 1)
                            sum -=  (10.0 + pawnEvalBlack[i, j]);

                        else if (piece == 2)
                            sum -=  (30.0 + bishopEvalBlack[i, j]);

                        else if (piece == 4)
                            sum -=  (50.0 + rookEvalBlack[i, j]);             

                        else if (piece == 6)
                            sum -= 900.0 + kingEvalBlack[i, j];
                    }

                }

            }
        }
        return (float)sum;
   }
}
