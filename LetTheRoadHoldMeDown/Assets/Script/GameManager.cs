using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static List<GameObject> items;
    public List<GameObject> itemsRef;
    static int sweetCount, sleepCount, gameCount;
    int stressValue, stressMax;
    static int stressMultiplier;
    static string lastItem;
    Environment environment;
    public Text environmentText, itemText, totalText, sweetText, sleepText, gameText;
    public GameObject winScreen, loseScreen;
    public Slider stressVisual;

    // Start is called before the first frame update
    void Start()
    {
        items = itemsRef;
        stressVisual.value = 0;
        stressMultiplier = -3;
        stressMax = 400;
        sweetCount = 16;
        sleepCount = 18;
        gameCount = 16;
        stressVisual.maxValue = stressMax;
        StartCoroutine(StressGain());
    }

    // Update is called once per frame
    void Update()
    {
        environmentText.text = "Environmental Stress: " + environment.currentStress;
        itemText.text = "Item Stress: " + stressMultiplier;
        totalText.text = stressVisual.value + "/" + stressMax;
        if(stressVisual.value == stressMax)
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        if (items.Count <= 0)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
        if(Time.timeScale == 0 && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        sweetText.text = ": " + sweetCount;
        sleepText.text = ": " + sleepCount;
        gameText.text = ": " + gameCount;

    }

    IEnumerator StressGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            //stressValue += stressMultiplier + environment.currentStress;
            stressVisual.value += stressMultiplier + environment.currentStress;
        }
    }

    public static void AdjustMultiplier(string tag, GameObject obj)
    {
        if (tag == "Sweets")
        {
            sweetCount--;
        }
        else if (tag == "Sleep")
        {
            sleepCount--;
        }
        else
        {
            gameCount--;
        }
        if(tag == lastItem)
        {
            stressMultiplier++;
        }
        else
        {
            lastItem = tag;
            stressMultiplier = -1;
        }
        items.Remove(obj);
    }

    public void SetCurrentEnvironment(Environment inEnvironment)
    {
        environment = inEnvironment;
    }
}
