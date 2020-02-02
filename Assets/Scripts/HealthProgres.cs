using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthProgres : MonoBehaviour
{
    public BreachGenerator generator;
    public Image hpBackground;

    private int maxHp;
    private int currentBreaches;
    private int currentHP;
    // Start is called before the first frame update
    void Awake()
    {
        maxHp = generator.maxNumberOnShip;
        currentHP = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        currentBreaches = BreachGenerator.CurrentNumberOnShip;
        currentHP = maxHp - currentBreaches;

        hpBackground.fillAmount = (float)currentHP / maxHp;
    }
}
