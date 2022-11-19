using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    void Awake() {

        healthBar = GetComponent<Slider>();

    }

    // percent should be in range [0, 1]
    void SetHealth(float value) {

        healthBar.value = value;

    }

}
