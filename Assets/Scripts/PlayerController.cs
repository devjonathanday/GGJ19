using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Vector3 maxVel; //Max RigidBody velocity for X and Z speed, Y is regulated by gravity
    public float jumpForce = 100f;
    public float lerpVal;
    public GameObject player; //Player object

    private Rigidbody rb; //Rigidbody attached to ^ player
    private Collider col; // player collider
    private Vector3 origin;
    private Vector3 lookPoint;

    public GroundTrigger groundTrigger;

    private float animTimer;
    public float animTransition;

    public GameManager GM;

    public AudioClip jumpSound;
    public AudioClip itemLose;

    Animator Anim;

    // Use this for initialization
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        col = GetComponentInChildren<Collider>();
        origin = new Vector3(1, 0, 0);
        Anim = player.GetComponent<Animator>();
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        animTimer += Time.deltaTime;
        // move parent
        Vector3 deltaMovement = Vector3.zero;
        deltaMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        deltaMovement.Normalize();
        deltaMovement *= moveSpeed * Time.deltaTime;
        //Clamp the velocity between +/- max velocity values
        //rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxVel.x, maxVel.x), rb.velocity.y,
        //                          Mathf.Clamp(rb.velocity.z, -maxVel.z, maxVel.z));
        rb.AddForce(deltaMovement);
        if (Input.GetKeyDown(KeyCode.Space)) Jump();

        // rotate child
        if (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.z) > 0.1f)
        {
            // player.transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * (Mathf.Atan2(-rb.velocity.z, rb.velocity.x)) + 90, 0);
            // desired rot
            Quaternion t = Quaternion.Euler(0, Mathf.Rad2Deg * (Mathf.Atan2(-rb.velocity.z, rb.velocity.x)) + 90, 0);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, t, lerpVal * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            Anim.SetBool("Walking", true);
        else Anim.SetBool("Walking", false);

        if (groundTrigger.grounded)
            Anim.SetBool("Jump", false);
        else Anim.SetBool("Jump", true);
    }

    void Jump()
    {
        if (groundTrigger.grounded)
        {
            //Anim.SetBool("Jump", true);
            GM.audioChannel.PlayOneShot(jumpSound);
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }
    }

    //bool OnGround()
    //{
    //    RaycastHit hit;
    //    Ray r = new Ray(player.transform.position, -Vector3.up);
    //    if (Physics.Raycast(r, out hit, col.bounds.extents.y + 0.1f))
    //    {
    //        if (hit.collider.CompareTag("Ground"))
    //        {
    //            Anim.SetBool("Jump", false);
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}