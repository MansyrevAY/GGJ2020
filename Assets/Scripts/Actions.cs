using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    public float repairOffset;
    public int fixInput;
    public float fixTime;

    private GameObject breaches;
    private Movement movement;

    private void Awake()
    {
        breaches = GameObject.FindGameObjectWithTag("BreachesParent");
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey((KeyCode) fixInput))
        {
            if (!movement.FlyingMode)
                FixBreach();
        }
    }

    private void FixBreach()
    {
        Transform[] breachesPositions = breaches.GetComponentsInChildren<Transform>();

        if (breachesPositions.Length > 1)
        {
            int repaired = 0;
            // Because i = 0 is parent object
            for (int i = 1; i < breachesPositions.Length; i++)
            {
                float robotBreachDistance = Vector3.Distance(breachesPositions[i].position, transform.position);
                Debug.Log(robotBreachDistance);
                if (robotBreachDistance < repairOffset)
                {
                    DisableBreach(breachesPositions[i].gameObject);
                    repaired++;
                }
            }

            Debug.Log($"Repaired: {repaired}");

            foreach (Transform breachesPosition in breachesPositions)
            {
                if (!breachesPosition.gameObject.activeInHierarchy)
                {
                    Debug.Log($"{breachesPosition.position} is destroyed");
                    Destroy(breachesPosition.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("No breaches to repair");
        }
    }

    private void DisableBreach(GameObject breach)
    {
        breach.SetActive(false);
    }
}
