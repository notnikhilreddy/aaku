using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool isGrounded;
    public float playerSpeed, jumpForce;
    public LayerMask wallsLayer;
    private Vector2 playerScale;
    private float direction;
    // Start is called before the first frame update

    public PlayerController() {
        this.playerSpeed = 10f;
        this.jumpForce = 15f;
        // this.wallsLayer = LayerMask.NameToLayer("Platform");
    }
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        // isGrounded = true;
        playerScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate() {
        isGrounded = rb.IsTouchingLayers(wallsLayer);
        if((direction = Input.GetAxis("Horizontal")) != 0) {

            if(!Input.mousePresent) // CHANGE LATER
                if(direction > 0)
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                else
                    transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);

            rb.velocity = new Vector2(playerSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
        }
        if(Input.GetAxis("Vertical") > 0 && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
