using System;
using System.Collections;
using UnityEngine;

public class ObstacleHitDetection : MonoBehaviour
{
    public CharacterBehaviour behaviour;
    public SphereRotation world;

    public GameObject onCollisionUi;
    public GameObject onWinUi;

    private void OnCollisionEnter(Collision other)
    {
        if (IsObstacle(other) || IsTrunk(other))
        {
            // Ignore collision if player is behind the object
            if (transform.position.z < other.transform.position.z)
                return;

            // Hit / Fall Animation
            if (IsObstacle(other))
                behaviour.HitWall();
            else 
                behaviour.FallFlat();

            // Stop World rotation
            world.StopRotation(true);

            // Open Ui
            StartCoroutine(OpenUi(onCollisionUi));
        }
        else if (IsOil(other))
        {
            world.playerOnOil = true;
        }
        else if (IsTheEnd(other))
        {
            // Open Ui
            StartCoroutine(OpenUi(onWinUi));
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (IsOil(other))
        {
            world.playerOnOil = false;
        }
    }

    private static IEnumerator OpenUi(GameObject ui)
    {
        yield return new WaitForSeconds(2.25f);

        ui.SetActive(true);
    }
    private static bool IsObstacle(Collision other)
    {
        return other.transform.CompareTag("Obstacle");
    }
    private static bool IsTheEnd(Collision other)
    {
        return other.transform.CompareTag("End");
    }
    private static bool IsTrunk(Collision other)
    {
        return other.transform.CompareTag("Trunk");
    }
    private static bool IsOil(Collision other)
    {
        return other.transform.CompareTag("Oil");
    }
}