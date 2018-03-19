using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;  


    private Quaternion m_RelativeRotation;     


    private void Start()
    {
        //게임 시작시에 부모의 로테이션 값을 가져옵니다.
        m_RelativeRotation = transform.parent.localRotation;
        
    }


    private void Update()
    {
        if (m_UseRelativeRotation)
            //게임 시작시에 가져온 로테이션 값을 계속 유지합니다.
            transform.rotation = m_RelativeRotation;
    }
}
