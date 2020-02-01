﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachGenerator : MonoBehaviour
{
    public GameObject breachParent;
    public GameObject breachPrefab;
    public Collider2D borders;
    public int maxNumberOfBreachesPerSolarFlare;
    public LayerMask otherBreaches;
    
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
        int numberOfBreaches = Random.Range(1, maxNumberOfBreachesPerSolarFlare + 1);
        for (int i = 0; i < numberOfBreaches; i++)
        {
            float randomX = Random.Range(borders.bounds.center.x - borders.bounds.extents.x,
                borders.bounds.center.x + borders.bounds.extents.x);
            float randomY = Random.Range(borders.bounds.center.y - borders.bounds.extents.y,
                borders.bounds.center.y + borders.bounds.extents.y);
            Vector2 position = new Vector2(randomX, randomY);

            // TODO: Update with OverlapBox
            while (Physics2D.OverlapCircle(position, 0.5f, otherBreaches))
            {
                Debug.Log($"I didn't like {position}");
                randomX = Random.Range(borders.bounds.center.x - borders.bounds.extents.x,
                    borders.bounds.center.x + borders.bounds.extents.x);
                randomY = Random.Range(borders.bounds.center.y - borders.bounds.extents.y,
                    borders.bounds.center.y + borders.bounds.extents.y);
                position = new Vector2(randomX, randomY);
            }

            Debug.Log($"Finally Here: {position}");
            Instantiate(breachPrefab, position, Quaternion.identity, breachParent.transform);
        }
    }
}