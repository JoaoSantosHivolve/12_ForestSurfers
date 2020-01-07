using UnityEngine;

public class SphereRotation : MonoBehaviour
{
    public bool playerOnOil;
    public float rotationSpeed = 0.45f;

    private bool m_StopRotation;

    private void FixedUpdate()
    {
        if(!m_StopRotation) 
            transform.RotateAround(transform.position, Vector3.right, playerOnOil ? rotationSpeed / 2 : rotationSpeed);
    }

    public void StopRotation(bool state)
    {
        m_StopRotation = state;
    }
}
