using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareAnimationEvent : MonoBehaviour
{
    public void GenerateBreachesAfterFlare()
    {
        FindObjectOfType<BreachGenerator>().GenerateBreaches();
    }

    public void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("Flare");
    }
}
