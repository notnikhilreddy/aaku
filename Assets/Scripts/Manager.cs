using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    public bool spawnPlayer = true;
    public Transform spawnPoint;
    public GameObject playerPrefab;
    public Sprite[] playerSkins;

    public bool addPlayerHealthCanvas = true;
    public Canvas playerHealthCanvas;
    public Slider healthSlider;

    public bool addPlayerControlCanvas = true;
    public Canvas playerControlCanvas;

    private int skinID = 2;


    // Start is called before the first frame update
    void Start() {
        if(spawnPlayer) {
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            player.GetComponent<PlayerHealth>().healthSlider = healthSlider;
            player.GetComponent<SpriteRenderer>().sprite = playerSkins[skinID];
            PhysicsMaterial2D material = player.GetComponent<PolygonCollider2D>().sharedMaterial;
            Destroy(player.GetComponent<PolygonCollider2D>());
            PolygonCollider2D collider = player.AddComponent<PolygonCollider2D>();
            collider.sharedMaterial = material;
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
