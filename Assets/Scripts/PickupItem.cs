using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ID { Berry, Fish };
    public GameManager manager;
    public ID itemID;
    public GameObject effect;

    public AudioClip itemGet;
    public AudioClip itemDeny;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (itemID == ID.Berry)
                if (manager.berries < manager.berriesNeeded)
                {
                    manager.berries++;
                    Instantiate(effect, other.gameObject.transform.position, Quaternion.identity);
                    manager.audioChannel.PlayOneShot(itemGet);
                    Destroy(gameObject);
                }
                else if (manager.misc < manager.miscNeeded)
                {
                    manager.misc++;
                    Instantiate(effect, other.gameObject.transform.position, Quaternion.identity);
                    manager.audioChannel.PlayOneShot(itemGet);
                    Destroy(gameObject);
                } else manager.audioChannel.PlayOneShot(itemDeny);
            if (itemID == ID.Fish)
                if (manager.fish < manager.fishNeeded)
                {
                    manager.fish++;
                    Instantiate(effect, other.gameObject.transform.position, Quaternion.identity);
                    manager.audioChannel.PlayOneShot(itemGet);
                    Destroy(gameObject);
                }
                else if (manager.misc < manager.miscNeeded)
                {
                    manager.misc++;
                    Instantiate(effect, other.gameObject.transform.position, Quaternion.identity);
                    manager.audioChannel.PlayOneShot(itemGet);
                    Destroy(gameObject);
                } else manager.audioChannel.PlayOneShot(itemDeny);
        }
    }
}
