using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeProgres : MonoBehaviour
{
    public BreachGenerator generator;
    public Image timeBackground;

    private float maxTime;
    private float currentTime = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        maxTime = generator.timeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        timeBackground.fillAmount = currentTime / maxTime;

        if (currentTime >= maxTime)
            Debug.Log("Success");
    }
}
