using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity;
using UnityEngine.AI;
using System;
using System.Text;
using UnityEngine.UI;


public class AgentControler : Singleton<AgentControler>, IInputClickHandler
{
    float eyeSightangle = 80.0f;
    public GameObject Agent;

    // Consts
    const float RayCastLength = 10.0f;


    NavMeshAgent agent;
    Animator animator;
    // Use this for initialization
    void Start () {

        Agent.SetActive(false);
        StartCoroutine(loop());
        if (Agent.GetComponent<NavMeshAgent>() != null )
        {
            agent = Agent.GetComponent<NavMeshAgent>();
        }
        animator = Agent.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);

    }


    //AirTapされたときに呼び出される関数
    public void OnInputClicked(InputClickedEventData eventData)
    {
        Vector3 hitPos, hitNormal;
                RaycastHit hitInfo;
        Vector3 uiRayCastOrigin = Camera.main.transform.position;
        Vector3 uiRayCastDirection = Camera.main.transform.forward;
        if (Physics.Raycast(uiRayCastOrigin, uiRayCastDirection, out hitInfo, RayCastLength, SpatialMappingManager.Instance.LayerMask))
        {
            if (!Agent.activeSelf)
            {
                Agent.SetActive(true);
                
                hitPos = hitInfo.point;
                hitNormal = hitInfo.normal;
                Agent.transform.position = hitPos;
                Vector3 heading = Agent.transform.position - Camera.main.transform.position;
                heading.y = 0;
                Agent.transform.rotation = Quaternion.LookRotation(heading);
                Agent.transform.rotation = Agent.transform.rotation * Quaternion.Euler(0, 180, 0);
                ConversationManager.Instance.Initialize();
            }
                 
        }

    }

    private IEnumerator loop()
    {
        // ループ
        while (true)
        {
            // 1秒毎にループします
            yield return new WaitForSeconds(1f);

            if (agent.velocity.sqrMagnitude == 0 && FocusAction.isGazed)
            {
                Vector3 heading = Agent.transform.position - Camera.main.transform.position;
                heading.y = 0;
                Agent.transform.rotation = Quaternion.LookRotation(heading);
                Agent.transform.rotation = Agent.transform.rotation * Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public void ComeHere()
    {
        agent.destination = Camera.main.transform.TransformPoint(0f,0f,1f);
    }


}
