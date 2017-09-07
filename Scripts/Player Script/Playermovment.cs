 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Playermovment : MonoBehaviour {

    public float movementspeed = 5f;
    public float jumpPower = 500f;
    public float secondJumpPower = 650f;
    public Transform groundCheckPosition;
    public float radius = 0.2f;
    //Layer in unity , determine which layer can collide with which layer
    public LayerMask layerGround;


    private Rigidbody mybody;
    private bool isGrounded;
    private bool playerJumped;
    private bool canDoubleJump;

    public GameObject smokePosition;

    private bool gameStarted;
    private PlayerAnimation playerAnim;
    private BGScroller bgScroller;
    private PlayerHealthDamage playershoot;

    private Button jumpBtn;

    void Awake()
    {
        mybody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<PlayerAnimation>();
        bgScroller = GameObject.Find(Tags.BACKGROUND_GAME_OBJ).GetComponent<BGScroller>();
        playershoot = GetComponent<PlayerHealthDamage>();
        jumpBtn = GameObject.Find(Tags.JUMP_BTN_OBJ).GetComponent<Button>();
        jumpBtn.onClick.AddListener(() => Jump());
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(StartGame());
	}
	
	// Update is called once per frame
	void Update () {
		

	}
    // Update 3-4 Frames PHYSICS CALCULATIONS
    void FixedUpdate()
    {
        if (gameStarted)
        {
        PlayerMove();
        PlayerGrounded();
        PlayerJump();
        }
       

    }
    void PlayerMove()
    {
         mybody.velocity = new Vector3(movementspeed,mybody.velocity.y,0f);
       
    }

    void PlayerGrounded()
    {
        isGrounded = Physics.OverlapSphere(groundCheckPosition.position, radius, layerGround).Length > 0;
      
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
            mybody.AddForce(new Vector3(0, secondJumpPower, 0));
        }else if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            playerAnim.DidJump();
            mybody.AddForce(new Vector3(0, jumpPower, 0));
            playerJumped = true;
            canDoubleJump = true;

        }
        
    }

    public void Jump()
    {
        if ( !isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
            mybody.AddForce(new Vector3(0, secondJumpPower, 0));
        }
        else if (isGrounded)
        {
            playerAnim.DidJump();
            mybody.AddForce(new Vector3(0, jumpPower, 0));
            playerJumped = true;
            canDoubleJump = true;

        }
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        gameStarted = true;
        smokePosition.SetActive(true);
        GameplayController.instance.canCountScore = true;
        playershoot.canShoot = true;
        bgScroller.canScroll = true;
        playerAnim.PlayerRun();

    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == Tags.PLATRFORM_TAG)
        {
            if (playerJumped)
            {
                playerJumped = false;
                playerAnim.DidLand();

            }
        }
    }
    
    


}//class
