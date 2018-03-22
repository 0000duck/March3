using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovementJoystick : MonoBehaviour {

    public GameObject m_Joystick;
    public float m_Speed;

    public int m_PlayerNumber = 1;
    //public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private Rigidbody m_Rigidbody;
    private float m_xValue;
    private float m_zValue;

    //private string m_MovementAxisName;
    //private string m_TurnAxisName;
    //private float m_MovementInputValue;
    //private float m_TurnInputValue;
    private float m_OriginalPitch;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        //m_MovementInputValue = 0f;
        //m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_xValue = m_Joystick.transform.localPosition.x;
        m_zValue = m_Joystick.transform.localPosition.y;

        EngineAudio();
    }

    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = new Vector3(m_xValue * m_Speed * Time.deltaTime, 0f, m_zValue * m_Speed * Time.deltaTime);

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        if (m_xValue == 0f && m_zValue == 0f)
        {
            transform.rotation = transform.rotation;
        }
        else
        {
            transform.LookAt(new Vector3(m_xValue, 0f, m_zValue));
        }
    }

    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (Mathf.Abs(m_xValue + m_zValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }
}
