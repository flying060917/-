using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FemaleGhost : MonoBehaviour
{
    private FirstPersonControler fpc;
    private bool isSeeMe=false;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fpc!=null)
        {
            Vector3 targetPos=new Vector3(fpc.transform.position.x,this.transform.position.y,
            fpc.transform.position.z);
            this.transform.LookAt(targetPos);
            if(isSeeMe)
            {
                if(Vector3.Angle(this.transform.forward,fpc.transform.forward)<90f)
                {
                    isSeeMe=false;
                }
            }
            else
            {
                if(Vector3.Angle(this.transform.forward,fpc.transform.forward)>90f)
                {
                    isSeeMe=true;
                    this.transform.position+=(targetPos-this.transform.position)/2;
                    audioSource.PlayOneShot(audioClip);
                    if(Vector3.Distance(this.transform.position,targetPos)<1f)
                    {
                        Debug.Log("Caught");
                        CaughtPlayer();
                        Invoke("Replay",5f);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(fpc==null&&other.tag=="Player")
        {
            fpc=other.GetComponent<FirstPersonControler>();
            isSeeMe=true;
        }
    }
    private void CaughtPlayer()
    {
        animator.CrossFade("Eat",1);
        this.transform.position=fpc.BeCaughtPos();
    }
    private void Replay()
    {
        SceneManager.LoadScene(0);
    }
}
