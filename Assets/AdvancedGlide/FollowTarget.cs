using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    AdvanceGlide player;

	// Use this for initialization
	void Start () {
        player = GetComponent<AdvanceGlide>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.position;
	}
}
