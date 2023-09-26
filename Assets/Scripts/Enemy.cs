using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public Vector3 targetVector;
    
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rb.AddForce(new Vector2(speedX, speedY));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
