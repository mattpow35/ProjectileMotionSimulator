using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Text angleText;
    public Slider angleSlider;
    public Text velText;
    public Slider velSlider;
    public GameObject projectile;
    
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderChanged()
    {
        angleText.text = $"Angle: {angleSlider.value} deg.";
    }

    public void SliderChangedVelocity()
    {
        velText.text = $"Velocity: {velSlider.value} m/s";
    }
}
