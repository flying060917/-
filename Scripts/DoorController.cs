using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle=90f;
    public float closeAngle=0f;
    public float smoothSpeed=5f;
    public bool isOpen=false;
    Quaternion openRotation;
    Quaternion closeRotation;
    private SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        openRotation=Quaternion.Euler(0f,openAngle,0f);
        closeRotation=Quaternion.Euler(0f,closeAngle,0f);
        sphereCollider=gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger=true;
        gameObject.tag="Door";
        if(sphereCollider.radius<1f)
        {
            sphereCollider.radius=1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen&&Quaternion.Angle(transform.rotation,openRotation)>0.01f)
        {
            transform.rotation=Quaternion.Slerp(transform.rotation,openRotation,smoothSpeed*Time.deltaTime);
        }
        else if(!isOpen&&Quaternion.Angle(transform.rotation,closeRotation)>0.01f)
        {
            transform.rotation=Quaternion.Slerp(transform.rotation,closeRotation,smoothSpeed*Time.deltaTime);
        }
    }
    public void ToggleDoor()
    {
        isOpen=!isOpen;
    }
}
