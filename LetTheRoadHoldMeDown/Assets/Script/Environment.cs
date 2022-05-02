using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    int minStress, maxStress;
    public int currentStress;
    [SerializeField] GameManager gameManager;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        minStress = -2;
        currentStress = minStress;
        maxStress = 3;
        cam = GameObject.FindObjectOfType<Camera>();
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
                cam.backgroundColor += new Color((41f + (214f * ((currentStress + 2) / 5))) / 255, 0, 0);
                Debug.Log(cam.backgroundColor);
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
            cam.backgroundColor = new Color((41f + (214f * ((currentStress + 2) / 5))) / 255, 41f / 255, 41f / 255);
            StartCoroutine(IncreasingStress());
            gameManager.SetCurrentEnvironment(this);
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
