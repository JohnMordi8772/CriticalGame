using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeteriorationBehaviour : MonoBehaviour
{
    public SpriteMask sm;
    public float deteriorationModifier = 25f;
    public Slider slider;
    public float stressPlaceholder;


    // Update is called once per frame
    void Update()
    {
        sm.transform.localScale = Vector2.one * slider.value / deteriorationModifier;
    }
}
