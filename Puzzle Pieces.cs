using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzlePieces : MonoBehaviour
{
    private Vector3 RightPosition;
    void Start()
    {
        // Save the correct position
        RightPosition = transform.position;

        // Move the piece to a random position
        float randomX = Random.Range(5f, 11f);
        float randomY = Random.Range(-7f, 2.5f); // Make sure min < max
        transform.position = new Vector3(randomX, randomY, 0f);
    }

    void Update()
    {
        // If the piece is close to the correct spot, snap it into place
        if (Vector3.Distance(transform.position, RightPosition) < 0.5f)
        {
            transform.position = RightPosition;
        }
    }
}



