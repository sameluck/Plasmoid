using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScript : MonoBehaviour
{
    private static string _baseMessage = "Defeat\nRestart in ";
    [SerializeField] private TextMeshProUGUI defeatText;
    private bool _startedCountdown = false;
    
    
    private void Update()
    {
        if (GameManager.gameManager._playerHealth.Health <= 0)
        {
            if (!_startedCountdown)
            {
                _startedCountdown = true;
                StartCoroutine(Countdown());
            }
        }
    }
    
    private IEnumerator Countdown()
    {
        defeatText.gameObject.SetActive(true);
        for (int i = 5; i >= 0; i--) {
            defeatText.SetText(_baseMessage+i);
            yield return new WaitForSeconds(1f);
        }
        LevelController.LoadLevel(SceneManager.GetActiveScene().name);
    }

}
