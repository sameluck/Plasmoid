using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private Image healthIndicator;
    public Sprite healthFull;
    public List<Sprite> healthStates;
    
    void Start()
    {
        healthIndicator = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager._playerHealth.untouched())
        {
            healthIndicator.sprite = healthFull;
        }
        else
        {
            healthIndicator.sprite = healthStates[GameManager.gameManager._playerHealth.Health];
        }
    }
}
