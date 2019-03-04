using UnityEngine;

[System.Serializable]
public class IngredientAnimation {
    public string name;
    [HideInInspector] public Animator animator; // Access this to control animation
    public RuntimeAnimatorController animatorController;
}
