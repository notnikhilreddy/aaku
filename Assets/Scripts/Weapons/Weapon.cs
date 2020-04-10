using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float weight;
    public float damagePerAttack;
    public float visualRange, visualAngle;

    public Weapon() {
        this.weight = 1f;
        this.damagePerAttack = 5f;
        this.visualRange = 10f;
        this.visualAngle = 90f;
    }
    
    public Weapon(float weight, float damagePerAttack, float visualRange, float visualAngle) {
        this.weight = weight;
        this.damagePerAttack = damagePerAttack;
        this.visualRange = visualRange;
        this.visualAngle = visualAngle;
    }
}
