using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControler : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed=5f;
    public float mouseSensitivity=5f;
    private float currentVerticalRotation=0f;
    private float currentHorizontalRotation=0f;
    public GameObject flashLight;
    private bool isLightOn=false;
    private float grivity=9.8f;
    private DoorController dc;
    private bool isCaught=false;
    public Transform beCaughtPos;
    private WEAPONTYPE weaponType;
    public GameObject[] weapons;
    private Animator animator;
    private Dictionary<WEAPONTYPE,int> bag=new Dictionary<WEAPONTYPE,int>();//背包里的子弹数
    private Dictionary<WEAPONTYPE,int> clip=new Dictionary<WEAPONTYPE,int>();//弹夹里的子弹数
    public int clipMaxFlashLightBattery=1;
    public int clipMaxSingleShotBullet=30;
    public int clipMaxAutoShotBullet=50;
    private bool isReloading=false;
    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<CharacterController>();
        Cursor.lockState=CursorLockMode.Locked;
        weaponType=WEAPONTYPE.FLASHLIGHT;
        animator=GetComponentInChildren<Animator>();
        bag.Add(WEAPONTYPE.FLASHLIGHT,1);
        bag.Add(WEAPONTYPE.SINGLESHOT,100);
        bag.Add(WEAPONTYPE.AUTOSHOT,100);
        clip.Add(WEAPONTYPE.FLASHLIGHT,clipMaxFlashLightBattery);
        clip.Add(WEAPONTYPE.SINGLESHOT,clipMaxSingleShotBullet);
        clip.Add(WEAPONTYPE.AUTOSHOT,clipMaxAutoShotBullet);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCaught)
        {
            return;
        }
        PlayerMove();
        PlayerRotation();
        PlayerLight();
        ToggleDoor();
        ChangeWeapon();
    }
    private void PlayerMove()
    {
       float horizontal=Input.GetAxis("Horizontal");
       float vertical=Input.GetAxis("Vertical");
       Vector3 moveDirection=transform.right*horizontal+transform.forward*vertical;
       controller.Move(moveDirection*moveSpeed*Time.deltaTime);
       controller.Move(Vector3.down*grivity*Time.deltaTime);
       if(moveDirection.magnitude>0)
       {
        animator.SetBool("Move",true);
       }
       else{
        animator.SetBool("Move",false);
       }
    }
    private void PlayerRotation()
    {
        float mouseX=Input.GetAxis("Mouse X");
        float mouseY=Input.GetAxis("Mouse Y");
        currentHorizontalRotation+= mouseX*mouseSensitivity*Time.deltaTime;
        currentVerticalRotation+=   mouseY*mouseSensitivity*Time.deltaTime;
        this.transform.eulerAngles=new Vector3(-currentVerticalRotation,currentHorizontalRotation,0);
    }
    private void PlayerLight()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isLightOn=!isLightOn;
            flashLight.SetActive(isLightOn);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Door")
        {
            dc=other.gameObject.GetComponent<DoorController>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Door")
        {
            dc=null;
        }
    }
    private void ToggleDoor()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&dc!=null)
        {
            Debug.Log("ToggleDoor");
            dc.ToggleDoor();
        }
    }
    public Vector3 BeCaughtPos()
    {
        isCaught=true;
        return beCaughtPos.position;
    }
    private void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {        
            weaponType=(WEAPONTYPE)((int)(weaponType+1)%3);
            ChangeWeaponObject();
            animator.SetTrigger("ChangeWeapon");
            switch(weaponType)
            {
                case WEAPONTYPE.FLASHLIGHT:
                Debug.Log("WeaponType:FLASHLIGHT");
                animator.SetBool("UseGun",false);
                break;
                case WEAPONTYPE.SINGLESHOT:
                Debug.Log("WeaponType:SINGLESHOT");
                animator.SetBool("UseGun",true);
                break;
                case WEAPONTYPE.AUTOSHOT:
                Debug.Log("WeaponType:AUTOSHOT");
                animator.SetBool("UseGun",true);
                break;
            }
        }
    }
    private void ChangeWeaponObject()
    {
        for(int i=0;i<weapons.Length;i++)
        {
            weapons[i].SetActive(false);
        }
        Debug.Log("Enabling weapon: " + (int)weaponType);
        weapons[(int)weaponType].SetActive(true);
    }
    private void Reload()
    {
        if(!Input.GetKeyDown(KeyCode.R))
        {
            return;
        }
        bool canReload=false;
        switch(weaponType)
        {
            case WEAPONTYPE.FLASHLIGHT:
            if(clip[WEAPONTYPE.FLASHLIGHT]<clipMaxFlashLightBattery)
            {
                canReload=true;
            }
            break;
            case WEAPONTYPE.SINGLESHOT:
            if(clip[WEAPONTYPE.SINGLESHOT]<clipMaxSingleShotBullet)
            {
                canReload=true;
            }
            break;
            case WEAPONTYPE.AUTOSHOT:
            if(clip[WEAPONTYPE.AUTOSHOT]<clipMaxAutoShotBullet)
            {
                canReload=true;
            }
            break;
        }
        if(canReload)
        {
            if(bag[weaponType]>0)
            {
                isReloading=true;
                Invoke("RecoverAttackState",3f);
                animator.SetTrigger("Reload");
                switch(weaponType)
                {
                    case WEAPONTYPE.FLASHLIGHT:
                    if(bag[WEAPONTYPE.FLASHLIGHT]>clipMaxFlashLightBattery)
                    {

                    }
                    break;
                    case WEAPONTYPE.SINGLESHOT:
                    if(bag[WEAPONTYPE.SINGLESHOT]>clipMaxSingleShotBullet)//背包里的子弹数大于弹夹的最大容量
                    {
                        //如果弹夹里面有剩余则补满
                        if()
                        {

                        }
                        else//弹夹里面没有剩余则直接把弹夹装满
                        {
                          
                        }   
                    }
                    else//背包里的子弹数不够装满弹夹
                    {
                      clip[WEAPONTYPE.SINGLESHOT]+=bag[WEAPONTYPE.SINGLESHOT];
                      bag[WEAPONTYPE.SINGLESHOT]=0;
                    }
                    break;
                    case WEAPONTYPE.AUTOSHOT:
                    break;
                }
            }
        }
    }
    private void RecoverAttackState()
    {
        isReloading=false;
    }
}
public enum WEAPONTYPE
{
  FLASHLIGHT,
  SINGLESHOT,
  AUTOSHOT

}
