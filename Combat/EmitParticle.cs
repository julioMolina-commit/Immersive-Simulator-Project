using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticle : MonoBehaviour
{ 
    public void OnEmitParticle(int emitionAmount)
    {
        GetComponentInChildren<ParticleSystem>().Emit(emitionAmount);
    }
}
