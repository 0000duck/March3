using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickLimiter : MonoBehaviour {
    private float yPos;
    private float xPos;
    public float yLimit = 150f;
    public float xLimit = 150f;

	void Start () {
        
	}
	
	void Update () {
        yPos = transform.position.y;
        xPos = transform.position.x;

        if (yPos >= yLimit)
        {            
            transform.position = new Vector3(transform.position.x, yLimit, 0f);
        }

        if (xPos >= xLimit)
        {
            transform.position = new Vector3(xLimit, transform.position.y, 0f);
        }

        if (yPos <= -yLimit)
        {
            transform.position = new Vector3(transform.position.x, yLimit, 0f);
        }

        if (xPos <= -xLimit)
        {
            transform.position = new Vector3(xLimit, transform.position.y, 0f);
        }
    }
}
