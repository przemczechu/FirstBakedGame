using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainGame : Singleton<MainGame>
{
    [Header("Prefabs:")]
    public GameObject goalPrefab;
    public GameObject particlePrefab;

    [Header("Spawn positions:")]
    public Transform spawnSlotsParent;

    [Header("Goals:")]
    public GoalType[] goalTypes;

    [Header("Stages:")]
    public Stage[] Stages;

    //Current stage info
    private int currentStageIndex = -1;
    private Stage currentStage;

    //Current game values
    private float currentPoints;
    private float time;
    private List<Goal> createdGoals = new List<Goal>();

    void Start()
    {
        StartNewGame();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > currentStage.frequencyInSeconds)
        {
            time = 0;

            //Can create new goal if is empty place
            if (createdGoals.Count < spawnSlotsParent.childCount)
            {
                CreateNewGoal(currentStage);
            }
            //No empty places. Game over!
            else
            {
                this.enabled = true;
                MainUI.FindInstance.ShowGameOverView(currentPoints);
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    CurrentClickedGoal(raycastHit.transform.GetComponent<Goal>());
                }
            }
        }
    }

    public void StartNewGame()
    {
        Debug.Log("Start new game.");
        currentStageIndex = -1;
        ResetValues();
        SetNextStage();
    }

    void ResetValues()
    {
        currentPoints = 0;
        time = 0;
    }

    void SetNextStage()
    {
        //Increase stage index
        currentStageIndex++;
        currentStage = Stages[currentStageIndex];

        Debug.Log("Next Stage Index: "+ currentStageIndex);

        ResetValues();
        DestroyGoals();

        ProgressBar.FindInstance.SetProgressBar(0, currentStage.pointsThreshold);
    }

    void CreateNewGoal(Stage stage)
    {
        //Choosing a spawn slot and setting it as the first child
        int emptySlots = spawnSlotsParent.childCount - createdGoals.Count;
        int randomChildIndex = createdGoals.Count + Random.Range(0, emptySlots);
        Transform goalParent = spawnSlotsParent.GetChild(randomChildIndex);
        goalParent.SetAsFirstSibling();

        //Create goal in the chosen slot
        Goal goal = Instantiate(goalPrefab, goalParent).GetComponent<Goal>();
        goal.transform.localPosition = Vector3.zero;// goalPosition.position;
        createdGoals.Add(goal);

        //Goal type randomization
        int goalTypeIndex = Random.Range(0, stage.goalTypes.Length);
        goal.SetGoal(goalTypes[goalTypeIndex]);
    }

    void RemoveGoalFromList(Goal goal)
    {
        for (int i = createdGoals.Count - 1; i >= 0; i--)
            if (createdGoals[i] == goal)
                createdGoals.RemoveAt(i);
    }

    void DestroyGoals()
    {
        for (int i = createdGoals.Count - 1; i >= 0; i--)
            if(createdGoals[i] != null)
                Destroy(createdGoals[i].gameObject);

        createdGoals = new List<Goal>();
    }

    void CurrentClickedGoal(Goal goal)
    {
        if (goal == null)
            return;

        //Create particles
        ParticleSystem particleSystem = Instantiate(particlePrefab).GetComponent<ParticleSystem>();
        particleSystem.transform.position = goal.transform.position;
        particleSystem.startColor = goal.GetComponent<MeshRenderer>().material.color;
        Destroy(particleSystem.gameObject, 2.0f);

        //Add points
        AddPoints(goal.points);

        //Set the slot as last because it is already empty
        goal.transform.parent.SetAsLastSibling();

        RemoveGoalFromList(goal);

        //Destroy clicked goal
        Destroy(goal.gameObject);
    }

    void AddPoints(float points)
    {
        currentPoints += points;
        ProgressBar.FindInstance.SetProgressBar(currentPoints, currentStage.pointsThreshold);

        if (IsStageAchieved() && !IsLastStage())
            SetNextStage();
    }

    bool IsLastStage() { return currentStageIndex == (Stages.Length - 1); }
    bool IsStageAchieved() { return currentPoints >= currentStage.pointsThreshold; }


}
