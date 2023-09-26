using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Movimiento
    public float thrustForce = 5f;
    public float rotationSpeed = 120f;
    public float xBorderLimit;
    public float yBorderLimit;
    
    //Disparo y Puntuacion
    public GameObject bulletPrefab;
    public GameObject bulletSpawner;
    public static int SCORE = 0;
    
    //Pausa y Reanudacion
    public GameObject pauseMenu;
    public static Boolean isPaused = false;
    public GameObject spawner;

    //Bullet Pooling
    public int poolSize;
    private List<GameObject> pool;
    
    
    //Rigidbody
    private Rigidbody2D _rigidbody;

    void Start()
    {
        // rigidbody nos permite aplicar fuerzas en el jugador
        _rigidbody = GetComponent<Rigidbody2D>();
        
        //Creamos la Pool de balas
        pool = bulletPool(poolSize);
    }
    private void FixedUpdate()
    {
        if (!isPaused)
        {
            var newPos = transform.position;
            if (newPos.x > xBorderLimit)
                newPos.x = -xBorderLimit;
            else if (newPos.x < -xBorderLimit)
                newPos.x = xBorderLimit - 1;
            else if (newPos.y > yBorderLimit)
                newPos.x = -yBorderLimit;
            else if (newPos.y < -yBorderLimit)
                newPos.y = yBorderLimit - 1;
            transform.position = newPos;
    
    
            // obtenemos las pulsaciones de teclado
            float rotation = Input.GetAxis("Rotate") * rotationSpeed * Time.deltaTime;
            float thrust = Input.GetAxis("Thrust") * thrustForce;
            // la dirección de empuje por defecto es .right (el eje X positivo)
            Vector3 thrustDirection = transform.right;
            // rotamos con el eje "Rotate" negativo para que la dirección sea correcta
            transform.Rotate(Vector3.forward, -rotation);
            // añadimos la fuerza capturada arriba a la nave del jugador
            _rigidbody.AddForce(thrust * thrustDirection);
            
            
            if(Input.GetKeyDown(KeyCode.Space))
            {
                GameObject bullet = RequestBullet();
                bullet.transform.position = bulletSpawner.transform.position;
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.targetVector = transform.right;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("SmallEnemy"))
        {
            SCORE = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private List<GameObject> bulletPool(int size)
    {
        List<GameObject> res = new List<GameObject>();
        
        for (int i = 0; i < size; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            res.Add(bullet);
        }

        return res;
    }

    private GameObject RequestBullet()
    {
        foreach (var bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        pool.Add(newBullet);

        return newBullet;
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            spawner.SetActive(false);
        } else {
            isPaused = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            spawner.SetActive(true);
        }
    }
}

