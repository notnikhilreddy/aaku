using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon {

    public float attackDelay, attackingTime;
    public GameObject attackObject;

    private GameObject parentObject;
    private string weaponOwner;
    private Vector2 pointDirection, firePoint;
    private Transform parentTransform;

    public Melee() {
        this.attackDelay = 0.5f;
        this.attackingTime = 0.5f;
    }
    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentTransform = parentObject.transform;

        weaponOwner = parentObject.tag;

        IEnumerator coroutine = MeleeWeapon();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    private IEnumerator MeleeWeapon() {
        if(weaponOwner.Equals("Player")) {
            float lastFireTime = 0f;
            while(true) {
                if(Input.GetMouseButtonDown(0)) {
                    
                    if(Time.time - lastFireTime >= attackDelay) {

                        firePoint = transform.GetChild(0).position;
                        GameObject newBullet = Instantiate(attackObject, firePoint, transform.rotation, transform);
                        newBullet.GetComponent<BulletController>().shotBy = parentObject.tag;
                        newBullet.GetComponent<BulletController>().damage = damagePerAttack;

                        lastFireTime = Time.time;

                        float time = Time.time;
                        while(Time.time - time <= 0.5f) {
                            yield return null;
                        }

                        Destroy(newBullet);
                    }
                }

                yield return null;
            }
        } else if(weaponOwner.Equals("Enemy")) {
            Vector2 raycastDirection;
            // RaycastHit2D hitInfo, hit;
            float lastFireTime = Time.time;
            
            while(true) {
                bool playerSpotted = false;

                RaycastHit2D hitInfo, hit;

                raycastDirection = parentTransform.right * parentTransform.localScale.x;
                raycastDirection = raycastDirection.normalized;
                
                hitInfo = Physics2D.Raycast(transform.position, raycastDirection, 0, ~1 << LayerMask.NameToLayer("Enemies"));
                for(float angle = visualAngle/2f; angle >= -visualAngle/2f; angle -= visualAngle/(visualRange*2)) {
                    hit = Physics2D.Raycast(transform.position, Quaternion.Euler(0f, 0f, angle) * raycastDirection, visualRange, ~1 << LayerMask.NameToLayer("Enemies"));
                    
                    if(hit && hit.collider.tag.Equals("Player")) {
                        hitInfo = hit;
                    }
                }
                
                if(hitInfo && hitInfo.collider.tag.Equals("Player")) {
                    playerSpotted = true;

                    Vector3 hitPoint = hitInfo.collider.transform.position;

                    pointDirection = hitPoint - parentObject.transform.position;
                    pointDirection = pointDirection.normalized;

                    Vector2 runVelocity;

                    parentObject.GetComponent<EnemyController>().isAttacking = true;

                    if(pointDirection.x >= 0) {
                        runVelocity = new Vector2(parentObject.GetComponent<EnemyController>().runningSpeed, parentObject.GetComponent<Rigidbody2D>().velocity.y);
                    } else {
                        runVelocity = new Vector2(-parentObject.GetComponent<EnemyController>().runningSpeed, parentObject.GetComponent<Rigidbody2D>().velocity.y);
                    }
                    
                    parentObject.GetComponent<Rigidbody2D>().velocity = runVelocity;
                    
                    if(hitInfo.distance <= 1.5f) {
                        parentObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        if(Time.time - lastFireTime >= attackDelay) {
                            
                            firePoint = transform.GetChild(0).position;
                            GameObject newBullet = Instantiate(attackObject, firePoint, transform.rotation, transform);
                            newBullet.GetComponent<BulletController>().shotBy = parentObject.tag;
                            newBullet.GetComponent<BulletController>().damage = damagePerAttack;

                            lastFireTime = Time.time;

                            float time = Time.time;
                            while(Time.time - time <= attackingTime) {
                                yield return null;
                            }

                            Destroy(newBullet);
                        }
                    }
                }

                if(!playerSpotted) {
                    parentObject.GetComponent<EnemyController>().isAttacking = false;
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                yield return null;
            }
        }
    }
}
