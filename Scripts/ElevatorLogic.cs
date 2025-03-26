using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLogic : MonoBehaviour
{
    bool isTrigger=false;
    public Transform elevator;
    public float openSpeed=5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isTrigger==true&&Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            isTrigger=true;
            Debug.Log("碰到了门");
        }
    }
    private void OpenDoor()
    {
        elevator.position=this.transform.GetChild(0).position;
        Vector3 targetPos=new Vector3(80f,transform.position.y,transform.position.z);
        Vector3.Lerp(this.transform.position,targetPos,openSpeed*Time.deltaTime);
        
        
    }

}
