using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{

    private float horizontal;
    private float speed = 4f;
    private float jumpPower = 11f;
    private bool isFacingRight;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask objLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rb;

    public bool isInteracting;

    public static CharacterController Instance;

	void Start() {
        Instance = this;
    }

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");

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