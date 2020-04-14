using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public float playerSpeed = 10f;
    public float jumpForce = 15f;

    private int platformLayer;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Vector2 playerScale;
    private float velocity;
    private bool leftPressed, rightPressed, upPressed;
    

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
        playerScale = transform.localScale;
    }


    // Update is called once per frame
    private void Update() {
        if(CrossPlatformInputManager.GetAxis("Horizontal") > 0f || Input.GetAxis("Horizontal") > 0f)
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        else if(CrossPlatformInputManager.GetAxis("Horizontal") < 0f || Input.GetAxis("Horizontal") < 0f)
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }


    void FixedUpdate() {
        if((velocity = CrossPlatformInputManager.GetAxis("Horizontal")) != 0f || (velocity = Input.GetAxis("Horizontal")) != 0f) {
            rb.velocity = new Vector2(playerSpeed * velocity, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if(isGrounded && (CrossPlatformInputManager.GetAxis("Vertical") > 0f || Input.GetAxis("Vertical") > 0f)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == platformLayer) {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == platformLayer) {
            isGrounded = false;
        }
    }
}
