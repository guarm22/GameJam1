using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{

    private float horizontal;
    private float speed = 4f;
    private float jumpPower = 12f;
    private bool isFacingRight;

    private bool onVine;
    private bool inWater;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask objLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform headCheck;

    private const float upVelocityThreshold = 0.1f;

    public bool isInteracting;
    private float defaultGravity;
    private float defaultSpeed;
    public static CharacterController Instance;

	void Start() {
        Instance = this;
        defaultGravity = rb.gravityScale;
        defaultSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Vine") {
            onVine = true;
        }

        if(collision.gameObject.tag == "Water") {
            if(collision.gameObject.GetComponent<BoxCollider2D>().bounds.Contains(headCheck.position)) {
                inWater = true;
            }
            else {
                inWater = false;
            }
        }

        if(collision.gameObject.tag == "BouncyPad") {
            Bounce();
        }

        if(collision.gameObject.tag == "Checkpoint") {
            CheckpointSystem.Instance.currentCheckpoint = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Vine") {
            onVine = false;
        }

        if(collision.gameObject.tag == "Water") {
            inWater = false;
        }
    }

    private void Bounce() {
        //only want player to bounce if they jump into it
        Debug.Log(rb.velocity.y);
        Debug.Log(rb.velocity.y > upVelocityThreshold);
        if(rb.velocity.y !< .5 && rb.velocity.y !> -.5) {
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, 25f);
    }

    private bool MovementModifiers() {
        if(inWater) {
            speed = 2f;
            rb.gravityScale = 2f;
            return true;
        } 

        if(onVine) {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            if(Input.GetKey(KeyCode.W)) {
                rb.velocity = new Vector2(rb.velocity.x, 3f);
            } else if(Input.GetKey(KeyCode.S)) {
                rb.velocity = new Vector2(rb.velocity.x, -3f);
            } 
            return true;   
        } 
        return false;
    }

    void Update() {
        if(GameSystem.Instance.isPaused) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        if(!MovementModifiers()) {
            speed = defaultSpeed;
            rb.gravityScale = defaultGravity;
        }

        if(Input.GetButtonDown("Jump") && isGrounded() || Input.GetKeyDown(KeyCode.W) && isGrounded()){
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if( (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W)) && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if(Input.GetKey(KeyCode.E)) {
            isInteracting = true;
        } else if(Input.GetKeyUp(KeyCode.E)) {
            isInteracting = false;
        }

        Flip();
    }

    private bool isGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer) || 
                Physics2D.OverlapCircle(groundCheck.position, 0.5f, objLayer);
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void Flip() {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}