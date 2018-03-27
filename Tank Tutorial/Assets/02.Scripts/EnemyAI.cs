using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public enum ENEMYSTATE
    {
        IDLE = 0, CHASE, ATTACK, DAMAGE, DEAD
    }

    public ENEMYSTATE enemyState = ENEMYSTATE.IDLE;

    public Rigidbody m_Rigidbody;
    public float m_Speed;
    public float rotationSpeed;
    public float detectableRange;
    public float attackableRange;
    //public float attackStateMaxTime = 2.0f;
    float stateTime = 0.0f;
    public float idleStateMaxTime = 2.0f;

    public Transform target;    
    private float distance;

    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    private float m_OriginalPitch;

    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        EngineAudio();
    }

    void FixedUpdate () {
        distance = Vector3.Distance(transform.position, target.position);
        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                if(distance < detectableRange)
                {
                    enemyState = ENEMYSTATE.CHASE;
                }
                break;
            case ENEMYSTATE.CHASE:
                //Debug.Log("chase");
                Move();
                Turn();

                if (distance < attackableRange)
                {
                    enemyState = ENEMYSTATE.ATTACK;
                }
                break;
            case ENEMYSTATE.ATTACK:
                //Debug.Log("attack");


                Turn();
                break;
            case ENEMYSTATE.DAMAGE:
                break;
            case ENEMYSTATE.DEAD:
                break;
            default:
                break;
        }
    }

    private void Move()
    {
        Vector3 dir = target.position - m_Rigidbody.position;
        Vector3 movement =
            new Vector3(dir.x * m_Speed * Time.deltaTime, 0f, dir.z * m_Speed * Time.deltaTime);
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        transform.LookAt(target);
    }

    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (enemyState == ENEMYSTATE.CHASE)
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
