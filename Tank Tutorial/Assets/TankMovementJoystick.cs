using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovementJoystick : MonoBehaviour {

    public GameObject m_Joystick;
    public float m_Speed;

    private Rigidbody m_Rigidbody;
    private float m_xValue;
    private float m_zValue;

    private float m_DegreeOfStick = 270f+45f;
    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        //m_xValue = FixedJoystickY();
        //m_zValue = FixedJoystickX();

        m_xValue = m_Joystick.transform.localPosition.y;
        m_zValue = m_Joystick.transform.localPosition.x;
    }

    //public float FixedJoystickY(float joystickYpos, float joystickXpos, float degree)
    //{
    //    return 
    //}

    //public float FixedJoystickX(float joystickYpos, float joystickXpos, float degree)
    //{
    //    return
    //}

    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = new Vector3(m_xValue * m_Speed * Time.deltaTime, 0f, -m_zValue * m_Speed * Time.deltaTime);

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }
}
