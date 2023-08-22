using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpressAnimation : MonoBehaviour
{
    private Animator animator = default;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Assert(animator != null);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EmpressDamage()
    {
        animator.SetTrigger("Damage");

    }
}
