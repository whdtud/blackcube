using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var character = collision.gameObject.GetComponent<CombatCharacterBase>();
        if (character == null)
            return;

        character.OnDead();
    }
}
