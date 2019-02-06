using UnityEngine;

public class UIAnimation : MonoBehaviour {

    [SerializeField] private Animator animator;

    public Animator Animator {
        get {
            return animator;
        }

        set {
            animator = value;
        }
    }
}
