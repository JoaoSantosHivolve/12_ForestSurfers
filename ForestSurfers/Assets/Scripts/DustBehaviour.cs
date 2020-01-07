using UnityEngine;

public class DustBehaviour : MonoBehaviour
{
    public CharacterBehaviour behaviour;
    public SphereRotation world;
    private ParticleSystem m_System;

    private void Awake()
    {
        m_System = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if ((behaviour.OnAnimation(PlayerAnims.Jump) 
             || behaviour.OnAnimation(PlayerAnims.FallFlat) 
             || behaviour.OnAnimation(PlayerAnims.HitWall) 
             || world.playerOnOil) 
            && !m_System.isStopped)
            m_System.Stop();
        else if(behaviour.OnAnimation(PlayerAnims.Run) && !m_System.isPlaying)
            m_System.Play();

        if(behaviour.OnAnimation(PlayerAnims.Slide))
            m_System.Emit(1);
    }
}
