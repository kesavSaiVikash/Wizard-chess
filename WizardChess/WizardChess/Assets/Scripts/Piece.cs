using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Piece : MonoBehaviour {

    // 1 = pawn
    // 2 = bishop
    // 3 = knight
    // 4 = rook
    // 5 = queen
    // 6 = king
    // negative for black positive for white
    public int type;

    public int[] square = new int[2]{0, 0};

    public float speed = 4.0f;
    public float rotateSpeed = 20.0f;

    public static Board board;

    public float attackTime = 1.15f;

    public GameObject fractured;

    private float timer = 0.0f;
    private Quaternion targetRotation;

    private GameObject deactivated = null;
    private GameObject deactivated1 = null;

    // 0 = idle
    // 1 = moving
    // 2 = moving to attack
    // 3 = attacking
    // 4 = rotating
    // 5 = deactivated
    public int state = 0;

    // next state  after rataion
    private int nextState = 0;

    public bool knightMoving = false;

    int[] target;
    int[] target2;  // only for knight
    int[] attack;

    Animator animator;
    NavMeshAgent agent;
    AudioSource audioSource;

     // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        agent.updateRotation = false;
        gameObject.tag = "piece";
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = speed;
        if (state == 1 || state == 2)
        {
            HandlePieceCollision();

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if ((Board.SquareToPos( target ) - transform.position).magnitude <= 0.1f )
            {
                square[0] = target[0];
                square[1] = target[1];
                animator.SetBool( "move", false );

                if ( state == 2)
                {
                    state = 3;
                }
                else
                {
                    board.busy = false;
                    state = 0;

                    // special case knight
                    if ((type == 3 || type == -3) && knightMoving)
                    {
                        target = target2;
                        state = 1;
                        knightMoving = false;
                        board.busy = true;
                        RotateTowards( target , 1);              
                    }
                    else 
                    {
                        if (deactivated != null)
                        {
                            deactivated.SetActive( true );
                            deactivated = null;
                        }

                        if (deactivated1 != null)
                        {
                            deactivated1.SetActive( true );
                            deactivated1 = null;
                        }
                        board.pieces[square[0], square[1]] = this;
                    }
                   
                }

                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

              
                
            }
        }
        else if (state == 3)
        {
            int tag = animator.GetCurrentAnimatorStateInfo(0).tagHash;

            if (timer >= attackTime)
            {
                // GameObject.Destroy( board.pieces[attack[0], attack[1]].gameObject, 0.0f );
                board.pieces[attack[0], attack[1]].Die();
                state = 1;
                target = attack;
                agent.SetDestination( Board.SquareToPos( target ));
                
                square[0] = attack[0];
                square[1] = attack[1];
                timer = 0.0f;
                animator.SetBool( "attack", false );
            }
            else 
            {
                if (timer == 0.0f)
                {   
                    RotateTowards( attack, 3);
                }
               

                timer += Time.deltaTime;
            }
        }
        else if (state == 4)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                                    targetRotation, rotateSpeed * Time.deltaTime);

            if ( Math.Abs( Quaternion.Angle( transform.rotation, targetRotation )) <= 2.5f)
            {
                transform.rotation = targetRotation;
                agent.SetDestination( Board.SquareToPos( target ));

                if (nextState == 3)
                {
                    animator.SetBool( "attack", true );
                    state = 3;                
                }
                else
                {
                    state = nextState;
                }

            }
        }
    }

    // execute the given move ( go to the end of the move)
    public void Move( Move move )
    {
        switch (move.type)
        {
            case MoveType.NORMAL:
                if (type == 3 || type == -3)
                {
                    target = new int[]{move.end[0], square[1]};
                    target2 = move.end;
                    knightMoving = true;
                }
                else
                    target = move.end;

                RotateTowards( target , 1);
                board.busy = true;
                board.pieces[square[0], square[1]] = null;

     
                break;

            case MoveType.CAPTURE:
                target = GetAttakingSquare( move );
                RotateTowards( target , 2);
                attack = move.end;
                board.busy = true;
                board.pieces[square[0], square[1]] = null;
                break;

            default:
                break;
        }
    }


    // get the square to attack the piece at the end of the move
    public int[] GetAttakingSquare( Move move )
    {
        if (move.type != MoveType.CAPTURE)
            return null;

        int x0 = square[0];
        int y0 = square[1];
        
        int x1, y1, k1, k2;

        if (type == 3 || type == -3)
        {
            if (move.end[0] - x0 == 1 || move.end[0] - x0 == -1)
                return new int[]{x0, move.end[1]};

            else
                return new int[]{move.end[0], y0};

        }
        else
        {
            x1 = move.end[0];
            y1 = move.end[1];

            k1 = 0;
            k2 = 0;

            if (x0 < x1) k1 = -1;
            if (x0 > x1) k1 = 1;

            if (y0 < y1) k2 = -1;
            if (y0 > y1) k2 = 1;
        }


        return new int[]{x1 + k1, y1 + k2};
    }

    // destroy in to pieces
    public void Die()
    {
        GameObject.Destroy( gameObject, 0.0f);
        GameObject.Destroy( Instantiate( fractured, transform.position, transform.rotation ), 6.0f);
    }

    private void RotateTowards( int[] targetSquare, int nextState )
    {
        if (targetSquare[0] == square[0] && targetSquare[1] == square[1])
        {
            this.state = nextState;
            this.nextState = nextState;
            return;
        }
        
        if (nextState == 1 || nextState == 2) 
        {
            animator.SetBool( "move", true );
        }

        Vector3 towards = Board.SquareToPos( targetSquare );
        targetRotation = Quaternion.LookRotation(towards - transform.position);
        this.state = 4;   
        this.nextState = nextState;
    }

    // if the piece going through a square with piece (knight move and capture for any piece) 
    // deactivate it 
    private void HandlePieceCollision( )
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        //  Debug.DrawRay( transform.position+ new Vector3(0, 2, 0), fwd * 1.5f, Color.red );
        if (Physics.Raycast( transform.position + new Vector3(0, 2, 0), fwd , out hit, 1.5f ))
        {
            if (hit.transform.gameObject.tag == "piece" )
            {
                if (deactivated != null)
                {
                    deactivated1 = hit.transform.gameObject;
                    deactivated1.SetActive( false );
                }
                else 
                {
                    // deactivated.SetActive( true );

                    deactivated = hit.transform.gameObject;
                    deactivated.SetActive( false );
                }
            }
        }     
    }

    

}
