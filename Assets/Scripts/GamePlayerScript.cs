using UnityEngine;
using System.Collections;

public class GamePlayerScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            FindObjectOfType<GameManagerScript>().NotifyTouched();
        }
    }
}
