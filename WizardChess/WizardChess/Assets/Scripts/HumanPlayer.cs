using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    private List<Move> possibleMoves;
    private int moveId;

    public HumanPlayer( Board board, bool isWhite = true ) 
    {
        this.isWhite = isWhite;
        this.isAI = false;
        this.state = 0;
        this.nextMove = new Move(0, 0, 0, 0);
        this.board = board;
        this.possibleMoves = null;
    }


    public override void Update() 
    {
        if (state == 1) // select the peice to move
        {
            int[] square = board.GetRayHitSquare();
                    
            int x = square[0];
            int y = square[1];

            if (this.possibleMoves == null)
            {
                this.possibleMoves = new List<Move>();
                int t = Move.GetPossibleMoves( this.board.matrix, this.isWhite, ref this.possibleMoves );

                if (t == -1 ) 
                {     
                    board.CheckMate = true;
                    return;
                }
                else if (t == 0 || t == -2)
                {
                    board.drawn = true;
                    return;
                }
            }


            if (x != -1 && y != -1)
            {
                if (this.isWhite && board.matrix[x, y] > 0 || !this.isWhite && board.matrix[x, y] < 0)
                {
                    this.moveId = 0;

                    foreach (Move move in this.possibleMoves)
                    {
                        if (move.start[0] == x && move.start[1] == y)
                        {
                            board.HighlightSqure( square, 1);
                            
                            if (Input.GetMouseButtonDown(0))
                            {
                                state = 2;
                                this.nextMove.start = square;
                            }
                        }
                        this.moveId++;
                    }
                }
                else 
                {
                    board.CleareHighlightSquare( 1 );
                }
            }
            else
            {
                board.CleareHighlightSquare( 1 );
            }
        }
        else if (this.state == 2) // select the destination
        {
            this.possibleMoves = null;
            int[] square = board.GetRayHitSquare();
                    
            int x = square[0];
            int y = square[1];

            if (x != -1 && y != -1)
            {          
                int starti = this.nextMove.start[0];
                int startj = this.nextMove.start[1];
                int pieceType = this.board.matrix[starti, startj];

                List<Move> possibleMovesPiece = new List<Move>();
                Move.GetPossibleMovesPiece( starti, startj, pieceType, board.matrix, ref possibleMovesPiece  );

                foreach (Move move in possibleMovesPiece)
                {
                    if (this.isWhite && x == move.end[0] && y == move.end[1])
                    {
                        board.HighlightSqure( square, 2 );

                        if (Input.GetMouseButtonDown(0))
                        {
                            state = 0;
                            this.nextMove = move;
                            board.CleareHighlightSquare( 1 );
                            board.CleareHighlightSquare( 2 );
                        }
                        break;

                    }
                    else
                    {
                        board.CleareHighlightSquare( 2 );
                    }
                }
                
            }
            else
            {
                board.CleareHighlightSquare( 2 );
            }
        }
        else
        {
            this.state = 1;
        }
        
    }
}
