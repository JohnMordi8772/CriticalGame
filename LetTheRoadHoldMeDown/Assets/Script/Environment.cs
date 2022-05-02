using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    int minStress, maxStress;
    public int currentStress;
    [SerializeField] GameManager gameManager;
    Camera[] cam;
    // Start is called before the first frame update
    void Start()
    {
        minStress = 0;
        currentStress = minStress;
        maxStress = 5;
        cam = GameObject.FindObjectsOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IncreasingStress()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (currentStress != maxStress)
            {
                currentStress++;
                cam[0].backgroundColor += new Color((41f + (214f * ((currentStress) / 5f))) / 255, 0, 0);
                cam[1].backgroundColor += new Color((41f + (214f * ((currentStress) / 5f))) / 255, 0, 0);
                //Debug.Log(cam.backgroundColor);
            }
        }
    }

    IEnumerator DecreasingStress()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            if (currentStress != minStress)
                currentStress--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(IncreasingStress());
            gameManager.SetCurrentEnvironment(this);
            cam[0].backgroundColor = new Color((41f + (214f * ((currentStress) / 5f))) / 255, 41f / 255, 41f / 255);
            cam[1].backgroundColor = new Color((41f + (214f * ((currentStress) / 5f))) / 255, 41f / 255, 41f / 255);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(DecreasingStress());
        }
    }
}
