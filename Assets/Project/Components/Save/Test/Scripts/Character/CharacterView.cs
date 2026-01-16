using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace My.Save
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField]
        private InputField idInputField;
        
        [SerializeField]
        private InputField nameInputField;
        
        public void SetCharacter(TestCharacter testCharacter)
        {
            idInputField.text = $"{testCharacter.Id}";
            nameInputField.text = testCharacter.Name;
        }
        
        public TestCharacter GetCharacter()
        {
            return new TestCharacter(int.Parse(idInputField.text), nameInputField.text);
        }
    }
}