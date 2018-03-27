using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickLimiter : MonoBehaviour {
    private float yPos;
    private float xPos;
    public float limit = 150f;

	void Start () {
        
	}
	
	void Update () {
        yPos = transform.localPosition.y;
        xPos = transform.localPosition.x;

        if (yPos >= limit)
        {            
            transform.localPosition = new Vector3(transform.localPosition.x, limit, 0f);
        }

        if (xPos >= limit)
        {
            transform.localPosition = new Vector3(limit, transform.localPosition.y, 0f);
        }

        if (yPos <= -limit)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -limit, 0f);
        }

        if (xPos <= -limit)
        {
            transform.localPosition = new Vector3(-limit, transform.localPosition.y, 0f);
        }
    }
}
