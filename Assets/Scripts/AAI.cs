using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AAI : MonoBehaviour
{

    NavMeshAgent agent;
    public Transform goal;
    public PlayerController pc;
    public Transform pt;
    public GameManager GM;
    public float detectionRadius;
    public float origSpeed;

    public Transform[] waypoints;
    int currentWaypointIndex;
    int randomWaypointIndex;

    bool chasingPlayer;

    public AudioSource channel;
    public AudioClip loseItems;
    public GameObject effect;

    public float chasePitchMod;

    public float catchTimer = 0.5f;
    public float catchDelay = 0.5f;
    public bool frozen;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pc = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<PlayerController>();
        pt = GameObject.FindGameObjectWithTag("Player").transform;
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        currentWaypointIndex = 1;
        origSpeed = 5f;
        chasingPlayer = false;
        agent.destination = waypoints[1].position;
        chasePitchMod = 1.3f;
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        catchTimer += Time.deltaTime;
        if (catchTimer <= catchDelay)
            agent.isStopped = true;
        else
        {
            agent.isStopped = false;
            anim.SetBool("Catch", false);
        }
        print(waypoints.Length);
        // if spotted
        Vector3 p = pt.position;
        // p.y = 0;
        Vector3 e = agent.transform.position;
        // e.y = 0;
        if (Vector3.Distance(e, p) <= detectionRadius && (GM.berries > 0 || GM.fish > 0 || GM.misc > 0))
        {
            print("chasing player now");
            agent.destination = pt.position;
            chasingPlayer = true;
            anim.SetBool("Running", true);
            GM.ambientMusic.pitch = chasePitchMod;
        }
        // if got away
        else
        {
            agent.destination = waypoints[currentWaypointIndex].position;
            chasingPlayer = false;
            anim.SetBool("Running", false);
            GM.ambientMusic.pitch = 1;
            print(currentWaypointIndex);
        }

        // if at dest
        if (!chasingPlayer && CheckIfReachedDestination())
        {
            FindNextWaypoint();
            print("next wp");
        }
    }

    void FindNextWaypoint()
    {
        // get random waypoint
        do
        {
            randomWaypointIndex = Random.Range(0, waypoints.Length);
        }
        while (randomWaypointIndex == currentWaypointIndex);
        currentWaypointIndex = randomWaypointIndex;
        print(currentWaypointIndex);

        agent.destination = waypoints[currentWaypointIndex].position;
    }

    bool CheckIfReachedDestination()
    {
        Vector3 e = transform.position;
        Vector3 p;
        if (chasingPlayer)
        {
            p = pt.position;
        }
        else p = waypoints[currentWaypointIndex].transform.position;
        print(Mathf.Abs(p.x - e.x) + " - " + Mathf.Abs(p.z - e.z));
        return Mathf.Abs(p.x - e.x) < 1 && Mathf.Abs(p.z - e.z) < 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GM.berries > 0 || GM.fish > 0 || GM.misc > 0)
            {
                Instantiate(effect, collision.gameObject.transform.position, Quaternion.identity);
                channel.PlayOneShot(loseItems);
            }
            GM.berries = 0;
            GM.fish = 0;
            GM.misc = 0;
            catchTimer = 0;
            anim.SetBool("Catch", true);
            chasingPlayer = false;
            agent.destination = waypoints[currentWaypointIndex].position;
            agent.speed = origSpeed;
            GM.ambientMusic.pitch = 1;
        }
    }
}
