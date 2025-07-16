
using Unity.Mathematics;
using UnityEngine;

public class Char1 : PlayerAll
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
        Jump();
        Run();
        Attack();
        animator.SetFloat("AirSpeedY", RB.linearVelocity.y);
        Roll();
        animator.SetBool("WallSlide", false);


    }
}

    