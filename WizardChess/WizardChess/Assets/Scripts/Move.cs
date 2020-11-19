using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    NORMAL,
    CAPTURE,
    CATSLE,
}

public class Move 
{
    // move is from start to end squares
    public int[] start; 
    public int[] end;

    public MoveType type;

    // constructor from given 2 squares
    public Move( int[] start, int[] end , MoveType type = MoveType.NORMAL ) 
    {
        this.start = start;
        this.end = end;
        this.type = type;
    }

    // constructor from i and j values of start and end squares
    public Move( int i0, int j0, int i1, int j1, MoveType type = MoveType.NORMAL)
    {
        this.start = new int[]{i0, j0};
        this.end = new int[]{i1, j1};
        this.type = type;
    }

    public bool Equals(Move move)
    {
        return move.start[0] == this.start[0] && move.start[1] == this.start[1] &&
                move.end[0] == this.end[0] && move.end[1] == this.end[1];
    }

    // add all possible moves according to the board
    // calculate for white or black input moves list must be empty
    // return 
    //      > 0 no of possible moves posible moves
    //      0 draw
    //      -1 checkmate
    //      -2 only 2 kings left
    public static int GetPossibleMoves( int[,] boardMatrix, bool forWhite, ref List<Move> moves ) 
    {
        int count = 0;

        for (int i = 0; i < 8; i++)   
        {
            for (int j = 0; j < 8; j++)
            {
                int pieceType = boardMatrix[i, j];

                if (pieceType != 0) count++;

                if ((forWhite && pieceType > 0) || ( !forWhite && pieceType < 0))
                    GetPossibleMovesPiece( i, j, pieceType, boardMatrix, ref moves );
            }
        }

        if (moves.Count == 0)
        {
            if (IsCheck( boardMatrix, forWhite ))
                return -1;

            return 0;
        }
        if (count == 2)  // only 2 kings
            return -2;

        return moves.Count;
    }

    public static int[] GetKingPos( int[,] boardMatrix, bool forWhite )
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0; j < 8; j++)
            {
                int pieceType = boardMatrix[i, j];

                // for king
                if ((forWhite && pieceType == 6) || (!forWhite && pieceType == -6))
                    return new int[]{i, j}; 
                
            }
        }

        return null;
    }

    // add the possible moves for a pawn at i, j for the boardMatrix 
    private static void GetMovesPawn( int i, int j, int k,  int[,] boardMatrix, ref List<Move> moves )
    {
        bool forWhite = k > 0;

        if (j > 0 && j < 7)
        {
            // check the square in front is free
            if (boardMatrix[i, j + k] == 0)
                AddMove( new Move( i, j, i, j + k), boardMatrix, forWhite, ref moves );

            // can skip one square at the beginning
            if (k == 1)
            {   
                if (j == 1 && boardMatrix[i, 3] == 0)
                    AddMove( new Move( i, 1, i, 3 ), boardMatrix, forWhite, ref moves);
            }
            else
            {
                if (j == 6 && boardMatrix[i, 4] == 0)
                    AddMove( new Move( i, 6, i, 4 ), boardMatrix, forWhite, ref moves);
            }
            
            // check for opponent in the next squares that is diagonal ( to capture )
            if (i > 0)
            {
                int temp = boardMatrix[i - 1, j + k] * k;
                if ( temp < 0 && temp * (-1) != 6)      
                    AddMove( new Move( i, j, i - 1, j + k, MoveType.CAPTURE ), boardMatrix, forWhite, ref moves);
            }
               

            if (i < 7)
            {
                int temp = boardMatrix[i + 1, j + k] * k;
                if (temp < 0 && temp * (-1) != 6)      
                    AddMove( new Move( i, j, i + 1, j + k, MoveType.CAPTURE ), boardMatrix, forWhite, ref moves);
            }
                                                    
        }
    }


    // add the possible moves for a bishop at i, j for the boardMatrix 
    private static void GetMovesBishop( int i, int j, int k, int[,] boardMatrix, ref List<Move> moves )
    {
        bool forWhite = k > 0;

        int i_ = i + 1;
        int j_ = j + 1;

        int [,] p = {{1, 1}, {1, -1}, {-1, 1}, {-1, -1}};

        // go through diagonal squars
        for (int x = 0; x < 4; x++)
        {
            i_ = i + p[x, 0];
            j_ = j + p[x, 1];

            while (true)
            {
                // edges of the board
                if (i_ < 0 || j_ < 0 || i_ > 7 || j_ > 7)
                    break;

                int temp = boardMatrix[i_, j_] * k;
    
                if (temp == 0)   // if empty 
                    AddMove( new Move( i, j, i_, j_ ), boardMatrix, forWhite, ref moves );

                else    // not empty
                {
                    if (temp < 0 && temp != -6)   // for opponent piece
                        AddMove( new Move( i, j, i_, j_, MoveType.CAPTURE ), boardMatrix, forWhite, ref moves);
                        
                    break;
                }     

                i_ += p[x, 0];
                j_ += p[x, 1];     
            }

           
        }
    }

    private static void GetMovesKing( int i, int j, int k, int[,] boardMatrix, ref List<Move> moves)
    {
        bool forWhite = k > 0;

        int [,] p = {{1, 1}, {1, -1}, {-1, 1}, {-1, -1}, 
                     {0, 1}, {0, -1}, {1, 0}, {-1, 0}};

        int i_, j_;

        for (int x = 0; x < 8; x++)
        {
            i_ = i + p[x, 0];
            j_ = j + p[x, 1];

            if (i_ < 0 || j_ < 0 || i_ > 7 || j_ > 7)
                continue;

            int temp = boardMatrix[i_, j_] * k;

            if (temp == 0) // if emopty
                AddMove( new Move( i, j, i_, j_), boardMatrix, forWhite, ref moves );

            else if (temp < 0 && temp != -6)
                AddMove( new Move( i, j, i_, j_, MoveType.CAPTURE), boardMatrix, forWhite, ref moves );
        }
    }

    private static void GetMovesKnight( int i, int j, int k, int[,] boardMatrix, ref List<Move> moves)
    {
        bool forWhite = k > 0;

        int[,] p = {{1, 2}, {1, -2}, {-1, 2}, {-1, -2}, {2, 1}, {-2, 1}, {2, -1}, {-2, -1} };
        
        int i_ ;
        int j_ ;


        for (int x = 0; x < 8; x++)
        {
            i_ = i + p[x, 0];
            j_ = j + p[x, 1];

            if (i_ < 0 || j_ < 0 || i_ > 7 || j_ > 7)
                continue;

            int temp = boardMatrix[i_, j_] * k;

            if (temp == 0) // if emopty
                AddMove( new Move( i, j, i_, j_), boardMatrix, forWhite, ref moves );

            else if (temp < 0 && temp != -6)
                AddMove( new Move( i, j, i_, j_, MoveType.CAPTURE ), boardMatrix, forWhite, ref moves );
        }
    }


    private static void GetMovesRook( int i, int j, int k, int[,] boardMatrix, ref List<Move> moves)
    {
        bool forWhite = k > 0;

        int i_ = i + 1;
        int j_ = j + 1;

        int [,] p = {{1, 0}, {-1, 0}, {0, 1}, {0, -1}};

        // go through diagonal squars
        for (int x = 0; x < 4; x++)
        {
            i_ = i + p[x, 0];
            j_ = j + p[x, 1];

            while (true)
            {
                // edges of the board
                if (i_ < 0 || j_ < 0 || i_ > 7 || j_ > 7)
                    break;

                int temp = boardMatrix[i_, j_] * k;
    
                if (temp == 0)   // if empty 
                    AddMove( new Move( i, j, i_, j_ ), boardMatrix, forWhite, ref moves );

                else    // not empty
                {
                    if (temp < 0 && temp != -6)   // for opponent piece
                        AddMove( new Move( i, j, i_, j_, MoveType.CAPTURE ), boardMatrix, forWhite, ref moves);
                        
                    break;
                }     

                i_ += p[x, 0];
                j_ += p[x, 1];     
            }

           
        }
    }

    private static void GetMovesQueen( int i, int j, int k, int[,] boardMatrix, ref List<Move> moves)
    {
        GetMovesBishop(i, j, k, boardMatrix, ref moves );
        GetMovesRook(i, j, k, boardMatrix, ref moves );
    }


    // add the possible moves for the given piecetype at given square (i, j)
    public static void GetPossibleMovesPiece( int i, int j, int pieceType, int[,] boardMatrix, ref List<Move> moves )
    {
        int k = (pieceType > 0)? 1 : -1;
      
        switch (k * pieceType)
        {
            // for pawn
            case 1:
                GetMovesPawn( i, j, k, boardMatrix, ref moves );
                break;
            
            // for bishop
            case 2:
                GetMovesBishop( i, j, k, boardMatrix, ref moves );
                break;

            // for knight
            case 3:
                GetMovesKnight( i, j, k, boardMatrix, ref moves );
                break;

            // for rook
            case 4:
                GetMovesRook( i, j, k, boardMatrix, ref moves );
                break;

            // for queen
            case 5:
                GetMovesQueen( i, j, k, boardMatrix, ref moves );
                break;

            // for king
            case 6:
                GetMovesKing( i, j, k, boardMatrix, ref moves );
                break;

            default:
                break;
        }
    }

    public static void GetPossibleMovesPiece( int[] square, int pieceType, int[,] boardMatrix, ref List<Move> moves )
    {
        GetPossibleMovesPiece( square[0], square[1], pieceType, boardMatrix, ref moves );
    }

    // true if the king is check 
    public static bool IsCheck( int[,] boardMatrix, bool forWhite )
    {
        int k = forWhite ? 1 : -1;

        int[] kingPos = GetKingPos( boardMatrix, forWhite );

        if (kingPos == null) return false;

        int i = kingPos[0];
        int j = kingPos[1];

        // for check from pawn
        if (j + k < 8 && j + k >= 0)
        {
            if (i > 0)
            {
                int temp = boardMatrix[i - 1, j + k] * k;
                if ( temp == -1 )      
                    return true;
            }
                
            if (i < 7)
            {
                int temp = boardMatrix[i + 1, j + k] * k;
                if (temp == -1)      
                    return true;
            }
        }

        // check diagonals 
        int [,] p = {{1, 1}, {1, -1}, {-1, 1}, {-1, -1}, 
                     {0, 1}, {0, -1}, {1, 0}, {-1, 0}};
        int i_, j_;

        // go through diagonal and direct squares
        for (int x = 0; x < 8; x++)
        {
            i_ = i + p[x, 0];
            j_ = j + p[x, 1];
            int temp;

            if (i_ >= 0  && j_ >= 0 && i_ <= 7 && j_ <= 7)
            {
                temp = boardMatrix[i_, j_] * k;

                if (temp == -6) 
                    return true;
            }

            while (true)
            {
                // edges of the board
                if (i_ < 0 || j_ < 0 || i_ > 7 || j_ > 7)
                    break;

                temp = boardMatrix[i_, j_] * k;
                
                if (x < 4)  // diagonal
                {
                    if (temp == -2 || temp == -5)   // only queen and bishop can check diagonally   
                        return true;  
                }
                else    // direct 
                {
                    if (temp == -4 || temp == -5)   // only queen and rook    
                        return true;                     
                }

                i_ += p[x, 0];
                j_ += p[x, 1];  
                
                if (temp != 0)
                    break;   
            }   
        }

        p = new  int[,]{{1, 2}, {1, -2}, {-1, 2}, {-1, -2}, {2, 1}, {-2, 1}, {2, -1}, {-2, -1} };
        
        // Check fromknight
        for (int x = 0; x < 8; x++)
        {
            i_ = i + p[x, 0];
            j_ = j + p[x, 1];

            if (i_ < 0 || j_ < 0 || i_ > 7 || j_ > 7)
                continue;

            int temp = boardMatrix[i_, j_] * k;

            if (temp == -3)
                return true;
        }
            


        return false;
    }
   
    // add move to the given list considering the check
    public static void AddMove( Move move, int[,] boardMatrix, bool forWhite, ref List<Move> moves )
    {
        int temp = Board.ApplyMove( ref boardMatrix, move );

        // if the king is check after applying the move, move is not valid
        if (IsCheck( boardMatrix, forWhite ))
        {
            Board.ReverseMove( ref boardMatrix, move, temp );
            return;
        }
        Board.ReverseMove( ref boardMatrix, move, temp );

        moves.Add( move );
    }


    public override string ToString()
    {
        return "(" + this.start[0] + ", " + this.start[1] + ") -> " +
                        "(" + this.end[0] + ", " + this.end[1] + ")";
    }
}
