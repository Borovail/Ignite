using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class SelectionAreaManager : MonoBehaviour
    {
        public RectTransform SelectionArea;
        public Canvas ParentCanvas;
        private static Vector3 _startPosition;

        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SelectionArea.gameObject.SetActive(false);
            }
            if (Input.GetMouseButtonDown(0))
            {
                SelectionArea.gameObject.SetActive(true);
                _startPosition = Input.mousePosition;
            }

            if (SelectionArea.gameObject.activeSelf)
            {
                var selectionArea = GetSelectionArea();
                var canvasScale = ParentCanvas.transform.localScale.x;
                SelectionArea.anchoredPosition = new Vector2(selectionArea.x, selectionArea.y) / canvasScale;
                SelectionArea.sizeDelta = new Vector2(selectionArea.width, selectionArea.height) / canvasScale;
            }
        }

        public static Rect GetSelectionArea()
        {
            var leftCorner = new Vector2
            (
                Math.Min(_startPosition.x, Input.mousePosition.x),
                Math.Min(_startPosition.y, Input.mousePosition.y)
            );
            var rightCorner = new Vector2
            (
                Math.Max(_startPosition.x, Input.mousePosition.x),
                Math.Max(_startPosition.y, Input.mousePosition.y)
            );

            return new Rect(leftCorner.x, leftCorner.y,
                rightCorner.x - leftCorner.x,
                rightCorner.y - leftCorner.y
            );
        }
    }
}