using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGlowEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer eye;
    float amount = 1;
    bool state = true;

    void Update()
    {
        if (state == true)
        {
            amount -= Time.deltaTime;
            eye.color = new Color(amount, 0, 0);
            if (amount <= 0) { state = false; }
        }
        else
        {
            amount += Time.deltaTime;
            eye.color = new Color(amount, 0, 0);
            if (amount >= 2) { state = true; }
        }
    }

}
