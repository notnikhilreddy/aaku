using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BulletGun : MonoBehaviour {
    public float weaponWeight = 1f;
    public float bulletSpeed = 20f;
    public GameObject bullet;
    public float attackDelay = 0.5f;
    public float bulletsPerAttack = 1f;
    public float bulletDelay = 0f;
    public float damagePerBullet = 1f;
    public float spreadAngle = 10f;
    public float horizontalSpread = 0f;
    public float bulletRange = 100f;
    
    private Transform attackPoint;
    private float lastFireTime = 0f;
    private GameObject owner;

    // Start is called before the first frame update
    void Start() {
        attackPoint = transform.Find("AttackPoint");

        owner = transform.parent.parent.gameObject;
    }

    private void Update() {
        
    }


    private bool attacking = false;
    public IEnumerator attack() {
        if(!attacking && Time.time - lastFireTime >= attackDelay) {
            attacking = true;
            float lastBulletTime;
                        
            GameObject newBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            newBullet.GetComponent<BulletController>().shotBy = owner.tag;
            newBullet.GetComponent<BulletController>().damage = damagePerBullet;
            newBullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * bulletSpeed);
            newBullet.GetComponent<BulletController>().life = bulletRange / bulletSpeed;

            lastBulletTime = Time.time;
            for(int i = 0; i < bulletsPerAttack-1; i++) {

                while(Time.time - lastBulletTime < bulletDelay) yield return null;
                // yield return new WaitForSeconds(bulletDelay);
                
                float offsetAngle = Random.Range(-spreadAngle/2f, spreadAngle/2f);
                float offsetV = Random.Range(-horizontalSpread/2f, horizontalSpread/2f);

                newBullet = Instantiate(bullet, attackPoint.position, transform.rotation);
                newBullet.GetComponent<BulletController>().shotBy = owner.tag;
                newBullet.GetComponent<BulletController>().damage = damagePerBullet;
                newBullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Quaternion.Euler(0, 0, offsetAngle) * Vector2.right * (bulletSpeed+offsetV));
                newBullet.GetComponent<BulletController>().life = bulletRange / (bulletSpeed+offsetV);

                lastBulletTime = Time.time;
            }

            lastFireTime = Time.time;
            attacking = false;
        }
    }
}

/*
    private IEnumerator Gun() {
        if(weaponOwner.Equals("Enemy")) {
            // Debug.Log(weaponOwner);
            parentObject.GetComponent<EnemyController>().characterSpeed /= weight;

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
*/