using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public float points;

    public void SetGoal(GoalType goalType) {
        SetGoal(goalType.scale, goalType.points);
    }

    public void SetGoal(float scale, float points)
    {
        this.transform.localScale = Vector3.one * scale;
        this.points = points;

        //Setting a random color
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.material.color = Random.ColorHSV();
    }

}
