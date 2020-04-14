using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public bool spawnPlayer = true;
    public GameObject playerObject;
    public Transform spawnPoint;
    public bool addPlayerCanvas = true;
    public Canvas playerCanvas;


    // Start is called before the first frame update
    void Start() {
        if(spawnPlayer) {
            Instantiate(playerObject, spawnPoint.position, Quaternion.identity);
        }
        if(addPlayerCanvas) {
            Instantiate(playerCanvas, Vector3.zero, Quaternion.identity);
        }
    }


    // Update is called once per frame
    void Update() {
        
    }
}
