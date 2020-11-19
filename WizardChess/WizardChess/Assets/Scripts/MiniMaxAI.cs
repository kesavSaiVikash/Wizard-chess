using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;
// using Unity.Collections;

public class MiniMaxAI : Player
{
    private const int treeHeight = 4;

    private Move drawMove = null;
    
    private Thread thread;
    

    // constructor
    public MiniMaxAI( Board board, bool isWhite = false) 
    {
        this.isWhite = isWhite;
        this.isAI = true;
        this.board = board;
        this.nextMove = null;
        this.state = 1;
        
    }


    public override void Update() 
    {
        if (state == 1)
        {
            state = 2;
            this.thread = new Thread( ThreadFunc );
            thread.Start();
            // this.nextMove = MiniMax( ref board.matrix, isWhite, 0, -Mathf.Infinity, Mathf.Infinity );
        }
        else 
        {

        }
        
    }

    // tree traversing recursive
    private Move MiniMax( ref int[,] board, bool isMax , int depth, float alpha, float beta )
    {
        List<Move> moves = new List<Move>();
        
        int t = Move.GetPossibleMoves( board, isMax, ref moves );
       
        if (t == 0)
        {
            if (depth == 0)
                this.board.drawn = true;
            
            return null;
        }
        else if (t == -1)
        {
            if (depth == 0)
            {
                this.board.CheckMate = true;
            }
            return null;
        }
        
        
        float minMax = 0.0f;
        int k = 0;
        float eval = 0.0f;

        if (isMax) eval = -Mathf.Infinity;
        else  eval = Mathf.Infinity;

        for (int i = 0; i < moves.Count; i++)
        {
            int temp = Board.ApplyMove( ref board, moves[i] );

            if (depth == treeHeight - 1)
            {
                eval = Evaluator.Evaluate( board );
            } 
            else 
            {   
                Move bestMove = MiniMax( ref board, !isMax, depth + 1, alpha, beta );

                if (bestMove == null)
                {
                    Board.ReverseMove( ref board, moves[i], temp );
                    continue;
                }

                int temp1 = Board.ApplyMove( ref board, bestMove );
                
                eval = Evaluator.Evaluate( board );

                Board.ReverseMove( ref board, bestMove, temp1 );
            }

            if (i == 0) 
            {
                minMax = eval;
                Board.ReverseMove( ref board, moves[i], temp );
                continue;
            }

            if (isMax)
            {
                if ( minMax < eval)
                {
                    minMax = eval;
                    k = i;
                }

                if (alpha < eval)
                    alpha = eval;
                
            }
            else
            {
                if ( minMax > eval)
                {
                    minMax = eval;
                    k = i;
                }

                if (beta > eval)
                    beta = eval;
            }

            

            Board.ReverseMove( ref board, moves[i], temp );

            if (beta <= alpha)
                break;
        }


        if (t == -2)
        {
            if (depth == 0)
                this.drawMove = moves[k];
        }

        return moves[k];
    }

    void ThreadFunc() 
    {
        Debug.Log( "start " +  this.state );
        this.nextMove = MiniMax( ref board.matrix, isWhite, 0, -Mathf.Infinity, Mathf.Infinity );
        
        if (this.drawMove != null)
            if (this.drawMove.Equals( this.nextMove ) )
                board.drawn = true;  

        state = 0;
        // this.thread.Abort();
        Debug.Log( "end" + this.state );

    }

}
