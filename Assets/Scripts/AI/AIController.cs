using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]//Only allow one copy of this script per GameObject
//Required components:
[RequireComponent(typeof(AIAttributes))]//AIAttributes
[RequireComponent(typeof(Rigidbody))]//RigidBody
[RequireComponent(typeof(BoxCollider))]//BoxCollider
public class AIController : MonoBehaviour
{
    public enum ENVIRONMENT { Water, Dirt };
    public enum STATE { Defensive, Offensive, Passive };
    public enum AI_TYPE { Fish, Enemy1 };

    public GameObject Player;

    ENVIRONMENT HomeEnvironment;//Environment the Ai stays in
    AI_TYPE CurrentType;//Type of AI that current GameObject is
    STATE CurrentState;//State Ai is currently in

    int Health;//Health of the current AI
    int Speed;//Speed of how fast the AI can move

    bool IsIdle;//Wheather the AI can walk or stay still
    float DecisionMaxTimer;//The reset value of the timer
    float DecisionTimer;//How often the AI makes decisions

    Vector3 Target;//The current target the AI is following
    public bool PlayerDetected;//Wheather the player has been detected or not
    Vector3 LastSafePlace;//Last location of safetyness

    public float FishDelay;//Time for the fish to turn around
    public float EnemyDelay;//Time for the Enemy to figure what the F*ck he is doing

    AIAttributes Attributes;//AIAttribute Script

    //Movement Variables
    Vector3 Direction;//The direction that the AI moves in


	void Start ()
    {
        Attributes = gameObject.GetComponent<AIAttributes>();
        //Assign all variables with the corresponding AIAttributes
        {
            HomeEnvironment = Attributes.HomeEnvironment;
            CurrentType = Attributes.AIType;
            CurrentState = Attributes.CurrentState;
            Health = Attributes.MaxHealth;
            Speed = Attributes.Speed;
        }

        IsIdle = false;
        DecisionMaxTimer = 10;
        DecisionTimer = DecisionMaxTimer;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerDetected = false;

        FishDelay = 3;
        EnemyDelay = 0;

        //Movement Assignment
        {
            Direction = new Vector3(1, 0, 0);
            Target = Vector3.zero;
        }
	}

    void Update()
    {
        DecisionTimer -= Time.deltaTime;//Lowering the timer
        if(FishDelay <= 3)//If the timer is less than or equal to its max, count down
            FishDelay -= Time.deltaTime;
        //if (EnemyDelay <= 3)//If the timer is less than or equal to its max, count down
        //    EnemyDelay -= Time.deltaTime;
        if(EnemyDelay <= 0 && PlayerDetected)//If the timer is less than or equal to its max, count down
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;//Makes sure the trigger is set to false
            Speed /= 2;//Cuts the speed in half
            PlayerDetected = false;//Player detect is dissabled
        }
            GetComponent<Rigidbody>().velocity = Vector3.zero;//sets the velocity to zero(player stops moving)

        if (!IsIdle && !PlayerDetected)//If the AI can move
            Movement();//Move the player

        if (!PlayerDetected)//Check if the AI is in the Offensive state
            if (DecisionTimer <= 0)//Make Decision
            {
                DecisionTimer = DecisionMaxTimer;//Reset the timer back to the max timer number
                //IsIdle = Random.Range(0, 100) < 10 ? true : false;//10% chance to Idle
                Direction = Random.Range(0, 100) < 50 ? new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)) : Direction;//can change directions
            }


        if (CurrentType == AI_TYPE.Enemy1 && !CheckForPlayerInSight() && PlayerDetected == true)//Check if the enemy an enemy and See if player is not in sight
        {
            EnemyDelay = 10;//sets the delay of the enemy
            gameObject.GetComponent<BoxCollider>().isTrigger = true;//sets the box collider to be true
            Speed *= 2;//doubles the speed
            
        }
        if (PlayerDetected)//If the player is found
            ChasePlayer();//Chase the hell out of them
    }

    void Movement()
    {
        if (Target == Vector3.zero)//Default Value for the target
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//Makes sure the AI does not rotate funny
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * Time.deltaTime * Speed);//Moves the player towards the Direction

            if(CurrentType == AI_TYPE.Fish && FishDelay <= 0)//If the Ai is a fish and timer is out
            {
                Direction = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));//Sets the direction to a random Vector3
                Direction -= transform.forward;//flip the direction
                FishDelay = 3;//Reset timer
            }

            if (CurrentType != AI_TYPE.Fish && TurnAroundDecision() && EnemyDelay > 0)//Wheather the AI needs to turn around 
                Direction = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));//Sets the direction to a random Vector3

            Quaternion Rotation = Quaternion.LookRotation(Direction);//Sets a angle in terms of a quanternion
            transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime * 5); //Lerps using Quanternions

        }
        else
        {
            Target = LastSafePlace;//Sets the target to the last known safe locations
            transform.LookAt(Target);//Look at the safe place
            transform.position += transform.forward * Time.deltaTime * Speed;//Get yourself home Buddy.
            if (Mathf.Abs(transform.position.x - Target.x) < 1f && Mathf.Abs(transform.position.y - Target.y) < 1f && Mathf.Abs(transform.position.z - Target.z) < 1f)
                Target = Vector3.zero;//Sets the target back to zero's
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//Change the AI to Never fall over
        }
    }
    bool TurnAroundDecision()
    {
        RaycastHit Hit;
        Vector3 DrawSpot;//Where it will draw the Raycast
        if (CurrentType == AI_TYPE.Fish)//If it is a fish
        {
            FishDelay = 3;//Set fish delay
            DrawSpot = gameObject.transform.position + gameObject.transform.forward * 2 + new Vector3(0, 1, 0);//set the drawspot for the Raycast
        }
        else
        {
        //    //if (Mathf.Abs(Player.transform.position.x - transform.position.x) > 20 || Mathf.Abs(Player.transform.position.y - transform.position.y) > 20 ||
        //    //    Mathf.Abs(Player.transform.position.z - transform.position.z) > 20)//If the AI is too far from the player
        //    //{
        //        Direction = Player.transform.position;//ComeBack!
        //        return false;
        //    }
        DrawSpot = gameObject.transform.position + gameObject.transform.forward + new Vector3(0, 1, 0);
        }

        // Debug.DrawRay(DrawSpot, -gameObject.transform.up, Color.red);
        if (Physics.Raycast(DrawSpot, -gameObject.transform.up, out Hit, 2))
            //If the object hit by Ray is not in the same Environment
            if (Hit.transform.gameObject.GetComponent<Environment>() &&
                Hit.transform.gameObject.GetComponent<Environment>().CurrentEnvironment != HomeEnvironment)//If AI walks into a wall that it dowsn't belong
                return true;//About to hit different Environment


        return false;
    }

   

    bool CheckForPlayerInSight()
    {
        RaycastHit Hit;
        Vector3[] DrawSpot = new Vector3[9];//Create a line of sight
        for (int i = 0; i < 7; i++)//Iterate through the array
        {
            DrawSpot[i] = transform.position + transform.right * (3 - i);//Set the positions of the Rays 1 unit apart
            Debug.DrawRay(DrawSpot[i], gameObject.transform.forward * 8, Color.red);//Debug draw them
            if (Physics.Raycast(DrawSpot[i], gameObject.transform.forward, out Hit, 8))//If an object hits the hit
                if (Hit.transform.gameObject.tag == "Player")//Check if it is the player
                {
                    Target = Hit.transform.position;//Make the Targer the player
                    PlayerDetected = true;//The player has now been detected

                    if (Physics.Raycast(transform.position, -gameObject.transform.up, out Hit, 1))//Creating the Raycast
                        if (Hit.transform.gameObject.GetComponent<Environment>().CurrentEnvironment == HomeEnvironment)//If floor matches currentenvironment
                            LastSafePlace = Hit.transform.position;//Saves the last saved position

                    return true;
                }
        }
            return false;
    }
    void ChasePlayer()
    {
        transform.LookAt(Target);//Look at the player when chasing the player
        transform.position += transform.forward * Time.deltaTime * Speed / 2;//Chase them!!!!!!
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.gameObject.GetComponent<Environment>() &&
            collision.transform.gameObject.GetComponent<Environment>().CurrentEnvironment != HomeEnvironment)//Check to see if a fish collider with a wall
            Direction = -transform.forward;//Turn the fish around
    }
}
