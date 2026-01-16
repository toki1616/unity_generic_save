using UnityEngine;
using UnityEngine.UI;

namespace My.Save
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField]
        private InputField idInputField;

        [SerializeField]
        private InputField nameInputField;

        [SerializeField]
        private InputField typeInputField;

        public void SetEnemy(TestEnemy enemy)
        {
            idInputField.text = $"{enemy.Id}";
            nameInputField.text = enemy.Name;
            typeInputField.text = enemy.Type;
        }

        public TestEnemy GetEnemy()
        {
            return new TestEnemy(
                int.Parse(idInputField.text),
                nameInputField.text,
                typeInputField.text
            );
        }
    }
}
