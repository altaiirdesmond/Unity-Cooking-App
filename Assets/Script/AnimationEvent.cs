using TMPro;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {

    public void RemoveSprite() {
        GetComponent<SpriteRenderer>().sprite = null;
    }
}
