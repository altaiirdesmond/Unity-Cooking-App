using TMPro;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {
    public void RemoveSprite() {
        GetComponent<SpriteRenderer>().sprite = null;
    }

    public void SetTrivia() {
        // Get the trivia container gameobject
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(FindObjectOfType<CookingManager>().Trivia);
    }
}
