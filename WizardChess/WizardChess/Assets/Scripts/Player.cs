using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player 
{
    // for human players
    //  0 = default state
    //  1 = selecting the piece
    //  2 = selecting the destination of the 
    // for AI
    //  0 = default
    public int state;


    public bool isAI = true; // true for ai player
    public  bool isWhite;
    public Move nextMove;

    public Board board;

    public abstract void Update();

}
