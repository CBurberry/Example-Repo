using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicineCounterUI : MonoBehaviour
{
    [SerializeField] Text counterText;
    private int current;
    private int max;
    private Image previewImage;

    public void Initialize(int max, Sprite icon = null) 
    {
        current = 0;
        this.max = max;
        counterText.text = $"{current} / {max}";
        SetSprite(icon);
    }

    public void IncrementCount() 
    {
        current++;
        SetCount(current);
    }

    public void DecrementCount()
    {
        current--;
        SetCount(current);
    }

    public void SetCount(int newCount) 
    {
        current = newCount;
        counterText.text = $"{current} / {max}";
    }

    public void SetSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            previewImage = GetComponent<Image>();
            previewImage.sprite = sprite;
        }
    }
}
