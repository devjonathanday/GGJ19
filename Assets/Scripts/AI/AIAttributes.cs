using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttributes : MonoBehaviour
{
    public AIController.ENVIRONMENT HomeEnvironment;
    public AIController.AI_TYPE AIType;
    public AIController.STATE CurrentState;

    public int MaxHealth;
    public int Speed;

    public float DetectionRadius;
}
