using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Disabler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("SmallEnemy"))
        {
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
