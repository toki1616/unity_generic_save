using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace My.Save
{
    public class CharacterListView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _scrollRectTransform;

        [SerializeField]
        private LayoutGroup _layoutGroup;

        [SerializeField]
        private RectTransform _parentTransform;

        [SerializeField]
        private GameObject _createPrefab;

        [SerializeField]
        private Button _createButton;

        [SerializeField]
        private Button _deleteButton;

        [SerializeField]
        private Button _saveButton;

        //生成したプレハブのスクリプトを保持
        private List<CharacterView> _items = new List<CharacterView>();

        void Start()
        {
            _createButton.onClick.AddListener(OnClickCreate);
            _deleteButton.onClick.AddListener(OnClickDelete);
            _saveButton.onClick.AddListener(OnClickSave);

            //起動時にロードして生成
            LoadAndCreateItems();
        }

        private void LoadAndCreateItems()
        {
            //保存データ読み込み
            TestCharacters data = SaveUtils.Load<TestCharacters>("Characters");

            Debug.Log("Loaded: " + data.GetLogString());

            //読み込んだキャラをUIに生成
            foreach (var c in data.CharacterList)
            {
                CreateItemFromData(c);
            }
        }

        private void OnClickCreate()
        {
            CreateItem();
        }

        private void OnClickDelete()
        {
            DeleteLastItem();
        }

        private void OnClickSave()
        {
            Debug.Log("Save");

            //UIからTestCharacterを作り直す
            List<TestCharacter> list = new List<TestCharacter>();

            foreach (var item in _items)
            {
                list.Add(item.GetCharacter());
            }

            //JSON保存
            TestCharacters saveData = new TestCharacters(list);
            SaveUtils.Save("Characters", saveData);

            Debug.Log("Saved: " + saveData.GetLogString());
        }

        public void CreateItem()
        {
            if (_createPrefab == null || _parentTransform == null)
                return;

            //プレハブ生成
            GameObject obj = Instantiate(_createPrefab, _parentTransform);

            //必要なら初期化処理を書く
            //obj.GetComponent<YourComponent>().Init(...);

            //スクリプトを取得してリストに追加
            CharacterView item = obj.GetComponent<CharacterView>();
            if (item != null)
            {
                //デフォルト値
                item.SetCharacter(new TestCharacter(0, "New Character"));

                _items.Add(item);
            }

            //レイアウト更新 → 自動スクロール
            RefreshLayout();
        }

        //読み込んだデータから生成する専用メソッド
        private void CreateItemFromData(TestCharacter character)
        {
            GameObject obj = Instantiate(_createPrefab, _parentTransform);

            CharacterView item = obj.GetComponent<CharacterView>();
            if (item != null)
            {
                item.SetCharacter(character);
                _items.Add(item);
            }

            RefreshLayout();
        }

        /// <summary>
        /// 最後の生成アイテムを削除
        /// </summary>
        private void DeleteLastItem()
        {
            if (_items.Count == 0)
                return;

            CharacterView last = _items[_items.Count - 1];

            Destroy(last.gameObject);

            _items.RemoveAt(_items.Count - 1);

            RefreshLayout();
        }

        /// <summary>
        /// スクロールビューの高さ調整
        /// </summary>
        private void RefreshLayout()
        {
            // レイアウトを即時再構築
            Vector2 offsetMin = _scrollRectTransform.offsetMin;
            _scrollRectTransform.offsetMin = offsetMin;

            _layoutGroup.CalculateLayoutInputHorizontal();
            _layoutGroup.CalculateLayoutInputVertical();
            _layoutGroup.SetLayoutHorizontal();
            _layoutGroup.SetLayoutVertical();

            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRectTransform);

            // レイアウト更新後にスクロール位置を調整
            ScrollToBottomNextFrame();
        }

        /// <summary>
        /// 次の Canvas 更新タイミングでスクロールを最下部へ移動
        /// </summary>
        private void ScrollToBottomNextFrame()
        {
            Canvas.willRenderCanvases += OnWillRender;
        }

        /// <summary>
        /// Canvas の描画直前に呼ばれる
        /// </summary>
        private void OnWillRender()
        {
            Canvas.willRenderCanvases -= OnWillRender;

            ScrollToBottom();
        }

        /// <summary>
        /// スクロールを一番下まで移動
        /// </summary>
        private void ScrollToBottom()
        {
            ScrollRect scrollRect = _scrollRectTransform.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases(); // レイアウト更新を強制
                scrollRect.verticalNormalizedPosition = 0f; // 一番下へ
            }
        }
    }
}
