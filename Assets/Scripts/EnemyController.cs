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
    private float weaponRange, attackPointDist;
    // private Animator animator;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if(characterSpeed == 0) {
            leftPoint = transform.position.x - 0.01f;
            rightPoint = transform.position.x + 0.01f;
        }

        eye = transform.Find("Eye");
        // StartCoroutine(shootRays());
        vel = characterSpeed;

        weapon = transform.Find("Hand").GetChild(0).gameObject;
        // animator = GetComponent<Animator>();
        attackPointDist = Mathf.Abs(eye.position.x - weapon.transform.Find("AttackPoint").position.x);
        
        if(weapon.CompareTag("BulletGun")) {
            weaponRange = weapon.GetComponent<BulletGun>().weaponRange;
            characterSpeed /= weapon.GetComponent<BulletGun>().weaponWeight;
        } else if(weapon.CompareTag("Melee")) {
            weaponRange = weapon.GetComponent<Melee>().weaponRange;
            // Debug.Log(weaponRange);
            characterSpeed /= weapon.GetComponent<Melee>().weaponWeight;
        }

        transform.Find("AlertSign").gameObject.SetActive(false);
        transform.Find("AttackSign").gameObject.SetActive(false);
        // Debug.Log(attackPointDist);
        // Debug.Log(weaponRange);


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
                    yield return new WaitForSeconds(0.5f);

                    if(!isAttacking && !onAlert) {
                        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                    }
                }
                if(pos >= rightPoint && transform.localScale.x > 0) {
                    vel = 0;
                    yield return new WaitForSeconds(0.5f);
                    
                    if(!isAttacking && !onAlert) {
                        transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                    }
                }
                vel = characterSpeed;
            }

            yield return null;
        }
    }


    private IEnumerator alert() {
        RaycastHit2D backRay;
        while(true) {
            if(!isAttacking) {
                backRay = Physics2D.Raycast(eye.position, eye.TransformDirection(Vector3.left), 3, ~1 << LayerMask.NameToLayer("Enemy"));
                
                if(backRay && backRay.collider.CompareTag("Player")) {
                    onAlert = true;
                    vel = 0;
                    // animator.SetTrigger("Alert");
                    
                    // while(!animationOver) yield return null;
                    transform.Find("AlertSign").gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.5f);

                    vel = characterSpeed / 2f;
                    if(!isAttacking) {
                        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                        yield return new WaitForSeconds(0.5f);
                    }
                    if(!isAttacking) {
                        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                        yield return new WaitForSeconds(0.5f);
                    }
                    if(!isAttacking) {
                        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                        yield return new WaitForSeconds(0.5f);
                    }
                    if(!isAttacking) {
                        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                        yield return new WaitForSeconds(0.5f);
                    }
                    vel = 0;
                    yield return new WaitForSeconds(0.5f);

                    transform.Find("AlertSign").gameObject.SetActive(false);
                    
                    onAlert = false;
                }
            }

            yield return null;
        }
    }


    private IEnumerator attack() {
        float lastAttackingTime = 0f;
        RaycastHit2D frontRay;
        Transform player = null;

        while(true) {
            frontRay = Physics2D.Raycast(eye.position, eye.TransformDirection(Vector3.right), visualRange, ~1 << LayerMask.NameToLayer("Enemy"));
            
            if(frontRay && frontRay.collider.CompareTag("Player")) {
                isAttacking = true;
                transform.Find("AlertSign").gameObject.SetActive(false);
                transform.Find("AttackSign").gameObject.SetActive(true);
                player = frontRay.transform;
                
                lastAttackingTime = Time.time;

            } else if(Time.time - lastAttackingTime >= 0.5f) {
                transform.Find("AttackSign").gameObject.SetActive(false);
                isAttacking = false;
            }

            if(isAttacking) {
                if(player.position.x >= transform.position.x)
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                else
                    transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);

                if(!frontRay) {
                    vel = 0;
                } else if(frontRay.distance < attackPointDist) {
                    vel = characterSpeed * -1.5f;
                } else if(frontRay.distance < attackPointDist + weaponRange) {
                    vel = 0;
                    if(weapon.CompareTag("BulletGun")) {
                        yield return new WaitForSeconds(0.25f);
                        StartCoroutine(weapon.GetComponent<BulletGun>().attack());
                    }
                    else if(weapon.CompareTag("Melee")) {
                        yield return new WaitForSeconds(0.25f);
                        StartCoroutine(weapon.GetComponent<Melee>().attack());
                    }
                } else {
                    vel = characterSpeed * 1.5f;
                }
            }

            yield return null;
        }
    }
}
