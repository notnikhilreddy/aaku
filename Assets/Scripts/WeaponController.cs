using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public enum Weapon {
        Gun,
        Melee
    }
    public Weapon weaponType = Weapon.Gun;
    public float weaponWeight = 1f;
    public float bulletSpeed = 20f;
    public GameObject bullet;
    public float damagePerAttack = 5f;
    public float attackDelay = 0.5f;
    public float bulletsPerAttack = 1f;
    public float bulletDelay = 0f;
    public float spreadAngle = 10f;
    
    private Transform attackPoint;
    private float lastFireTime = 0f;
    private GameObject owner;

    // Start is called before the first frame update
    void Start() {
        attackPoint = transform.Find("AttackPoint");

        owner = transform.parent.parent.gameObject;
        // if(owner.tag.Equals("Player")) {
        //     owner.GetComponent<PlayerController>().characterSpeed /= weaponWeight;
        // } else if(owner.tag.Equals("Enemy")) {
        //     owner.GetComponent<EnemyController>().characterSpeed /= weaponWeight;
        // }
    }

    // Update is called once per frame
    void Update() {

    }

    private bool attacking = false;
    public IEnumerator attack() {
        if(weaponType == Weapon.Gun) {
            if(!attacking && Time.time - lastFireTime >= attackDelay) {
                attacking = true;
                float lastBulletTime;
                            
                GameObject newBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
                newBullet.GetComponent<BulletController>().shotBy = owner.tag;
                newBullet.GetComponent<BulletController>().damage = damagePerAttack / bulletsPerAttack;
                newBullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * bulletSpeed);

                lastBulletTime = Time.time;
                for(int i = 0; i < bulletsPerAttack-1; i++) {

                    while(Time.time - lastBulletTime < bulletDelay) yield return null;
                    // yield return new WaitForSeconds(bulletDelay);
                    
                    float offsetAngle = Random.Range(spreadAngle/2f, -spreadAngle/2f);

                    newBullet = Instantiate(bullet, attackPoint.position, transform.rotation);
                    newBullet.GetComponent<BulletController>().shotBy = owner.tag;
                    newBullet.GetComponent<BulletController>().damage = damagePerAttack / bulletsPerAttack;
                    newBullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Quaternion.Euler(0, 0, offsetAngle) * Vector2.right * bulletSpeed);

                    lastBulletTime = Time.time;
                }

                lastFireTime = Time.time;
                attacking = false;
            }
        }
    }
}
