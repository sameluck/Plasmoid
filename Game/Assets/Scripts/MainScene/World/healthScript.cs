using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthScript
{
    int _currentHealth;
    int _maxhealth;
    bool _dead;
    private bool _untouched;

    public int Health
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }

    public int MaxHealth
    {
        get { return _maxhealth; }
        set { _maxhealth = value; }
    }

    public healthScript(int health, int maxhealth)
    {
        _currentHealth = health;
        _maxhealth = maxhealth;
        if (health > 0)
        {
            _dead = false;
        }
        else
        {
            _dead = true;
        }

        _untouched = true;
    }

    public healthScript(int health)
    {
        _currentHealth = health;
        _maxhealth = health;

        if (health > 0)
        {
            _dead = false;
        }
        else
        {
            _dead = true;
        }

        _untouched = true;
    }

    public void TakeDamage(int dmg)
    {
        _currentHealth -= dmg;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _dead = true;
        }

        _untouched = false;
    }

    public void HealDamage(int heal)
    {
        if (!_dead)
        {
            _currentHealth += heal;
            if (_currentHealth > _maxhealth)
            {
                _currentHealth = _maxhealth;
            }
        }
    }

    public bool alive()
    {
        return !_dead;
    }

    public bool untouched()
    {
        return _untouched;
    }
}