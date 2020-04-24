using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {
    public float weaponWeight = 1f;
    public float damagePerAttack;
    public bool attacking = false;

    private GameObject owner;
    private GameObject edge;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        owner = transform.parent.parent.gameObject;

        edge = transform.Find("Edge").gameObject;
        edge.SetActive(false);
        
        edge.GetComponent<EdgeController>().damage = damagePerAttack;
        edge.GetComponent<EdgeController>().attackedBy = owner.tag;

        animator = GetComponent<Animator>();
    }

    private void Update() {
        if(attacking) {
            edge.SetActive(true);
        } else {
            edge.SetActive(false);
        }
    }

    // Update is called once per frame
    public void attack() {
        if(attacking == false) {
            animator.SetTrigger("Attack");
        }
    }
}
