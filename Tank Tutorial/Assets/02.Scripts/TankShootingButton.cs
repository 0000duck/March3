using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShootingButton : MonoBehaviour {

    private bool m_ReadyToFire = true;

    public int m_PlayerNumber = 1;
    //private string m_FireButton;

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

    private void Start()
    {
        //m_FireButton = "Fire" + m_PlayerNumber;

        m_ReadyToFire = true;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    void OnPress()
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

    private void Update()
    {
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


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
        m_ReloadSlider.value = m_ReloadTime;
    }

    private void Fire()
    {
        // Instantiate and launch the shell.

        m_ReadyToFire= true;

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
