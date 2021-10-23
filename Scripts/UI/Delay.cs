using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Delay : MonoBehaviour
{    
    void Start()
    {
        StartCoroutine(SceneDelay());
    }
    private IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
  
}
