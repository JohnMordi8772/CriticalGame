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
    static Slider stressVisualStatic;
    public Camera cameraMain;

    // Start is called before the first frame update
    void Awake()
    {
        items = itemsRef;
        stressVisual.value = 0;
        stressMultiplier = -8;
        stressMax = 400;
        sweetCount = 16;
        sleepCount = 18;
        gameCount = 16;
        stressVisual.maxValue = stressMax;
        stressVisualStatic = stressVisual;
        StartCoroutine(StressGain());
    }

    // Update is called once per frame
    void Update()
    {
        environmentText.text = "Environmental Stress: " + environment.currentStress;
        //cameraMain.backgroundColor = Color.Lerp(cameraMain.backgroundColor, new Color((41f + (214f * ((environment.currentStress + 2) / 5)))/255, 41f/255, 41f/255), Mathf.PingPong(Time.time, 1));
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
            stressVisual.value += 2 + environment.currentStress;
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
            stressVisualStatic.value += stressMultiplier;
            stressMultiplier += 2;
        }
        else
        {
            lastItem = tag;
            stressMultiplier = -8;
            stressVisualStatic.value += stressMultiplier;
            stressMultiplier = -6;
        }
        items.Remove(obj);
    }

    public void SetCurrentEnvironment(Environment inEnvironment)
    {
        environment = inEnvironment;
    }
}
