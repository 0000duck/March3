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
    float stateTime = 0.0f;
    public float idleStateMaxTime = 2.0f;

    public Transform target;

    public float speed = 1.0f;
    public float rotationSpeed = 90.0f;
    public float detectableRange = 40.0f;
    public float attackableRange = 20.0f;
    public float attackStateMaxTime = 2.0f;

    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void FixedUpdate () {
        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:

                break;
            case ENEMYSTATE.CHASE:
                //Debug.Log("chase");
                Vector3 dir = target.position - m_Rigidbody.position;
                Vector3 movement = 
                    new Vector3(dir.x * m_Speed * Time.deltaTime, 0f, dir.z * m_Speed * Time.deltaTime);
                m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < attackableRange)
                {
                    enemyState = ENEMYSTATE.ATTACK;
                }
                break;
            case ENEMYSTATE.ATTACK:
                Debug.Log("attack");

                break;
            case ENEMYSTATE.DAMAGE:

                break;
            case ENEMYSTATE.DEAD:

                break;
            default:
                break;
        }
    }
}
