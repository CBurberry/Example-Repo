using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicineCounterUI : MonoBehaviour
{
    [SerializeField] Text counterText;
    private int current;
    private int max;

    public void Initialize(int max) 
    {
        current = 0;
        this.max = max;
        counterText.text = $"{current} / {max}";
    }

    public void SetCount(int newCount) 
    {
        current = newCount;
        counterText.text = $"{current} / {max}";
    }
}
