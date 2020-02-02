using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
    public float repairOffset;
    public int fixInput;
    public float FixSpeed = 0.1f;

    [Header("Sprites")]
    public SpriteRenderer frontRobot;
    public SpriteRenderer backRobot;

    private float fixProgress = 0;
    private Slider slider;
    private GameObject breaches;
    private Movement movement;

    private void Awake()
    {
        breaches = GameObject.FindGameObjectWithTag("BreachesParent");
        movement = GetComponent<Movement>();
        slider = GetComponentInChildren<Slider>();
        slider.gameObject.SetActive(false);
        backRobot.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKey((KeyCode)fixInput))
        {

            if (!movement.FlyingMode)
                FixBreach();

            if(!backRobot.enabled)
            {
                frontRobot.enabled = false;
                backRobot.enabled = true;
            }

        }
        else
        {
            fixProgress = 0;
            slider.value = fixProgress;
            slider.gameObject.SetActive(false);

            frontRobot.enabled = true;
            backRobot.enabled = false;
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
                    if (fixProgress >= 1)
                    {
                        BreachGenerator.CurrentNumberOnShip -= 1;
                        DisableBreach(breachesPositions[i].gameObject);
                        repaired++;
                        fixProgress = 0;
                        slider.value = fixProgress;
                        slider.gameObject.SetActive(false);
                    }
                    else
                    {
                        slider.gameObject.SetActive(true);
                        fixProgress += FixSpeed;
                        slider.value = fixProgress;
                        Debug.Log(fixProgress);
                    }
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
