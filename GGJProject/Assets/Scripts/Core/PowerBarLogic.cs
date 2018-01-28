﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PowerBarLogic : MonoBehaviour {

    public Image EnergyBar;

    public int FullEnergy = 100;
    public int CurrentEnergy;

    void Start () {
        if (EnergyBar == null)
        {
            EnergyBar = GetComponent<Image>();
        }

        CurrentEnergy = FullEnergy;
        UpdateEnergy();
    }

    public void LoseEnergy(int amount)
    {
        CurrentEnergy -= amount;
        UpdateEnergy();

        //TODO effect
    }

    public void Recharge(int amount)
    {
        CurrentEnergy += amount;
        CurrentEnergy = (CurrentEnergy > FullEnergy) ? FullEnergy : CurrentEnergy;
        UpdateEnergy();

        //TODO effect
    }

    void UpdateEnergy()
    {
        EnergyBar.fillAmount = (float) CurrentEnergy / FullEnergy;
    }
}
