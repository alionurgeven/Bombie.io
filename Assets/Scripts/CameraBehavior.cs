using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    [SerializeField]
    private Transform Player;

    private void Update ()
    {
        FollowPlayer();
	}

    private void FollowPlayer()
    {
        if (Player != null)
        {
            transform.position = 
                Vector3.right * Player.position.x + 
                Vector3.up * Player.position.y + 
                Vector3.back * 10;
        }
    }
}
