using UnityEngine;

public class AnimationEvent : MonoBehaviour {

    [SerializeField] private SpriteRenderer spriteToRemove;

    public void RemoveSprite() {
        spriteToRemove.sprite = null;
    }
}
