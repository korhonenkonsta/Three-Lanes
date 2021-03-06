﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public int hpRegen;
    public float nextRegenTime;
    public float regenInterval = 3f;
    public int armor;
    public Image healthBarForeground;
    public Player owner;

    AudioSource audioSource;
    public GameObject explosionEffect;

    bool isDead;

    void Start()
    {
        hp = maxHp;
    }

    public void ChangeMaxHp(int amount)
    {
        maxHp += amount;
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            hp += amount + armor;
        }
        else
        {
            hp += amount;
        }
        

        if (healthBarForeground)
        {
            healthBarForeground.fillAmount = (float)hp / maxHp;
        }

        if (hp <= 0 && !isDead)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            if (GetComponent<Base>())
            {
                GetComponent<Base>().OnDeath();
            }
            else
            {
                if (GetComponent<Deathrattle>())
                {
                    GetComponent<Deathrattle>().OnDeath();
                }

                if (GetComponent<Explosion>())
                {
                    GetComponent<Explosion>().Explode(GetComponent<Unit>().owner);
                }

                owner.opponent.RemoveUnitFromTargetLists(transform);
            }
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            ParticleSystem PS = effect.GetComponent<ParticleSystem>();
            Destroy(effect, PS.main.duration);

            Destroy(gameObject);

            if (GetComponent<Unit>())
            {
                if (!GetComponent<Unit>().isBullet)
                {
                    GameObject fracturedObj = Instantiate(GetComponent<Unit>().fracturedObject, transform.position, transform.rotation);

                    if (fracturedObj.transform.localScale != transform.localScale / 2f)
                    {
                        fracturedObj.transform.localScale = transform.localScale / 2f;
                    }

                    foreach (Transform child in fracturedObj.transform)
                    {
                        child.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
                        child.GetComponent<Renderer>().materials[1].color = GetComponent<Renderer>().material.color;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (hp == maxHp)
        {
            nextRegenTime = Time.time + regenInterval;
        }
        else if (Time.time > nextRegenTime)
        {
            nextRegenTime = Time.time + regenInterval;
            hp += hpRegen;
        }
    }
}
