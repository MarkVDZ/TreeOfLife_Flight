using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print(">>>>>>");
        AdvanceGlide player = other.GetComponent<AdvanceGlide>();
        if(player != null)
        {
            if(player.forwardAirMovement > 0)
            {
                player.forwardAirMovement -= 5;
            }
            else
            {
                player.forwardAirMovement += 5;
            }
            
        }
    }
}
