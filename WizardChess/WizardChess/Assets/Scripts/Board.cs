using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    // represents the board as a matrix 
    // 0 if empty negative vaues for black peices
    // positive values for whietes
    public int[,] matrix; 

    // stores the pieces
    public Piece[,] pieces;

    public Piece whitePawn0;
    public Piece whitePawn1;
    public Piece whitePawn2;
    public Piece whitePawn3;
    public Piece whitePawn4;
    public Piece whitePawn5;
    public Piece whitePawn6;
    public Piece whitePawn7;
    public Piece whiteKnight0;
    public Piece whiteKnight1;
    public Piece whiteBishop0;
    public Piece whiteBishop1;
    public Piece whiteRook0;
    public Piece whiteRook1;
    public Piece whiteQueen;
    public Piece whiteKing;

    public Piece blackPawn0;
    public Piece blackPawn1;
    public Piece blackPawn2;
    public Piece blackPawn3;
    public Piece blackPawn4;
    public Piece blackPawn5;
    public Piece blackPawn6;
    public Piece blackPawn7;
    public Piece blackKnight0;
    public Piece blackKnight1;
    public Piece blackBishop0;
    public Piece blackBishop1;
    public Piece blackRook0;
    public Piece blackRook1;
    public Piece blackQueen;
    public Piece blackKing;


    public GameObject squreSelect1;
    public GameObject squreSelect2;

    public bool CheckMate;
    // public bool blackCheckMate;
    public bool drawn;

    private Player player1;
    private Player player2;


    // true while a peice is moveing or player is making decision
    public bool busy; 
    
    public int turn; // current turn 1 or 2

    public static float speedFactor = 1.0f;

    // game types
    //  0 = human player vs minimax ai
    //  1 = human Player vs random ai
    //  2 = minimax ai vs minimax ai
    public static int gameType = 0;

    public static string finishState = "White Wins !!!";


    // Start is called before the first frame update
    void Start()
    {
        busy = false;
        turn = 1;
        matrix = new int[8, 8];
        pieces = new Piece[8, 8];
        Piece.board = this;
        InitGame();
    }


    // Update is called once per frame
    void Update()
    {
        if (!busy)
        {
            if (turn == 1)  // turn 1 is 
            {
                player1.Update();

                if (CheckMate)
                {
                    finishState = "Black Wins !!!";
                    Debug.Log( "Black wins!!!" );
                    SceneManager.LoadScene( "Finish" );
                    return;
                }

                if (drawn)
                {
                    finishState = "Game Drawn !!!";
                    Debug.Log( "Game Drawn" );
                    SceneManager.LoadScene( "Finish" );
                    return;
                }
                
                if (player1.state == 0)
                {
                    ApplyMove( player1.nextMove );
                    if (player1.isAI)
                        player1.state = 1;

                    turn = 2;
                }
            }
            else
            {
                player2.Update();

                if (CheckMate)
                {
                    Debug.Log( "White wins!!!" );
                    SceneManager.LoadScene( "Finish" );
                    return;
                }

                if (drawn)
                {
                    finishState = "Game Drawn !!!";
                    Debug.Log( "Game Drawn" );
                    SceneManager.LoadScene( "Finish" );
                    return;
                }
                // ApplyMove( ref matrix, player2.nextMove );

               
                // foreach( Move m in  Move.GetPossibleMoves( matrix, false ))
                //     Debug.Log( m );
                 if (player2.state == 0)
                {
                    // Debug.Log( player1.nextMove );
                    ApplyMove( player2.nextMove );

                    if (player2.isAI)
                        player2.state = 1;

                    turn = 1;
                }
                // ApplyMove( player2.nextMove );
                // PrintBoard( matrix );
                // Debug.Log( player2.nextMove );
                // turn = 1;
            }
        }

           

    }


    public void InitGame()
    {
        switch (gameType)  
        {
            case 0:
                player1 = new HumanPlayer( this );
                player2 = new MiniMaxAI( this );
                speedFactor = 1.0f;
                break;

            case 1:
                player1 = new HumanPlayer( this );
                player2 = new RandomAI( this );
                speedFactor = 1.0f;
                break;

            case 2:
                player1 = new MiniMaxAI( this, true );
                player2 = new MiniMaxAI( this );
                speedFactor = 1.5f;
                break;

            default:
                break;
        }
        
        AddPiece( 0, 1, ref whitePawn0 );
        AddPiece( 1, 1, ref whitePawn1 );
        AddPiece( 2, 1, ref whitePawn2 );
        AddPiece( 3, 1, ref whitePawn3 );
        AddPiece( 4, 1, ref whitePawn4 );
        AddPiece( 5, 1, ref whitePawn5 );
        AddPiece( 6, 1, ref whitePawn6 );
        AddPiece( 7, 1, ref whitePawn7 );

        AddPiece( 2, 0, ref whiteBishop0 );
        AddPiece( 5, 0, ref whiteBishop1 );

        AddPiece( 1, 0, ref whiteKnight0 );
        AddPiece( 6, 0, ref whiteKnight1 );

        AddPiece( 0, 0, ref whiteRook0 );
        AddPiece( 7, 0, ref whiteRook1 );

        AddPiece( 3, 0, ref whiteQueen );
        AddPiece( 4, 0, ref whiteKing );


        AddPiece( 0, 6, ref blackPawn0 );
        AddPiece( 1, 6, ref blackPawn1 );
        AddPiece( 2, 6, ref blackPawn2 );
        AddPiece( 3, 6, ref blackPawn3 );
        AddPiece( 4, 6, ref blackPawn4 );
        AddPiece( 5, 6, ref blackPawn5 );
        AddPiece( 6, 6, ref blackPawn6 );
        AddPiece( 7, 6, ref blackPawn7 );

        AddPiece( 2, 7, ref blackBishop0 );
        AddPiece( 5, 7, ref blackBishop1 );

        AddPiece( 1, 7, ref blackKnight0 );
        AddPiece( 6, 7, ref blackKnight1 );

        AddPiece( 0, 7, ref blackRook0 );
        AddPiece( 7, 7, ref blackRook1 );

        AddPiece( 3, 7, ref blackQueen );
        AddPiece( 4, 7, ref blackKing );

        Evaluator.Init();
    }

    // apply the given move to the given board
    // return = the value of the piece (or 0 if empty) that was in the 
    // end move squar 
    public static int ApplyMove( ref int[,] board, Move move )
    {
        int temp = board[move.end[0], move.end[1]];
        board[move.end[0], move.end[1]] = board[move.start[0], move.start[1]];
        board[move.start[0], move.start[1]] = 0;

        return temp;
    }


    // apply move to this board
    public void ApplyMove( Move move ) 
    {
        int x = move.start[0];
        int y = move.start[1];

        Piece piece = pieces[x, y];

        if (piece == null)
        {
            return;
        }
        piece.Move( move );
        ApplyMove( ref matrix, move );
    }


    // reverse the applied move
    public static void ReverseMove( ref int[,] board, Move move, int old_piece )
    {
        board[move.start[0], move.start[1]] = board[move.end[0], move.end[1]];
        board[move.end[0], move.end[1]] = old_piece;
    }


     // get the hit object from the camera to world
    // also can get the hit position 
    GameObject GetRayHitObject( ref Vector3 hitPos ) 
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

        // get the hitting posistion of the camera to world ray
        if ( Physics.Raycast(ray, out hit) ) {
            hitPos = hit.point;
            // Debug.DrawLine( hitPos, new Vector3( 10, 10, 10), Color.red, 10.0f );

            return hit.transform.gameObject;      
        }

        return null;
    }


    public int[] GetRayHitSquare() 
    {
        Vector3 hitPos = new Vector3(0, 0, 0);
        GameObject hitObject = GetRayHitObject( ref hitPos );     
        int x, z;
        
        if (hitObject == null) 
        {
            x = -1;
            z = -1;
        }
        else if (hitObject.tag == "board") 
        {
            x = GetSqureCoord( hitPos.x );
            z = GetSqureCoord( hitPos.z );      
        }
        else if (hitObject.tag == "piece" )
        {
           Piece piece = hitObject.GetComponent<Piece>();
           x = piece.square[0];
           z = piece.square[1];
        }    
        else
        {
            x = -1;
            z = -1;
        }

        return new int[]{x, z};
    }


    public static int GetSqureCoord( float value ) 
    {
        int res = (int)(value / 2.0f);


        if (value > 16.0f || value < 0.0f)
            return -1;

        return res;
    }


    public static float SquareCoordToPos( int squareCoord )
    {
        return (float)( squareCoord * 2 + 1 );
    }


    public static Vector3 SquareToPos( int[] square )
    {
        float x = SquareCoordToPos( square[0] );
        float z = SquareCoordToPos( square[1] );

        return new Vector3( x, 0.0f, z );
    }


    public void HighlightSqure( int[] square, int select )
    {
        if (select == 1)
        {
            squreSelect1.SetActive( true );
            squreSelect1.transform.position 
                    = new Vector3( SquareCoordToPos( square[0]), 0.01f, 
                                    SquareCoordToPos( square[1]) );
        }
        else
        {
            squreSelect2.SetActive( true );
            squreSelect2.transform.position 
                    = new Vector3( SquareCoordToPos( square[0]), 0.01f, 
                                    SquareCoordToPos( square[1]) );
        }
    }


    public void CleareHighlightSquare( int select )
    {
        if (select == 1)
            squreSelect1.SetActive( false );

        else
            squreSelect2.SetActive( false );

    }


    public static void PrintBoard( int[,] board )
    {
        for (int i = 0; i < 8; i++)
        {
            string o = "";

            for (int j = 0; j < 8; j++)
            {
                o += board[i, j] + " ";
            }
            Debug.Log( o );
        }   
    }


    public static void PrintBoard( Piece[,] board )
    {
        for (int i = 0; i < 8; i++)
        {
            string o = "";

            for (int j = 0; j < 8; j++)
            {
                o += ((board[i, j] != null) ? board[i, j].type : 0) + " ";
            }
            Debug.Log( o );
        }   
    }

    public void AddPiece( int i, int j, ref Piece piece )
    {
        pieces[i, j] = piece;
        piece.square[0] = i;
        piece.square[1] = j;
        piece.speed *= speedFactor;
        piece.rotateSpeed *= speedFactor;
        matrix[i, j] = piece.type;
        piece.transform.position = SquareToPos( new int[]{i, j});
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene( "Menu" );
    }
}
