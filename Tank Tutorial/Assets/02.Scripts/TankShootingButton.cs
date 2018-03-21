using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShootingButton : MonoBehaviour {

    public bool m_Fired = true;

    private void Start()
    {
        m_Fired = true;
    }

    void OnPress()
    {
        switch (m_Fired)
        {
            case true:
                m_Fired = false;
                Debug.Log("Press!");
                break;
            case false:
                m_Fired = true;
                Debug.Log("Fire!");
                break;            
        }  
    }

    private void Update()
    {
        if (!m_Fired)
        {
            Debug.Log("Charging!");
        }
    }
}
