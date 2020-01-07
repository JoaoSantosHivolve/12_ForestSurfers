using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour
{
    public List<GameObject> obstacles;
    public List<GameObject> instantiatedObstacles;
    public SphereRotation world;
    public Transform parent;

    [Header("Properties")]
    public int obstacleCount;
    public int obstacleTarget;
    public int lastIndex;

    [Header("Timer")]
    public float timer;
    private const float TIMER_THRESHOLD = 1.5f;

    private Action m_Action;

    private void Update() => m_Action?.Invoke();

    public void StartGenerator()
    {
        ResetGenerator();
        m_Action += Generator;
    }
    public void ResetGenerator()
    {
        timer = TIMER_THRESHOLD;
        obstacleCount = 0;
        m_Action -= Generator;
        foreach (var o in instantiatedObstacles)
        {
            Destroy(o.gameObject);
        }
    }

    private int GetObstacleIndex()
    {
        int index;

        do
        {
            index = Random.Range(1, obstacles.Count);
        } 
        while (index == lastIndex);
        lastIndex = index;

        return index;
    }
    private void Generator()
    {
        timer += world.playerOnOil ? Time.deltaTime / 2 : Time.deltaTime;
        if (timer >= TIMER_THRESHOLD && obstacleCount <= obstacleTarget)
        {
            timer = 0;
            obstacleCount += 1;
            var index = obstacleCount > obstacleTarget ? 0 : GetObstacleIndex();
            GenerateObstacle(index);
        }
    }
    private void GenerateObstacle(int index)
    {
        var obstacle = Instantiate(obstacles[index]);
        obstacle.transform.position = new Vector3(0, 0, -23.5f);
        obstacle.transform.localEulerAngles = new Vector3(-90, 0,0);
        obstacle.transform.SetParent(parent);
        obstacle.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        
        Destroy(obstacle, 8f);
        instantiatedObstacles.Add(obstacle);
    }
}