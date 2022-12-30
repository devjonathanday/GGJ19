using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoParticles : MonoBehaviour
{
    GameObject Player;//The Player
    ParticleSystem JumpParticle;//The particle made by landing on land
    ParticleSystem WaterParticles;//The partical made by landing in water
    bool HasJumped;//Determining if he has jumped or not

    bool WaterWasLast;//Checks if the water was the last partical to play

    void Start ()
    {
        //initilize the values
        HasJumped = false;
        WaterWasLast = false;
        JumpParticle = GameObject.Find("JumpParticles").GetComponent<ParticleSystem>();
        WaterParticles = GameObject.Find("JumpParticlesWater").GetComponent<ParticleSystem>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

	void Update ()
    {
        if (HasJumped)//If the player has jumped
            if (OnGround())//And they are now on the ground
            {
                HasJumped = false;//Resets the HasJumped
                RaycastHit Hit;

                if (Physics.Raycast(Player.transform.position, -gameObject.transform.up, out Hit, 0.5f))
                    if (Hit.transform.gameObject.GetComponent<Environment>())//Checks if the object that has been hit is a floor
                        if (Hit.transform.gameObject.GetComponent<Environment>().CurrentEnvironment == AIController.ENVIRONMENT.Water)//And it is water
                        {
                            WaterParticles.GetComponent<ParticleSystem>().Play();//Play the water partical animation
                            WaterWasLast = true;//Water was the last partical to accur
                        }
                        else if (Hit.transform.gameObject.GetComponent<Environment>().CurrentEnvironment == AIController.ENVIRONMENT.Dirt)//If not, then dirt?
                            if (!WaterWasLast)//As long as water wasn't last
                                JumpParticle.GetComponent<ParticleSystem>().Play();//Play Landing partical animation
                            else
                                WaterWasLast = false;//if it was the last one, then reset the flag

            }
        if (!OnGround())//Is the player not on the ground?
            HasJumped = true;//if so, he has jumped
    }


    bool OnGround()//On Ground?
    {
        RaycastHit hit;
        Debug.DrawRay(Player.transform.position, -gameObject.transform.up * 0.5f);
        if (Physics.Raycast(Player.transform.position, -gameObject.transform.up, out hit, 0.5f))
            if (hit.collider.CompareTag("Ground"))//If object hit is the ground
                return true;//then yes I was on the ground
        return false;
    }


}
