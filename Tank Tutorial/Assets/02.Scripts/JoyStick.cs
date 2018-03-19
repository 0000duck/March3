using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour {

    public Vector3 initPos;
    public float stickPower;
    public float vValue;
    public float hValue;

	void Start () {
        initPos = gameObject.transform.position;
	}
	

	void Update () {
        stickPower = Vector3.Distance(gameObject.transform.position, initPos);
        vValue = gameObject.transform.position.y - initPos.y;
        hValue = gameObject.transform.position.x - initPos.x;
	}
}
