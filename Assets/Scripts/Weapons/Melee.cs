using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {
    [HideInInspector] public bool attacking = false;
    [HideInInspector] public float weaponRange = 0.2f;
    public float weaponWeight = 1f;
    public float damagePerAttack;

    private GameObject owner;
    private GameObject edge;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        weaponRange = 0.2f;

        owner = transform.parent.parent.gameObject;

        edge = transform.Find("AttackPoint").gameObject;
        edge.SetActive(false);
        
        edge.GetComponent<EdgeController>().damage = damagePerAttack;
        edge.GetComponent<EdgeController>().attackedBy = owner.tag;

        animator = GetComponent<Animator>();
    }

    private void Update() {
        if(!attacking)
            edge.SetActive(false);
    }

    public void attack() {
        if(!attacking) {
            attacking = true;
            edge.SetActive(true);
            animator.SetTrigger("Attack");
        }
    }
}
