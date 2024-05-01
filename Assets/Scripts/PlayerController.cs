using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{

    private float horizontal;
    private float speed = 5f;
    private float jumpPower = 12f;
    private bool isFacingRight;

    private bool onVine;
    private bool headInWater;
    private bool feetInWater;
    private GameObject water;
    private GameObject vine;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask objLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform headCheck;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite jumpSprite;

    private const float upVelocityThreshold = 0.1f;

    public bool isInteracting;
    private float defaultGravity;
    private float defaultSpeed;
    public static CharacterController Instance;
    public bool carrying = false;

	void Start() {
        Instance = this;
        defaultGravity = rb.gravityScale;
        defaultSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Vine") {
            onVine = true;
            vine = collision.gameObject;
        }

        if(collision.gameObject.tag == "Water") {
            water = collision.gameObject;
            if(collision.gameObject.GetComponent<BoxCollider2D>().bounds.Contains(headCheck.position)) {
                headInWater = true;
            }
            else {
                headInWater = false;
            }
        }

        if(collision.gameObject.tag == "BouncyPad") {
            Bounce(collision.gameObject.GetComponent<BouncyPad>().bounceForce);
        }

        if(collision.gameObject.tag == "Checkpoint") {
            CheckpointSystem.Instance.currentCheckpoint = collision.gameObject;
        }

        if(collision.gameObject.tag == "DeathZone") {
            CheckpointSystem.Instance.Resurrection();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Vine") {
            onVine = false;
            vine = null;
        }

        if(collision.gameObject.tag == "Water") {
            headInWater = false;
        }
    }

    private void Bounce(float force) {
        //only want player to bounce if they jump into it
        if(rb.velocity.y !< .5 && rb.velocity.y !> -.5) {
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, force);
    }

    private bool MovementModifiers() {
        if(headInWater) {
            rb.gravityScale = 6f;
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            if(Input.GetKey(KeyCode.W)) {
                rb.velocity = new Vector2(rb.velocity.x, 3f);
            } else if(Input.GetKey(KeyCode.S)) {
                rb.velocity = new Vector2(rb.velocity.x, -3f);
            } 
            return true;
        } 

        if(!headInWater && feetInWater) {
            if(Input.GetKey(KeyCode.W)) {
                rb.velocity = new Vector2(rb.velocity.x, 5f);
                headInWater = false;
            }
        }

        if(onVine) {
            //print distance from player to vine
            float distance = transform.position.x - vine.transform.position.x;
            bool onLeftSide = distance < 0;

            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            if(Input.GetKey(KeyCode.W)) {
                rb.velocity = new Vector2(rb.velocity.x, 3f);
            } else if(Input.GetKey(KeyCode.S)) {
                rb.velocity = new Vector2(rb.velocity.x, -3f);
            } 

            if(Input.GetKeyDown(KeyCode.Space)) {
                //jump off vine in direction of onLeftSide
                rb.gravityScale = defaultGravity;
                rb.velocity = new Vector2(onLeftSide ? -11.5f : 11.5f, 11.5f);
                onVine = false;
                vine = null;
            }

            return true;   
        } 
        return false;
    }

    void Update() {
        if(GameSystem.Instance.isPaused) return;

        //horizontal = Input.GetAxisRaw("Horizontal");
        horizontal = 0f;
        if (Input.GetKey(KeyCode.A)) {
             horizontal = -1f;
        } else if (Input.GetKey(KeyCode.D)) {
            horizontal = 1f;
        }

        if(!isGrounded() && (rb.velocity.y > upVelocityThreshold)) {
            GetComponent<SpriteRenderer>().sprite = jumpSprite;
        } else {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }

        if(!MovementModifiers()) {
            speed = defaultSpeed;
            rb.gravityScale = defaultGravity;
        }

        if(Input.GetButtonDown("Jump") && isGrounded() || Input.GetKeyDown(KeyCode.W) && isGrounded() && !carrying){
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if( (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W)) && rb.velocity.y > 0f && !carrying) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if(Input.GetKey(KeyCode.E)) {
            isInteracting = true;
        } else if(Input.GetKeyUp(KeyCode.E)) {
            isInteracting = false;
            carrying = false;
        }

        if(water!=null) {
            if(water.GetComponent<BoxCollider2D>().bounds.Contains(groundCheck.position)) {
                feetInWater = true;
            }
            else {
                feetInWater = false;
                water = null;
            }   
        }
        
        Flip();
    }

    public Vector2 GetVelocity() {
        return rb.velocity;
    }

    private bool isGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer) || 
                Physics2D.OverlapCircle(groundCheck.position, 0.5f, objLayer);
    }

    private void FixedUpdate() {
        if (horizontal == 0 && !isGrounded() && !onVine) {
            // If no horizontal input, slow down the player
            rb.velocity = new Vector2(rb.velocity.x * 0.99f, rb.velocity.y);
        }
        else if (horizontal == 0) {
            rb.velocity = new Vector2(rb.velocity.x * 0.3f, rb.velocity.y);
        } else {
            // If there is horizontal input, move the player
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private void Flip() {
        if(horizontal == 0) return;

        if(isFacingRight && horizontal > 0f || !isFacingRight && horizontal < 0f) {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isFacingRight = !isFacingRight;
        }
    }
}