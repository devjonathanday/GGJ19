using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointEffect : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Color effectColor;
    public float fadeSpeed;
    public float jumpSpeed;
    public float rotateSpeed;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.material.color = effectColor;
        GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpSpeed, 0));
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed, 0));
        meshRenderer.material.color = new Color(
        meshRenderer.material.color.r,
        meshRenderer.material.color.g,
        meshRenderer.material.color.b,
        meshRenderer.material.color.a - fadeSpeed);
        if(meshRenderer.material.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
