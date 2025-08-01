using _GAME;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private Animator animator;
    
    public override void BeginInteract(Player player)
    {
        animator.SetBool("IsOpen", !animator.GetBool("IsOpen"));
    }

    public void Close()
    {
        animator.SetBool("IsOpen", false);
    }

    public void Open()
    {
        animator.SetBool("IsOpen", true);
    }
}
