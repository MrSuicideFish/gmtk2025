using UnityEngine;

namespace _GAME.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorVariable : MonoBehaviour
    {
        public string VariableName;

        public void SetBool(bool value) => GetComponent<Animator>().SetBool(VariableName, value);
        public void SetFloat(float value) => GetComponent<Animator>().SetFloat(VariableName, value);
    }
}