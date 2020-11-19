using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : Player
{

    public RandomAI( Board board, bool isWhite = false )
    {
        this.isWhite = isWhite;
        this.isAI = true;
        this.board = board;
        this.nextMove = null;
    }

    // Update is called once per frame
    public override void Update()
    {
        List<Move> moves = new List<Move>();
        int t = Move.GetPossibleMoves( this.board.matrix, this.isWhite, ref moves );

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

        int i = Random.Range(0, t);

        this.nextMove = moves[i];
        this.state = 0;
    }
}
