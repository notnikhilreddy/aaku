using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    public bool spawnPlayer = true;
    public GameObject player;
    public int skinID = 2;
    public Sprite[] playerSkins;

    public bool addWeapon = true;
    public int weaponID = 0;
    public GameObject[] Weapons;

    public bool addPlayerHealthCanvas = true;
    public Canvas playerHealthCanvas;
    public Slider healthSlider;

    public bool addPlayerControlCanvas = true;
    public Canvas playerControlCanvas;


    // Start is called before the first frame update
    void Start() {
        player.SetActive(false);
        if(spawnPlayer) {
            // player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            player.GetComponent<PlayerHealth>().healthSlider = healthSlider;
            player.GetComponent<SpriteRenderer>().sprite = playerSkins[skinID];
            // PhysicsMaterial2D material = player.GetComponent<PolygonCollider2D>().sharedMaterial;
            Destroy(player.GetComponent<PolygonCollider2D>());
            PolygonCollider2D collider = player.AddComponent<PolygonCollider2D>();
            // collider.sharedMaterial = material;
            player.SetActive(true);
        }

        if(addWeapon) {
            Transform playerHand = player.transform.Find("Hand");
            GameObject weapon = Instantiate(Weapons[weaponID], playerHand.position, Quaternion.identity, playerHand);
            player.GetComponent<PlayerController>().weapon = weapon;
            weapon.layer = LayerMask.NameToLayer("PlayerWeapon");
            weapon.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerWeapon";
        }

        playerHealthCanvas.gameObject.SetActive(false);
        if(addPlayerHealthCanvas) {
            playerHealthCanvas.gameObject.SetActive(true);
        }

        playerControlCanvas.gameObject.SetActive(false);
        if(addPlayerControlCanvas) {
            playerControlCanvas.gameObject.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update() {
        
    }
}
