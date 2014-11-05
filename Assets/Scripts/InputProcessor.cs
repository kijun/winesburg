using UnityEngine;
using System.Collections;

public class InputProcessor : MonoBehaviour {

    public CharacterController mainController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        mainController.changeDirection(GetDirectionFromInput());
	}

    Direction GetDirectionFromInput () {
        if (Input.GetKey ("up")) {
            return Direction.Up;
        } else if (Input.GetKey ("down")) {
            return Direction.Down;
        } else if (Input.GetKey ("left")) {
            return Direction.Left;
        } else if (Input.GetKey ("right")) {
            return Direction.Right;
        } else {
            return Direction.None;
        }
    }
       
}
