using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public KeyCode left, right, front, back;
    public float speed;
    public float strafeSpeed;
    public float jumpForce;
    public GameObject EndPanel;
    // public GameObject EndPanel;

    public Rigidbody body;
    public bool isGrounded;
    PhotonView PV;
    public Camera cam;
    GameObject canvas;

    public AudioSource audioSource;
    public AudioClip jumpClip;

    // static AudioSource audioSrc;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        cam = transform.root.GetComponentInChildren<Camera>();
        canvas = GameObject.Find("EndCanvas"); 
        audioSource = GetComponent<AudioSource>();
        jumpClip = Resources.Load("BackGroundMusic/jumpSound") as AudioClip;

    }

    void Start()
    {
        
        if (!PV.IsMine)
        {
            Destroy(cam);
            Destroy(body);
        }
    }
    
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;//내꺼아니면 작동안함

        if (Input.GetKey(front))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isRun", true);

                body.AddForce(body.transform.forward * speed * 2f);
            }
            else
            {
                animator.SetBool("isRun", false);
                animator.SetBool("isWalk", true);
                body.AddForce(body.transform.forward * speed);
            }
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }

        if (Input.GetKey(left))
        {
            animator.SetBool("isSideLeft", true);
            body.AddForce(-body.transform.right * strafeSpeed);
        }
        else
        {
            animator.SetBool("isSideLeft", false);
        }

        if (Input.GetKey(back))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isRun", true);

                body.AddForce(-body.transform.forward * speed * 2f);
            }
            else
            {
                animator.SetBool("isRun", false);
                animator.SetBool("isWalk", true);

                body.AddForce(-body.transform.forward * speed);
            }
        }
        else if (!Input.GetKey(front))
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }

        if (Input.GetKey(right))
        {
            animator.SetBool("isSideRight", true);
            body.AddForce(body.transform.right * strafeSpeed);
        }
        else if (!Input.GetKey(left))
        {
            animator.SetBool("isSideRight", false);
            animator.SetBool("isSideLeft", false);
        }

        if (Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                // Debug.Log(jumpSound);
                // audioSource.clip = jumpClip;
                body.AddForce(new Vector3(0, jumpForce, 0));
                isGrounded = false;
                audioSource.PlayOneShot(jumpClip);
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            canvas.transform.Find("EndPanel").gameObject.SetActive(true);
        }
    }
}