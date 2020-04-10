using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static bool leftPressed, rightPressed;
    public static bool upPressed;

    public static void pressLeft() {
        leftPressed = true;
    }
    public static void releaseLeft() {
        leftPressed = false;
    }
}
