using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// this component will change amount of letters and randomize them
public class LettersHandler : MonoBehaviour
{
    [FormerlySerializedAs("lettersContainerSerializable"),SerializeField] private Transform lettersContainer;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI widthText;
    [Space]
    [SerializeField] private bool autoRandomizeOnChangeSize;
    
    // private static variables
    private static FlexibleGridLayout _flexibleGridLayout;
    private static GameObject _letter;

    private void Awake()
    {
        _flexibleGridLayout = lettersContainer.GetComponent<FlexibleGridLayout>();
        _letter = lettersContainer.GetChild(0).gameObject;
    }

    // will invoke in input field "OnEndEdit"
    public void ChangeLettersAmount()
    {
        var heightString = Regex.Replace(heightText.text, @"[^0-9]","");
        var widthString = Regex.Replace(widthText.text, @"[^0-9]","");

        int.TryParse(heightString, out int height);
        int.TryParse(widthString, out int width);

        if ((height > 1 && width > 1) == false)
            return;
        
        _flexibleGridLayout.ChangeSize(width, height);
        
        for (int i = 1; i < lettersContainer.childCount; i++)
        {
            Destroy(lettersContainer.GetChild(i).gameObject);
        }

        for (int i = 1; i < _flexibleGridLayout.columns * _flexibleGridLayout.rows; i++)
        {
            Instantiate(_letter, lettersContainer);
        }
        
        if (autoRandomizeOnChangeSize)
            GenerateRandomLetters();
    }

    // invoke on button press
    public void GenerateRandomLetters()
    {
        for (int i = 0; i < lettersContainer.childCount; i++)
        {
            lettersContainer.GetChild(i).GetComponent<TextMeshProUGUI>().text = ((char) Random.Range(97, 123)).ToString();
        }
    }
}
