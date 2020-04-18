using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public float characterSpeed = 10f;
    public float jumpForce = 15f;

    private int platformLayer;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Vector2 playerScale;
    // public float speedScale = 1f;
    private float direction;
    private GameObject weapon;
    // private bool goLeft, goRight, jump;
    

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
        playerScale = transform.localScale;

        weapon = transform.Find("Hand").GetChild(0).gameObject;
        characterSpeed /= weapon.GetComponent<WeaponController>().weaponWeight;
    }


    // Update is called once per frame
    private void Update() {
        if(CrossPlatformInputManager.GetAxis("Horizontal") > 0f || Input.GetAxis("Horizontal") > 0f)
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        else if(CrossPlatformInputManager.GetAxis("Horizontal") < 0f || Input.GetAxis("Horizontal") < 0f)
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
    
        if(CrossPlatformInputManager.GetAxis("Attack") > 0 || Input.GetKey(KeyCode.Space)) {
            StartCoroutine(weapon.GetComponent<WeaponController>().attack());
        }
    }


    void FixedUpdate() {
        if((direction = CrossPlatformInputManager.GetAxis("Horizontal")) != 0f || (direction = Input.GetAxis("Horizontal")) != 0f) {
            rb.velocity = new Vector2(characterSpeed * direction, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if(isGrounded && (CrossPlatformInputManager.GetAxis("Vertical") > 0f || Input.GetAxis("Vertical") > 0f)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        // rb.velocity = new Vector2(playerSpeed * direction * speedScale, rb.velocity.y);
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

    // public void goLeft() {
    //     direction = -1;
    // }
    // public void goRight() {
    //     direction = 1;
    // }
}
