using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    private bool m_ReadyToFire = true;
    //public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;
    public float m_ReloadTime = 2f;
    public Slider m_ReloadSlider;
    public GameObject m_ReloadIcon;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private float reloadTime;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
        m_ReloadSlider.value = m_ReloadTime;
    }

    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        m_OriginalPitch = m_MovementAudio.pitch;

        m_ReadyToFire = true;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        EngineAudio();

        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;
        m_ReloadSlider.value = reloadTime;
        reloadTime -= Time.deltaTime;

        if (reloadTime < 0)
            m_ReloadIcon.SetActive(false);

        if (!m_ReadyToFire)
        {
            //Debug.Log("Charging!");

            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_ReadyToFire)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
            Reload();
        }
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

                if (distance > detectableRange)
                {
                    enemyState = ENEMYSTATE.IDLE;
                }
                break;
            case ENEMYSTATE.ATTACK:
                //Debug.Log("attack");
                Turn();

                Fireee();

                if (distance > attackableRange)
                {
                    enemyState = ENEMYSTATE.CHASE;
                }
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

    void Fireee()
    {
        if (reloadTime < 0)
        {
            m_ReloadIcon.SetActive(false);
            switch (m_ReadyToFire)
            {
                case true:
                    m_ReadyToFire = false;
                    //Debug.Log("Press!");

                    m_CurrentLaunchForce = m_MinLaunchForce;

                    m_ShootingAudio.clip = m_ChargingClip;
                    m_ShootingAudio.Play();

                    break;
                case false:
                    m_ReadyToFire = true;
                    //Debug.Log("Fire!");

                    Fire();
                    Reload();

                    break;
            }
        }
    }

    private void Fire()
    {
        // Instantiate and launch the shell.

        m_ReadyToFire = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    private void Reload()
    {
        reloadTime = m_ReloadTime;
        m_ReloadIcon.SetActive(true);
    }
}
