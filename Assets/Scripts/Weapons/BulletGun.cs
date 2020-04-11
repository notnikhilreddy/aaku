using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BulletGun : Weapon {
    public float bulletSpeed;
    public float attackDelay, bulletsPerAttack;
    public bool simultaneously;
    public float timeBetweenBullets, spreadAngle;
    public GameObject bulletObject;
    public LineRenderer aimingLaser;
    public Joystick aimingJoystick;

    private GameObject parentObject;
    private string weaponOwner;
    private Vector2 pointDirection, firePoint;
    private Transform parentTransform;

    public BulletGun() {
        this.bulletSpeed = 40;
        this.attackDelay = 0.5f;
        this.bulletsPerAttack = 1f;
        this.simultaneously = false;
        this.timeBetweenBullets = 0.03f;
        this.spreadAngle = 5;
    }

    // Start is called before the first frame update
    void Start() {
        parentObject = transform.parent.gameObject;
        parentTransform = parentObject.transform;

        weaponOwner = parentObject.tag;

        IEnumerator coroutine = Gun();
        StartCoroutine(coroutine);
    }

    private void Update() {
        
    }

    // Update is called once per frame
    private IEnumerator Gun() {
        if(weaponOwner.Equals("Player")) {
            float lastFireTime = 0f;
            while(true) {
                // if(!(CrossPlatformInputManager.GetAxis("HorizontalAim") == 0f && CrossPlatformInputManager.GetAxis("VerticalAim") == 0f)) {
                // if(Input.mousePresent) { //CHANGE LATER
                    // pointDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - parentObject.transform.position;
                    pointDirection = new Vector2(CrossPlatformInputManager.GetAxis("HorizontalAim"), CrossPlatformInputManager.GetAxis("VerticalAim"));
                    pointDirection = pointDirection.normalized;

                    RaycastHit2D hitInfo = Physics2D.Raycast(firePoint, pointDirection, visualRange);
                    // aimingLaser.SetPosition(0, firePoint);
                    // aimingLaser.SetPosition(1, firePoint + pointDirection * visualRange);

                    float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;

                    if(pointDirection.x >= 0f) {
                        transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        parentTransform.localScale = new Vector2(Mathf.Abs(parentTransform.localScale.x), parentTransform.localScale.y);
                    } else {
                        transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
                        parentTransform.localScale = new Vector2(-Mathf.Abs(parentTransform.localScale.x), parentTransform.localScale.y);
                    }

                    // if(Input.GetMouseButtonDown(0)) {
                    if(CrossPlatformInputManager.GetAxis("Shoot") > 0f) {
                        firePoint = transform.GetChild(0).position;

                        pointDirection = new Vector2(CrossPlatformInputManager.GetAxis("HorizontalAim"), CrossPlatformInputManager.GetAxis("VerticalAim"));
                        pointDirection = pointDirection.normalized;
                        
                        if(Time.time - lastFireTime >= attackDelay) {
                            
                            GameObject newBullet = Instantiate(bulletObject, firePoint, transform.rotation);
                            newBullet.GetComponent<BulletController>().shotBy = parentObject.tag;
                            newBullet.GetComponent<BulletController>().damage = damagePerAttack / bulletsPerAttack;
                            newBullet.GetComponent<Rigidbody2D>().velocity = parentTransform.right * bulletSpeed;

                            float lastBulletTime = Time.time;
                            for(int i = 0; i < bulletsPerAttack-1; i++) {
                                if(!simultaneously) {
                                    while(Time.time - lastBulletTime < timeBetweenBullets) yield return null;
                                }
                                float shootAngle = Random.Range(spreadAngle/2f, -spreadAngle/2f);
                                pointDirection = Quaternion.Euler(0, 0, shootAngle) * pointDirection;

                                newBullet = Instantiate(bulletObject, firePoint, transform.rotation);
                                newBullet.GetComponent<BulletController>().shotBy = parentObject.tag;
                                newBullet.GetComponent<BulletController>().damage = damagePerAttack / bulletsPerAttack;
                                newBullet.GetComponent<Rigidbody2D>().velocity = pointDirection * bulletSpeed;

                                lastBulletTime = Time.time;
                            }

                            lastFireTime = Time.time;
                        }
                    }
                // }

                yield return null;
            }
        } else if(weaponOwner.Equals("Enemy")) {
            // Debug.Log(weaponOwner);
            parentObject.GetComponent<EnemyController>().walkingSpeed /= weight;

            Vector2 raycastDirection;
            RaycastHit2D hitInfo;
            float lastFireTime = Time.time;
            
            while(true) {
                bool playerSpotted = false;
                for(float angle = visualAngle/2f; angle >= -visualAngle/2f; angle -= visualAngle/(visualRange*2)) {
                    raycastDirection = Quaternion.Euler(0f, 0f, angle) * parentTransform.right * parentTransform.localScale.x;
                    raycastDirection = raycastDirection.normalized;
                    
                    hitInfo = Physics2D.Raycast(transform.position, raycastDirection, visualRange, ~1 << LayerMask.NameToLayer("Enemy"));
                    
                    if(hitInfo && hitInfo.collider.tag.Equals("Player")) {
                        playerSpotted = true;
                        parentObject.GetComponent<EnemyController>().isAttacking = true;

                        Vector3 hitPoint = hitInfo.collider.transform.position;

                        pointDirection = hitPoint - parentObject.transform.position;
                        pointDirection = pointDirection.normalized;
                        
                        float gunAngle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
                        if(pointDirection.x >= 0f) {
                            transform.rotation = Quaternion.Euler(0f, 0f, gunAngle);
                        } else {
                            transform.rotation = Quaternion.Euler(0f, 0f, gunAngle + 180);
                        }

                        if(Time.time - lastFireTime >= attackDelay) {
                            
                            firePoint = transform.GetChild(0).position;
                            GameObject newBullet = Instantiate(bulletObject, firePoint, transform.rotation);
                            newBullet.GetComponent<BulletController>().shotBy = parentObject.tag;
                            newBullet.GetComponent<BulletController>().damage = damagePerAttack / bulletsPerAttack;
                            newBullet.GetComponent<Rigidbody2D>().velocity = pointDirection * bulletSpeed;

                            float lastBulletTime = Time.time;
                            for(int i = 0; i < bulletsPerAttack-1; i++) {
                                if(!simultaneously) {
                                    while(Time.time - lastBulletTime < timeBetweenBullets) yield return null;
                                }
                                float shootAngle = Random.Range(spreadAngle/2f, -spreadAngle/2f);
                                pointDirection = Quaternion.Euler(0, 0, shootAngle) * pointDirection;

                                newBullet = Instantiate(bulletObject, firePoint, transform.rotation);
                                newBullet.GetComponent<BulletController>().shotBy = parentObject.tag;
                                newBullet.GetComponent<BulletController>().damage = damagePerAttack / bulletsPerAttack;
                                newBullet.GetComponent<Rigidbody2D>().velocity = pointDirection * bulletSpeed;

                                lastBulletTime = Time.time;
                            }

                            lastFireTime = Time.time;
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
