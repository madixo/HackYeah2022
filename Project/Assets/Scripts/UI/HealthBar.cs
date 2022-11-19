using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    public void Start() {

        healthBar = GetComponent<Slider>();

    }

    // percent should be in range [0, 1]
    public void SetHealth(float value) {

        healthBar.value = value;

    }

}
