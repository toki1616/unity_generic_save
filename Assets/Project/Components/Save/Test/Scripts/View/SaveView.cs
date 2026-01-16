using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using My.Save;

public class SaveView : MonoBehaviour
{
    [SerializeField] private Button _characterSaveButton;
    [SerializeField] private Button _characterLoadButton;
    [SerializeField] private Button _enemySaveButton;
    [SerializeField] private Button _enemyLoadButton;

    void Start()
    {
        _characterSaveButton.onClick.AddListener(OnClickCharacterSave);
        _characterLoadButton.onClick.AddListener(OnClickCharacterLoad);

        _enemySaveButton.onClick.AddListener(OnClickEnemySave);
        _enemyLoadButton.onClick.AddListener(OnClickEnemyLoad);
    }

    private void OnClickCharacterSave()
    {
        var list = new List<TestCharacter>()
        {
            new TestCharacter(1, "chara1"),
            new TestCharacter(2, "chara2"),
            new TestCharacter(3, "chara3"),
        };

        var data = new TestCharacters(list);
        SaveUtils.Save(SaveConst.SaveName.Characters.ToString(), data);

        Debug.Log("キャラデータを保存しました");
        Debug.Log(data.GetLogString());
    }

    private void OnClickCharacterLoad()
    {
        var data = SaveUtils.Load<TestCharacters>(SaveConst.SaveName.Characters.ToString());

        Debug.Log("キャラデータをロードしました");
        Debug.Log(data.GetLogString());
    }

    private void OnClickEnemySave()
    {
        var list = new List<TestEnemy>()
        {
            new TestEnemy(10, "enemy1", "Normal"),
            new TestEnemy(20, "enemy2", "Elite"),
            new TestEnemy(99, "enemy3", "Boss"),
        };

        var data = new TestEnemys(list);
        SaveUtils.Save(SaveConst.SaveName.Enemys.ToString(), data);

        Debug.Log("敵データを保存しました");
        Debug.Log(data.GetLogString());
    }

    private void OnClickEnemyLoad()
    {
        var data = SaveUtils.Load<TestEnemys>(SaveConst.SaveName.Enemys.ToString());

        Debug.Log("敵データをロードしました");
        Debug.Log(data.GetLogString());
    }
}
