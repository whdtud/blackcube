using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var character = other.gameObject.GetComponent<CombatCharacterBase>();
        if (character == null)
            return;

        character.OnDead();
    }
}
