using UnityEngine;

public class IngredientAnimationManager : MonoBehaviour {
    public IngredientAnimation[] animationPrefabs;

    private void Start() {
        foreach (var ingredientAnimation in animationPrefabs) {
            // Assign the mecanim to the animator to control
            ingredientAnimation.animator.runtimeAnimatorController = ingredientAnimation.animatorController;
        }
    }
}
