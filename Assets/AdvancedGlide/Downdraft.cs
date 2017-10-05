using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downdraft : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        print("IN THE WIND");
        AdvanceGlide player = other.GetComponent<AdvanceGlide>();
        if (player != null)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - Time.deltaTime * 3);
        }
    }
}
