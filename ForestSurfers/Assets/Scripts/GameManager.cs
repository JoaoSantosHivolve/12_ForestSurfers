using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObstacleGenerator generator;
    public CharacterBehaviour behaviour;
    public SphereRotation rotation;

    public void StartGame_OnClick()
    {
        rotation.StopRotation(false);
        generator.StartGenerator();
        behaviour.ResetBehaviour();
    }
    public void ResetGame_OnClick()
    {
        generator.ResetGenerator();
        behaviour.ResetBehaviour();
    }
}
