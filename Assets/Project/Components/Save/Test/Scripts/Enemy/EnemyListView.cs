using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace My.Save
{
    public class EnemyListView : MonoBehaviour
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

        [SerializeField]
        private Button _loadButton;

        //生成したプレハブのスクリプトを保持
        private List<EnemyView> _items = new List<EnemyView>();

        void Start()
        {
            _createButton.onClick.AddListener(OnClickCreate);
            _deleteButton.onClick.AddListener(OnClickDelete);
            _saveButton.onClick.AddListener(OnClickSave);
            _loadButton.onClick.AddListener(OnClickLoad);

            //起動時にロードして生成
            LoadAndCreateItems();
        }

        private void LoadAndCreateItems()
        {
            //保存データ読み込み
            TestEnemys data = SaveUtils.Load<TestEnemys>("Enemys");

            Debug.Log("Loaded: " + data.GetLogString());

            //読み込んだ敵データをUIに生成
            foreach (var e in data.EnemyList)
            {
                CreateItemFromData(e);
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
            //UIからTestEnemyを作り直す
            List<TestEnemy> list = new List<TestEnemy>();

            foreach (var item in _items)
            {
                list.Add(item.GetEnemy());
            }

            //JSON保存
            TestEnemys saveData = new TestEnemys(list);
            SaveUtils.Save("Enemys", saveData);

            Debug.Log("Saved: " + saveData.GetLogString());
        }

        private void OnClickLoad()
        {
            // 既存の UI を削除
            foreach (var item in _items)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            _items.Clear();

            // 再ロード
            LoadAndCreateItems();
        }

        public void CreateItem()
        {
            if (_createPrefab == null || _parentTransform == null)
                return;

            //プレハブ生成
            GameObject obj = Instantiate(_createPrefab, _parentTransform);

            //スクリプトを取得してリストに追加
            EnemyView item = obj.GetComponent<EnemyView>();
            if (item != null)
            {
                //デフォルト値
                item.SetEnemy(new TestEnemy(0, "New Enemy", "Normal"));

                _items.Add(item);
            }

            //レイアウト更新 → 自動スクロール
            RefreshLayout();
        }

        //読み込んだデータから生成する専用メソッド
        private void CreateItemFromData(TestEnemy enemy)
        {
            GameObject obj = Instantiate(_createPrefab, _parentTransform);

            EnemyView item = obj.GetComponent<EnemyView>();
            if (item != null)
            {
                item.SetEnemy(enemy);
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

            EnemyView last = _items[_items.Count - 1];

            Destroy(last.gameObject);

            _items.RemoveAt(_items.Count - 1);

            RefreshLayout();
        }

        /// <summary>
        /// スクロールビューの高さ調整
        /// </summary>
        private void RefreshLayout()
        {
            Vector2 offsetMin = _scrollRectTransform.offsetMin;
            _scrollRectTransform.offsetMin = offsetMin;

            _layoutGroup.CalculateLayoutInputHorizontal();
            _layoutGroup.CalculateLayoutInputVertical();
            _layoutGroup.SetLayoutHorizontal();
            _layoutGroup.SetLayoutVertical();

            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRectTransform);

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
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }
}
