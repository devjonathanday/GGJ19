using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]//Only allow one copy of this script per GameObject
//Required components:
[RequireComponent(typeof(AIAttributes))]//AIAttributes
[RequireComponent(typeof(Rigidbody))]//RigidBody
[RequireComponent(typeof(BoxCollider))]//BoxCollider
public class Enemy : MonoBehaviour
{
    public List<GameObject> Waypoints;
    int CurrentPoint = 0;

     
    AIAttributes Att;
    public float Speed;

    AIController.ENVIRONMENT HomeEnvironment;//Environment the Ai stays in
    AIController.AI_TYPE CurrentType;//Type of AI that current GameObject is
    AIController.STATE CurrentState;//State Ai is currently in

    Rigidbody rb;
    BoxCollider ColliderBox;
    BoxCollider TriggerCollider;

    Vector3 Direction;

    GameObject Player;

    bool HOLD;

    void Start ()
    {
        Att = GetComponent<AIAttributes>();

        Player = GameObject.FindGameObjectWithTag("Player");

        HomeEnvironment = Att.HomeEnvironment;
        CurrentType = Att.AIType;
        CurrentState = Att.CurrentState;

        rb = GetComponent<Rigidbody>();
        ColliderBox = GetComponent<BoxCollider>();

        Direction = Vector3.forward;

        TriggerCollider = gameObject.AddComponent<BoxCollider>();
        TriggerCollider.center = new Vector3(0.5f, 1.5f, 0f);
        TriggerCollider.size = new Vector3(6, 6, 6);
        TriggerCollider.isTrigger = true;

        HOLD = false;
	}
	
	void Update ()
    {
        if(!HOLD)
            Movement();
	}

    void Movement()
    {
        Direction = Waypoints[CurrentPoint].transform.position - transform.position;

        Quaternion Rotation = Quaternion.LookRotation(Direction);//Sets a angle in terms of a quanternion
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime * 5); //Lerps using Quanternions

        transform.position += (Direction).normalized * Time.deltaTime * Speed;
    }

    bool CheckForPlayer()
    {
        
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject == Waypoints[CurrentPoint])
        {
            CurrentPoint++;
            if (CurrentPoint == 4)
                CurrentPoint = 0;
        }
        if(other.transform.gameObject == Player)
        {
            HOLD = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject == Player)
        {
            transform.LookAt(Player.transform.position);
            transform.position += (Player.transform.position - transform.position).normalized * Time.deltaTime * Speed;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject == Player)
            HOLD = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject == Player)
        {
            print("*Big Dino tucks little dino into bed for the night*");
        }
    }

}
