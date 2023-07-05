
using Abstract;
using UnityEngine;

public class PlayerStats : Stats
{
    
    [SerializeField] private float runSpeed=2f;
    [SerializeField] private float jumpForce = 4f;
    public float RunSpeed => runSpeed;
    public float JumpForce => jumpForce;
    
    

}
