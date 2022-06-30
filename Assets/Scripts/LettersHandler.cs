using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// this component will change amount of letters, randomize and mix them
// This sounds like a god-object but I think we should do it in one class
// In my opinion it's right although we can split this into 3 different classes and it also will be fine
public class LettersHandler : MonoBehaviour
{
   [FormerlySerializedAs("lettersContainerSerializable"), SerializeField] private Transform lettersContainer;
   [SerializeField] private TextMeshProUGUI heightText;
   [SerializeField] private TextMeshProUGUI widthText;
   [Space]
   [SerializeField] private bool autoRandomizeOnChangeSize = true;
   [SerializeField] private bool increaseDensity = true;
   [SerializeField] private int densityModifier = 20; // increase it to increase density
   [SerializeField] private float lettersMoveTime = 2;

   // private variables
   private FlexibleGridLayout _flexibleGridLayout;
   private GameObject _letter;
   private bool _isAble2Action = true; // is able to mix or generate 

   private void Awake()
   {
      _flexibleGridLayout = lettersContainer.GetComponent<FlexibleGridLayout>();
      _letter = lettersContainer.GetChild(0).gameObject;
      
      SetLetters();
   }

   // will invoke in input field "OnEndEdit"
   public void SetLetters()
   {
      if (_isAble2Action == false)
         return;

      _flexibleGridLayout.enabled = true;
      
      ChangeLettersAmount();

      if (autoRandomizeOnChangeSize)
         GenerateRandomLetters();
      
      if (increaseDensity)
         ChangePadding();
      else
      {
         _flexibleGridLayout.padding.left = 0;
         _flexibleGridLayout.padding.right = 0;
         _flexibleGridLayout.padding.top = 0;
         _flexibleGridLayout.padding.bottom = 0;
      }

      void ChangeLettersAmount()
      {
         var heightString = Regex.Replace(heightText.text, @"[^0-9]", "");
         var widthString = Regex.Replace(widthText.text, @"[^0-9]", "");

         int.TryParse(heightString, out int height);
         int.TryParse(widthString, out int width);

         if ((height > 0 && width > 0) == false)
            return;

         _flexibleGridLayout.ChangeSize(width, height);

         for (int i = 1; i < lettersContainer.childCount; i++)
            Destroy(lettersContainer.GetChild(i).gameObject);

         for (int i = 1; i < _flexibleGridLayout.columns * _flexibleGridLayout.rows; i++)
            Instantiate(_letter, lettersContainer);
      }

      void ChangePadding()
      {
         if (_flexibleGridLayout.columns == _flexibleGridLayout.rows)
         {
            _flexibleGridLayout.padding.left = densityModifier;
            _flexibleGridLayout.padding.right = densityModifier;
            _flexibleGridLayout.padding.top = densityModifier;
            _flexibleGridLayout.padding.bottom = densityModifier;
         }
         else if (_flexibleGridLayout.columns < _flexibleGridLayout.rows)
         {
            var differenceModifier = _flexibleGridLayout.rows - _flexibleGridLayout.columns;

            _flexibleGridLayout.padding.left = differenceModifier * densityModifier;
            _flexibleGridLayout.padding.right = differenceModifier * densityModifier;
         }
         else
         {
            var differenceModifier = _flexibleGridLayout.columns - _flexibleGridLayout.rows;
            
            _flexibleGridLayout.padding.top = differenceModifier * densityModifier;
            _flexibleGridLayout.padding.bottom = differenceModifier * densityModifier;
         }
      }
   }

   // invoke on button "Сгенерировать" press
   public void GenerateRandomLetters()
   {
      if (_isAble2Action == false)
         return;
      
      _flexibleGridLayout.enabled = true;
      
      for (int i = 0; i < lettersContainer.childCount; i++)
      {
         lettersContainer.GetChild(i).GetComponent<TextMeshProUGUI>().text = ((char) Random.Range(97, 123)).ToString();
      }
   }

   // invoke on button "Перемешать"
   public void MixLetters()
   {
      if (_isAble2Action == false)
         return;
      
      _flexibleGridLayout.enabled = false;
      _isAble2Action = false;
      
      Dictionary<int, Vector2> positions = new Dictionary<int, Vector2>(); // index, position

      for (int i = 0; i < _flexibleGridLayout.transform.childCount; i++)
         positions.Add(i, _flexibleGridLayout.transform.GetChild(i).transform.position);

      for (int i = 0; i < _flexibleGridLayout.transform.childCount; i++)
      {
         var itemIndex = Random.Range(0, positions.Count);

         _flexibleGridLayout.transform.GetChild(i)
                            .DOMove(positions.ElementAt(itemIndex).Value, lettersMoveTime)
                            .OnComplete(() =>
                            {
                               _isAble2Action = true;
                            });
         
         positions.Remove(positions.ElementAt(itemIndex).Key);
      }
   }
}