using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float originalGravity;
    public GameManager gameManager;
    
    public abstract void Act();

    protected void Setup()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        SwitchToEdit();
    }

    public void SwitchToEdit()
    {
        rb.gravityScale = 0f;
    }

    public void SwitchToGame()
    {
        rb.gravityScale = originalGravity;
    }

    protected void BaseCollisionChecks(Collision2D other)
    {
        if (other.gameObject.tag.Equals("death")) {
            // spikes / bottom of screen / etc
            gameManager.EnemyDied(this);
            return;
        }
    }

    protected void BaseTriggerChecks(Collider2D other)
    {
        // nothing atm
    }
}