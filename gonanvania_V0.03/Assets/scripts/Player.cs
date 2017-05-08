using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { Idle, Moving, InAir };
public enum AimState { Right, Left, Up, Down, DiagUpRight, DiagUpLeft, DiagDownRight, DiagDownLeft };

public class Player : MonoBehaviour, IReaction {

    public PlayerState currentState;
    public AimState currentAimState;

    public GameObject whip;

    public GameObject whipRight;
    public GameObject whipLeft;
    public GameObject whipUp;
    public GameObject whipDown;
    public GameObject whipDiagUpRight;
    public GameObject whipDiagUpLeft;
    public GameObject whipDiagDownRight;
    public GameObject whipDiagDownLeft;
    public GameObject shuriken;
    public GameObject upperBody;

    Rigidbody rb;

    public GameObject crouchCollider;
    CapsuleCollider fullCollider;

    public float hSpeed = 10;
    public float vSpeed = 600;

    public float groundCheckDist = 0.5f;

    public LayerMask groundOnly;

    public int hp;

    public bool canWhip = true;
    bool crouchWhip;
    bool isCrouching;

    float tickTime;


    public bool canMove = true;



    public float extraGravity;

    float addedGravity;

    bool hasJumped;

    public bool canBeHit = true;

    public bool whipping;

    public Slider healthSlider; //need to actually make one of these

    Vector3 velocity;

    float horizontalAxis;
    float verticalAxis;

    void Start () {

        currentState = PlayerState.Idle;
        currentAimState = AimState.Right;
        rb = GetComponent<Rigidbody>();
        //whipScript = whip.GetComponent<Whip>();
        fullCollider = GetComponent<CapsuleCollider>();
    }
	
    public void Activate() {

    }

	public void TakeDamage() {
        if (canBeHit) {
            hp--;
            print("player took a hit");
            Invoke("CanTakeAHit", 2);
        }
    }
        

    void CanTakeAHit() {
        canBeHit = true;
    }

    public void React() {
        //print("player reacted");
    }

    public void CrouchEnd() {
        if (crouchWhip) {
            whip.transform.position += new Vector3(0, 0.5f, 0);
            crouchWhip = false;
        }
        //print("crouchend called");

        fullCollider.enabled = true;
        crouchCollider.SetActive(false);
        upperBody.GetComponent<MeshRenderer>().enabled = true;
        currentState = PlayerState.Idle;
        isCrouching = false;
        //animation back to normal sized
    }

    void Whip() {
        canWhip = true;
    }

    void FixedUpdate () {

        //if (Input.GetKeyDown(KeyCode.C)) {
        //    if (canMove) {
        //        canMove = false;
        //    } else {
        //        canMove = true;
        //    }
        //}

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (rb.velocity.y < 0) {
            hasJumped = false;
        }

        velocity = rb.velocity; // make a reference to our rigidbody


        // check if player is in the air
        if (!Physics.Raycast(transform.position, Vector3.down, groundCheckDist, groundOnly)) {
            if (isCrouching) {   //stop crouching if we jump
                CrouchEnd();
            }
            currentState = PlayerState.InAir;           //set inAir
        } else if (currentState == PlayerState.InAir) { //on the ground set idle
            currentState = PlayerState.Idle;
        }

        //crouch

        if (currentState != PlayerState.InAir) {            // so we can't crouch in the air
            if (verticalAxis < 0f && !isCrouching) { // check for the button
                isCrouching = true;          // set the PlayerState
                if (!crouchWhip) {                          // change the whip to crouch-state
                    whip.transform.position += new Vector3(0, -0.5f, 0);
                    crouchWhip = true;
                }             
            
                crouchCollider.SetActive(true); // make the collider smaller
                fullCollider.enabled = false;
                upperBody.GetComponent<MeshRenderer>().enabled = false; // make it look like we are crouching
                // animation crouch
            } else if (verticalAxis >= 0f && isCrouching) {
                CrouchEnd();
            }
        }                

        if (currentState != PlayerState.InAir) {
            if (horizontalAxis != 0f) {                     // check if we are moving
                currentState = PlayerState.Moving;
            }

            else if (!isCrouching) {
                currentState = PlayerState.Idle;            // idle if we are not in the air, moving or crouching
            }
        }

        //if (horizontalAxis < 0) currentAimState = AimState.Left;
        //if (horizontalAxis > 0) currentAimState = AimState.Right;

        // moving

        if (whipping && currentState != PlayerState.InAir) {
            canMove = false;
        } else {
            canMove = true;
        }

        if (!canMove) {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        } else if (horizontalAxis != 0) {
            if (isCrouching) {
                rb.velocity = new Vector3(horizontalAxis * (hSpeed / 2), rb.velocity.y, 0);
            } else {
                rb.velocity = new Vector3(horizontalAxis * hSpeed, rb.velocity.y, 0);
            }
            
        } else {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        

        if (currentState == PlayerState.InAir && rb.velocity.y < 0) {
            velocity.y -= extraGravity;
        }


        // jump + dropdown
        if (Input.GetButtonDown("Jump")) {
            if (currentState != PlayerState.InAir) {  // the obvious checks
                CrouchEnd();

                hasJumped = true;
                rb.velocity = new Vector3(rb.velocity.x, vSpeed, 0);
                print("jump called");
            }
        }


        //// aiming
        //if (verticalAxis > 0f) {
        //    if (horizontalAxis > 0f) {
        //        currentAimState = AimState.DiagUpRight;
        //    }
        //    else if (horizontalAxis < 0f) {
        //        currentAimState = AimState.DiagUpLeft;
        //    }
        //    else {
        //        currentAimState = AimState.Up;
        //    }
        //}
        //if (verticalAxis < 0f) {
        //    if (horizontalAxis > 0f) {
        //        currentAimState = AimState.DiagDownRight;
        //    }

        //    else if (horizontalAxis < 0f) {
        //        currentAimState = AimState.DiagDownLeft;
        //    }

        //    else if (horizontalAxis == 0 && currentState == PlayerState.InAir) {
        //        currentAimState = AimState.Down;
        //    }
        //}

        //// shooting
        //if (Input.GetButtonDown("Fire1") && canWhip) {
        //    // shooting forward
        //    if (!isCrouching && currentAimState == AimState.Right) {
        //        // whip right
        //        //whipRight.SetActive(true);
        //        whipRight.GetComponent<Whip>().DoIt();
        //    } else if (!isCrouching && currentAimState == AimState.Left) {
        //        // whip left
        //        whipLeft.SetActive(true);
        //        whipLeft.GetComponent<Whip>().DoIt();
        //    }

        //    // shooting forward while crouched
        //    if ((isCrouching)) {
        //        if (currentAimState == AimState.Right) {
        //            // whip right crouched
        //            whipRight.SetActive(true);
        //            whipRight.GetComponent<Whip>().DoIt();
        //        }

        //        else if (currentAimState == AimState.Left) {
        //            // whip left crouched
        //            whipLeft.SetActive(true);
        //            whipLeft.GetComponent<Whip>().DoIt();
        //        }
        //    }

        //    // shooting upwards
        //    if (currentAimState == AimState.DiagUpRight) {
        //        //diagupright
        //        whipDiagUpRight.SetActive(true);
        //        whipDiagUpRight.GetComponent<Whip>().DoIt();
        //    }
        //    if (currentAimState == AimState.DiagUpLeft) {
        //        //diagupleft
        //        whipDiagUpLeft.SetActive(true);
        //        whipDiagUpLeft.GetComponent<Whip>().DoIt();
        //    }
        //    if (currentAimState == AimState.Up) {
        //        //up
        //        whipUp.SetActive(true);
        //        whipUp.GetComponent<Whip>().DoIt();
        //    }

        //    // shooting downwards
        //    if (currentAimState == AimState.DiagDownRight) {
        //        //diagdownright
        //        whipDiagDownRight.SetActive(true);
        //        whipDiagDownRight.GetComponent<Whip>().DoIt();
        //    }
        //    if (currentAimState == AimState.DiagDownLeft) {
        //        //diagdownleft
        //        whipDiagDownLeft.SetActive(true);
        //        whipDiagDownLeft.GetComponent<Whip>().DoIt();
        //    }
        //    if (currentState == PlayerState.InAir && currentAimState == AimState.Down) {
        //        //down
        //        whipDown.SetActive(true);
        //        whipDown.GetComponent<Whip>().DoIt();
        //    }
        //    canWhip = false;
        //    //Invoke("Whip", 0.5f);
        //}

        //rb.velocity = velocity;     // put our referenced velocity back into the rigidbody
    }

    void Update() {

        

        if (horizontalAxis < 0) currentAimState = AimState.Left;
        if (horizontalAxis > 0) currentAimState = AimState.Right;

        if (Input.GetKeyDown(KeyCode.C)) {
            if (canMove) {
                canMove = false;
            }
            else {
                canMove = true;
            }
        }

        // aiming
        if (verticalAxis > 0f) {
            if (horizontalAxis > 0f) {
                currentAimState = AimState.DiagUpRight;
            }
            else if (horizontalAxis < 0f) {
                currentAimState = AimState.DiagUpLeft;
            }
            else {
                currentAimState = AimState.Up;
            }
        }
        if (verticalAxis < 0f) {
            if (horizontalAxis > 0f) {
                currentAimState = AimState.DiagDownRight;
            }

            else if (horizontalAxis < 0f) {
                currentAimState = AimState.DiagDownLeft;
            }

            else if (horizontalAxis == 0 && currentState == PlayerState.InAir) {
                currentAimState = AimState.Down;
            }
        }

        // shooting
        if (Input.GetButtonDown("Fire1") && canWhip) {
            // shooting forward
            if (!isCrouching && currentAimState == AimState.Right) {
                // whip right
                //whipRight.SetActive(true);
                whipRight.GetComponent<Whip>().DoIt();
            }
            else if (!isCrouching && currentAimState == AimState.Left) {
                // whip left
                whipLeft.SetActive(true);
                whipLeft.GetComponent<Whip>().DoIt();
            }

            // shooting forward while crouched
            if ((isCrouching)) {
                if (currentAimState == AimState.Right) {
                    // whip right crouched
                    whipRight.SetActive(true);
                    whipRight.GetComponent<Whip>().DoIt();
                }

                else if (currentAimState == AimState.Left) {
                    // whip left crouched
                    whipLeft.SetActive(true);
                    whipLeft.GetComponent<Whip>().DoIt();
                }
            }

            // shooting upwards
            if (currentAimState == AimState.DiagUpRight) {
                //diagupright
                whipDiagUpRight.SetActive(true);
                whipDiagUpRight.GetComponent<Whip>().DoIt();
            }
            if (currentAimState == AimState.DiagUpLeft) {
                //diagupleft
                whipDiagUpLeft.SetActive(true);
                whipDiagUpLeft.GetComponent<Whip>().DoIt();
            }
            if (currentAimState == AimState.Up) {
                //up
                whipUp.SetActive(true);
                whipUp.GetComponent<Whip>().DoIt();
            }

            // shooting downwards
            if (currentAimState == AimState.DiagDownRight) {
                //diagdownright
                whipDiagDownRight.SetActive(true);
                whipDiagDownRight.GetComponent<Whip>().DoIt();
            }
            if (currentAimState == AimState.DiagDownLeft) {
                //diagdownleft
                whipDiagDownLeft.SetActive(true);
                whipDiagDownLeft.GetComponent<Whip>().DoIt();
            }
            if (currentState == PlayerState.InAir && currentAimState == AimState.Down) {
                //down
                whipDown.SetActive(true);
                whipDown.GetComponent<Whip>().DoIt();
            }
            canWhip = false;
            //Invoke("Whip", 0.5f);
        }
    }

    //private void FixedUpdate() {
    //    velocity = rb.velocity;
    //    rb.velocity = velocity;     // put our referenced velocity back into the rigidbody
    //}


}   // to do: set the right animations
    // secondary weapon
    // indiana jones
