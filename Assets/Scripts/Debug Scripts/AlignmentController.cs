using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AlignmentController : MonoBehaviour
{
    public Transform HandTransform;

    public float ABDistance = 1f;

    public enum AlignmentState
    {
        None, 
        PivotOneSet, 
        PivotTwoSet,
        PivotThreeSet
    }

    // public Vector3 OriginOffset;
    // public Vector3 RotationOffset;

    public AlignmentState alignmentState = AlignmentState.None;

    private void Update()
    {
        switch (alignmentState)
        {
            case AlignmentState.None:
                if (!Input.GetKeyDown("t")) return;
                transform.position = HandTransform.position; // + OriginOffset;
                alignmentState = AlignmentState.PivotOneSet;
                break;

            case AlignmentState.PivotOneSet:
                if (Input.GetKeyDown("y"))
                {
                    alignmentState = AlignmentState.None;
                    return;
                }

                if (!Input.GetKeyDown("t")) return;

                var lookAtPosition1 = HandTransform.position; // + OriginOffset;
                var pivotOneToTwo = lookAtPosition1 - transform.position;
                // var scaleFactor1 = pivotOneToTwo.magnitude / ABDistance;

                transform.LookAt(lookAtPosition1, Vector3.up);
                // transform.rotation *= RotationOffset;
                // transform.localScale *= scaleFactor;

                alignmentState = AlignmentState.PivotTwoSet;
                break;
            
            case AlignmentState.PivotTwoSet:
                if (Input.GetKeyDown("y"))
                {
                    alignmentState = AlignmentState.None;
                    return;
                }

                if (!Input.GetKeyDown("t")) return;

                var lookAtPosition2 = HandTransform.position; // + OriginOffset;
                var pivotTwoToThree = lookAtPosition2 - transform.position;
                // var scaleFactor2 = pivotTwoToThree.magnitude / ABDistance;

                transform.LookAt(lookAtPosition2, Vector3.up);
                // transform.rotation *= RotationOffset;
                // transform.localScale *= scaleFactor;

                alignmentState = AlignmentState.PivotThreeSet;
                break;

            case AlignmentState.PivotThreeSet:
                if (Input.GetKeyDown("y"))
                {
                    alignmentState = AlignmentState.None;
                }
                break;
        }
    }
}