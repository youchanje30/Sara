using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{

    [SerializeField] Transform playerTrans;
    [SerializeField] float dontFollowDistance;


    [Range(0f, 1f)]
    [SerializeField] float range;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if(!playerTrans)
            playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        SetPos();
    }


    void SetPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 pos = playerTrans.position;

        float x = mousePos.x - pos.x;
        float y = mousePos.y - pos.y;

        Vector3 followPos = new Vector3(x, y, 0f) * range;
        float distance = followPos.magnitude;
        // float distance = (new Vector3(x, y, 0f) * range).magnitude;

        if(distance <= dontFollowDistance)
        {
            transform.position = pos;
            return;
        }

        transform.position = followPos + pos;
    }
}
