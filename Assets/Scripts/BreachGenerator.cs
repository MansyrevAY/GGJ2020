using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachGenerator : MonoBehaviour
{
    public GameObject breachParent;
    public GameObject breachPrefab;
    public Collider2D borders;
    public int maxNumberOfBreachesPerSolarFlare;
    public int maxNumberOnShip;
    public LayerMask otherBreaches;

    public LayerMask shipMask;
    public LayerMask obstacle;

    public static int CurrentNumberOnShip { get; set; }

    public float timeLeft;
    public float timeToNextFlare;

    public float minTimeToFlare;
    public float maxTimeToFlare;

    private bool gameOver;

    void Start()
    {
        CurrentNumberOnShip = 0;
        gameOver = false;
        timeToNextFlare = Random.Range(5f, 10f);
    }

    void Update()
    {
        if(!gameOver)
        {
            timeLeft -= Time.deltaTime;
            timeToNextFlare -= Time.deltaTime;
            if (timeLeft < 0 && !gameOver)
            {
                Debug.Log("YOU WON");
                gameOver = true;
            }

            if (timeToNextFlare < 0)
            {
                GenerateBreaches();
                timeToNextFlare = Random.Range(minTimeToFlare, maxTimeToFlare);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateBreaches();
            }
        }
    }

    private void GenerateBreaches()
    {
        if (CurrentNumberOnShip < maxNumberOnShip)
        {
            // TODO: LIMIT WITH MAX NUMBER OF BREACHES OTHERWISE IT MIGHT CRASH
            int numberOfBreaches = Random.Range(1, maxNumberOfBreachesPerSolarFlare + 1);
            CurrentNumberOnShip += numberOfBreaches;
            for (int i = 0; i < numberOfBreaches; i++)
            {
                float randomX = Random.Range(borders.bounds.center.x - borders.bounds.extents.x,
                    borders.bounds.center.x + borders.bounds.extents.x);
                float randomY = Random.Range(borders.bounds.center.y - borders.bounds.extents.y,
                    borders.bounds.center.y + borders.bounds.extents.y);
                Vector2 position = new Vector2(randomX, randomY);

                // TODO: Update with OverlapBox
                while (Physics2D.OverlapCircle(position, 0.5f, otherBreaches) ||
                       !Physics2D.OverlapCircle(position, 0.000000000001f, shipMask) ||
                       Physics2D.OverlapCircle(position, 0.5f, obstacle))
                {
                    Debug.Log($"I didn't like {position}");
                    randomX = Random.Range(borders.bounds.center.x - borders.bounds.extents.x,
                        borders.bounds.center.x + borders.bounds.extents.x);
                    randomY = Random.Range(borders.bounds.center.y - borders.bounds.extents.y,
                        borders.bounds.center.y + borders.bounds.extents.y);
                    position = new Vector2(randomX, randomY);
                }

                Debug.Log($"Finally Here: {position}");
                Vector3 euler = transform.eulerAngles;
                euler.z = Random.Range(0f, 360f);
                transform.eulerAngles = euler;
                Instantiate(breachPrefab, position, Quaternion.Euler(0, 0, Random.Range(0, 360)),
                    breachParent.transform);
            }

            print($"current number: {CurrentNumberOnShip}");
        }
        else
        {
            print("GAME OVER!");
            gameOver = true;
        }
    }
}