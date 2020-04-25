using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    // Start is called before the first frame update

    private bool isAttacking = false, onAlert = false;
    public float leftPoint, rightPoint, characterSpeed = 3f;
    public float visualRange = 5f;
    private float vel;
    private Rigidbody2D rb;
    private Transform eye;
    private GameObject weapon;
    private float weaponRange;
    // private Animator animator;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if(characterSpeed == 0) {
            leftPoint = transform.position.x - 0.01f;
            rightPoint = transform.position.x + 0.01f;
        }

        eye = transform.Find("Eye");
        // StartCoroutine(shootRays());

        weapon = transform.Find("Hand").GetChild(0).gameObject;
        if(weapon.CompareTag("BulletGun")) {
            weaponRange = weapon.GetComponent<BulletGun>().bulletRange;
            characterSpeed /= weapon.GetComponent<BulletGun>().weaponWeight;
        } else if(weapon.CompareTag("Melee")) {
            weaponRange = 1f;
            characterSpeed /= weapon.GetComponent<Melee>().weaponWeight;
        }
        vel = characterSpeed;
        // animator = GetComponent<Animator>();

        StartCoroutine(chill());
        StartCoroutine(alert());
        StartCoroutine(attack());
    }

    void Update() {
        rb.velocity = eye.TransformDirection(Vector2.right) * new Vector2(vel, rb.velocity.y);
    }


    private IEnumerator chill() {
        while(true) {
            if(!isAttacking && !onAlert) {
                float pos = transform.position.x;

                if(pos <= leftPoint && transform.localScale.x < 0) {
                    vel = 0;
                    yield return new WaitForSeconds(1f);

                    if(!isAttacking && !onAlert) {
                        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                        vel = characterSpeed;
                    }
                }
                if(pos >= rightPoint && transform.localScale.x > 0) {
                    vel = 0;
                    yield return new WaitForSeconds(1f);
                    
                    if(!isAttacking && !onAlert) {
                        transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                        vel = characterSpeed;
                    }
                }
            }

            yield return null;
        }
    }


    public bool animationOver;
    private IEnumerator alert() {
        RaycastHit2D backRay;
        while(true) {
            backRay = Physics2D.Raycast(eye.position, eye.TransformDirection(Vector3.left), 3, ~1 << LayerMask.NameToLayer("Enemy"));
            
            if(backRay && backRay.collider.CompareTag("Player")) {
                onAlert = true;
                vel = 0;
                // animator.SetTrigger("Alert");
                
                // while(!animationOver) yield return null;
                transform.Find("AlertSign").gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);

                if(!isAttacking)
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(0.5f * Time.deltaTime);
                if(!isAttacking)
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(0.5f * Time.deltaTime);
                if(!isAttacking)
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(0.5f * Time.deltaTime);
                if(!isAttacking)
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(0.5f * Time.deltaTime);

                transform.Find("AlertSign").gameObject.SetActive(false);
                
                vel = characterSpeed;
                onAlert = false;
            }

            yield return null;
        }
    }


    private IEnumerator attack() {
        float lastAttackingTime = 0f;
        RaycastHit2D frontRay;
        while(true) {
            frontRay = Physics2D.Raycast(eye.position, eye.TransformDirection(Vector3.right), visualRange, ~1 << LayerMask.NameToLayer("Enemy"));
            
            if(frontRay && frontRay.collider.CompareTag("Player")) {
                isAttacking = true;
                
                if(frontRay.distance <= weaponRange) {
                    vel = 0;
                    if(weapon.CompareTag("BulletGun"))
                        StartCoroutine(weapon.GetComponent<BulletGun>().attack());
                    else if(weapon.CompareTag("Melee"))
                        weapon.GetComponent<Melee>().attack();
                } else {
                    vel = characterSpeed * 1.5f;
                }
                lastAttackingTime = Time.time;

            } else if(Time.time - lastAttackingTime >= 0.5f) {
                isAttacking = false;
            }

            yield return null;
        }
    }
    private RaycastHit2D backRay;
    // private IEnumerator shootRays() {
    //     while(true) {
    //         frontRay = Physics2D.Raycast(eye.position, eye.TransformDirection(Vector3.right), visualRange, ~1 << LayerMask.NameToLayer("Enemy"));
    //         backRay = Physics2D.Raycast(eye.position, eye.TransformDirection(Vector3.left), 2, ~1 << LayerMask.NameToLayer("Enemy"));

    //         if(frontRay && frontRay.collider.CompareTag("Player"))
    //             isAttacking = true;
    //         else
    //             isAttacking = false;
            
    //         if(backRay && backRay.collider.CompareTag("Player"))
    //             onAlert = true;
    //         else
    //             onAlert = false;
            
    //         yield return null;
    //     }
    // }

    // private IEnumerator waitAndGoLeft(float time, float speed) {
    //     vel = 0;
    //     yield return new WaitForSeconds(time);
    //     transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
    //     vel = speed;
    // }
    // private IEnumerator waitAndGoRight(float time, float speed) {
    //     vel = 0;
    //     yield return new WaitForSeconds(time);
    //     transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    //     vel = speed;
    // }

    // private IEnumerator waitAndTurn() {
    //     while(true) {
    //         float pos = transform.position.x;
            
    //         turnRight = false; turnLeft = false;
    //         if(!isAttacking && pos <= leftPoint && transform.localScale.x < 0) {
    //             vel = 0;
    //             yield return new WaitForSeconds(1f);
    //             turnRight = true;
    //         } else if(!isAttacking && pos >= rightPoint && transform.localScale.x > 0) {
    //             vel = 0;
    //             yield return new WaitForSeconds(1f);
    //             turnLeft = true;
    //         } else
    //             vel = characterSpeed;

    //         yield return null;
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log(other.tag);
    //     if(other.CompareTag("Player")) {
    //         Debug.Log("player enter");
    //         onAlert = true;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if(other.CompareTag("Player")) {
    //         onAlert = false;
    //     }
    // }
}

