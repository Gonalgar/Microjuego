using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 10;
    public Vector3 targetVector;
    public GameObject miniPrefab;
    public float miniForce = 100f;

    private GameObject scoreText;
    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("UI");
    }
    void Update()
    {
        // la bala se mueve en la dirección del jugador al disparar
        transform.Translate(targetVector * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
            // Genera dos mini-asteroides
            SpawnMiniAsteroid(transform.position, 45f);
            SpawnMiniAsteroid(transform.position, -45f);
            IncreaseScore();
        }
        if (collision.gameObject.CompareTag("SmallEnemy"))
        {
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
            IncreaseScore();
        }
    }
    public void IncreaseScore()
    {
        // cuando un asteroide es destruido, llama a esta función para darnos puntos.
        Player.SCORE++;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        // llamamos a esta función cada vez que ganamos puntos para actualizar el marcador
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score\n" + Player.SCORE;
    }
    void SpawnMiniAsteroid(Vector2 position, float angle)
    {
        // Crea un mini-asteroide a partir del prefab
        GameObject miniAsteroid = Instantiate(miniPrefab, position, Quaternion.identity);

        // Aplica una rotación al mini-asteroide
        miniAsteroid.transform.Rotate(Vector3.forward * angle);
        targetVector = new Vector3(miniForce * Mathf.Sign(angle), 0, 0);
        miniAsteroid.GetComponent<Rigidbody2D>().AddForce(targetVector);
    }
}

