using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaAnimation : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimationDamageArea()
    {
        Debug.Log("PlayAnimationDamageArea");
        animator.SetTrigger("isDamageAreaDamaged");
    }
}
