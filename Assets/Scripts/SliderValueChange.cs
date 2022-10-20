using UnityEngine;
using UnityEngine.UI;

public class SliderValueChange : MonoBehaviour
{
    private Slider slider;
    private Text textComp;
    public string leadText;

    void Awake()
    {
        slider = GetComponentInParent<Slider>();
        textComp = GetComponent<Text>();
    }

    void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    void UpdateText(float val)
    {
        textComp.text = leadText + val.ToString();
    }
}