using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// it's just grid layout with extended functionality
public class FlexibleGridLayout : LayoutGroup
{
   [Space]
   // protected variables
   [SerializeField] private int columns;
   [SerializeField] private int rows;
   
   // private variables
   private Vector2 cellSize;

   public override void CalculateLayoutInputHorizontal()
   {
      base.CalculateLayoutInputHorizontal();

      var rectTransformWidth = rectTransform.rect.width;
      var cellWidth = (rectTransformWidth / columns) -(padding.left / (float)columns) - (padding.right / (float) columns);
      cellSize.x = cellWidth;

      for (int i = 0; i < rectChildren.Count; i++)
      {
         var currentColumn = i % columns;
         var item = rectChildren[i];
         var xPos = cellSize.x * currentColumn + padding.left;
         
         SetChildAlongAxis(item, 0, xPos, cellSize.x);
      }
   }

   public override void CalculateLayoutInputVertical()
   {
      var rectTransformHeight = rectTransform.rect.height;
      var cellHeight = (rectTransformHeight / rows) - (padding.top / (float) rows) - (padding.bottom / (float) rows);
      cellSize.y = cellHeight;

      for (int i = 0; i < rectChildren.Count; i++)
      {
         var currentRow = i / columns;
         var item = rectChildren[i];
         var yPos = cellSize.y * currentRow + padding.top;

         SetChildAlongAxis(item, 1, yPos, cellSize.y);
      }
   }

   public override void SetLayoutHorizontal()
   {
   }

   public override void SetLayoutVertical()
   {
   }
}
