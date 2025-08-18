using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DragandDrop : MonoBehaviour
{
    public GameObject SelectedPiece;
    public float snapSize = 1f;
    public bool rotationOnDrop = true;
    public bool dragging = false;
    void Start()
    {
        if (Input.GetMouseButtonDown(0)) //for when the mouse is pressed down 
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.transform.CompareTag("Puzzle"))
            {
                SelectedPiece = hit.transform.gameObject;
            }
            ///when the mouse is released snap the pieces to the grid
            if (Input.GetMouseButtonUp(0))
            {
                if (SelectedPiece != null)
                {
                    Vector3 pos = SelectedPiece.transform.position;
                    pos.x = Mathf.Round(pos.x / snapSize) * snapSize;
                    pos.y = Mathf.Round(pos.y / snapSize) * snapSize;
                    SelectedPiece.transform.position = pos;
                }
                if (rotationOnDrop)
                {
                    SelectedPiece.transform.Rotate(0, 0, 90); //will rotate the puzzle pieces by 90 degrees
                                                              //when the mouse is released and snappd into the grids
                }
                SelectedPiece = null;
            }

            // While dragging
            if (SelectedPiece != null)
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePoint.z = 0; // Keep on 2D plane
                SelectedPiece.transform.position = mousePoint;

            }
        }
    }
}



