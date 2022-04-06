using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    List<GameObject> candy, games, pillows;
    int stressValue, stressMax;
    static int stressMultiplier;
    static string lastItem;
    public Slider stressVisual;

    // Start is called before the first frame update
    void Start()
    {
        stressVisual.value = 0;
        stressMultiplier = 1;
        stressMax = 200;
        stressVisual.maxValue = stressMax;
        StartCoroutine(StressGain());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && stressMultiplier != 5)
        {
            stressMultiplier++;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            stressMultiplier = 1;
        }
        if(stressVisual.value == stressMax)
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator StressGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            stressVisual.value += stressMultiplier;
        }
    }

    public static void AdjustMultiplier(string tag)
    {
        if(tag != lastItem)
        {
            stressMultiplier++;
        }
        else
        {
            stressMultiplier = 1;
        }
    }
}
