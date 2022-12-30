using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 targetDifference; //Vector3 difference of player position and desired camera position
    public Vector3 targetRotation; //Desired camera rotation
    public float moveLerp;
    public bool lerping;
    public float closeEnough;
    public float lerpTimer;
    public float delay;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        
        if (lerping)
        {
            lerpTimer += Time.deltaTime;
            if (lerpTimer >= delay)
            {
                lerping = false;
                transform.position = player.position + targetDifference;
            }
            transform.position = Vector3.Lerp(transform.position, player.position + targetDifference, moveLerp);
            transform.LookAt(player);
            if ((transform.position.x - (player.position.x + targetDifference.x)) < closeEnough &&
               (transform.position.y - (player.position.y + targetDifference.y)) < closeEnough &&
               (transform.position.z - (player.position.z + targetDifference.z)) < closeEnough)
                lerping = false;
        }
        else transform.position = player.position + targetDifference;
    }
}