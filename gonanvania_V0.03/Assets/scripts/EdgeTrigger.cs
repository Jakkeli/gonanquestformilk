using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c) {
        if (c.GetComponent<IReaction>() != null && c.tag == "enemy") {
            c.GetComponent<IReaction>().Activate();
        }
    }
}
