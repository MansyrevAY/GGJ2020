using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachGenerator : MonoBehaviour
{
    public GameObject breachParent;
    public GameObject breachPrefab;
    public Collider2D borders;
    public int maxNumberOfBreachesPerSolarFlare;
    public LayerMask otherBreaches;

    public LayerMask shipMask;
    public LayerMask obstacle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateBreaches();
        }
    }

    private void GenerateBreaches()
    {
        // TODO: LIMIT WITH MAX NUMBER OF BREACHES OTHERWISE IT MIGHT CRASH
//        int numberOfBreaches = Random.Range(1, maxNumberOfBreachesPerSolarFlare + 1);
        int numberOfBreaches = 1;
        for (int i = 0; i < numberOfBreaches; i++)
        {
            float randomX = Random.Range(borders.bounds.center.x - borders.bounds.extents.x,
                borders.bounds.center.x + borders.bounds.extents.x);
            float randomY = Random.Range(borders.bounds.center.y - borders.bounds.extents.y,
                borders.bounds.center.y + borders.bounds.extents.y);
            Vector2 position = new Vector2(randomX, randomY);

            // TODO: Update with OverlapBox
            while (Physics2D.OverlapCircle(position, 0.5f, otherBreaches) ||
                   !Physics2D.OverlapCircle(position, 0.000000000001f, shipMask)||
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
            Instantiate(breachPrefab, position, Quaternion.Euler(0, 0, Random.Range(0, 360)), breachParent.transform);
        }
    }
}