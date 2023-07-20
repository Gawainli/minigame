using Cysharp.Threading.Tasks;
using MiniGame.Asset;
using MiniGame.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.UI
{
    public abstract class UIWindow
    {
        private System.Object[] _userDatas;
        private GameObject _uiPanel;
        private Canvas _uiCanvas;
        private Canvas[] _uiChildCanvas;
        private bool _isCreate = false;
        private GraphicRaycaster _uiGraphicRaycaster;
        private GraphicRaycaster[] _uiChildGraphicRaycaster;
        
        public Transform Transform => _uiPanel.transform;
        public GameObject UIPanel => _uiPanel;
        public System.Object[] UserDatas => _userDatas;
        
        public string WindowName { get; private set; }
        public int WindowLayer { get; private set; }
        public bool Prepared { get; private set; }
        public bool FullScreen { get; private set; }
        
        
        public int Depth
        {
            get => _uiCanvas != null ? _uiCanvas.sortingOrder : -999;
            set
            {
                if (_uiCanvas != null)
                {
                    if (_uiCanvas.sortingOrder == value)
                    {
                        return;
                    }
                    _uiCanvas.sortingOrder = value;
                    int depth = value;
                    foreach (var child in _uiChildCanvas)
                    {
                        if (child != null && child != _uiCanvas)
                        {
                            depth++;
                            child.sortingOrder = depth;
                        }
                    }
                }

                if (_isCreate)
                {
                    OnSortDepth(value);
                }
            }
        }
        
        public bool Visible
        {
            get => _uiPanel != null && _uiPanel.activeSelf;
            set
            {
                if (_uiPanel != null && _uiPanel.activeSelf != value)
                {
                    _uiPanel.SetActive(value);

                    if (_isCreate)
                    {
                        OnSetVisible(value);
                    }
                }
            }
        }
        
        public bool Interactable
        {
            get => _uiGraphicRaycaster && _uiGraphicRaycaster.enabled;
            set
            {
                if (_uiGraphicRaycaster && _uiGraphicRaycaster.enabled != value)
                {
                    _uiGraphicRaycaster.enabled = value;
                    foreach (var rc in _uiChildGraphicRaycaster)
                    {
                        rc.enabled = value;
                    }
                }
            }
        }
        
        public void Init(string windowName, int windowLayer, bool fullScreen)
        {
            WindowName = windowName;
            WindowLayer = windowLayer;
            FullScreen = fullScreen;
        }
        
        /// <summary>
        /// 窗口创建
        /// </summary>
        public abstract void OnCreate();

        /// <summary>
        /// 窗口刷新
        /// </summary>
        public abstract void OnRefresh();

        /// <summary>
        /// 窗口更新
        /// </summary>
        public abstract void OnUpdate(float deltaTime);

        /// <summary>
        /// 窗口销毁
        /// </summary>
        public abstract void OnDestroy();

        /// <summary>
        /// 当触发窗口的层级排序
        /// </summary>
        protected virtual void OnSortDepth(int depth) { }

        /// <summary>
        /// 当因为全屏遮挡触发窗口的显隐
        /// </summary>
        protected virtual void OnSetVisible(bool visible) { }

        public async UniTask LoadAsync(string assetPath, System.Object[] userDatas)
        {
            _userDatas = userDatas;
            var panelPrefab = await AssetModule.LoadAssetAsync<GameObject>(assetPath);
            if (panelPrefab == null)
            {
                LogModule.Error("UIWindow Load Error: panelPrefab is null. path: " + assetPath);
                return;
            }
            InstantiatePanel(panelPrefab);
        }
        
        public void LoadSync(string assetPath, System.Object[] userDatas)
        {
            _userDatas = userDatas;
            var panelPrefab = AssetModule.LoadAssetSync<GameObject>(assetPath);
            if (panelPrefab == null)
            {
                LogModule.Error("UIWindow Load Error: panelPrefab is null. path: " + assetPath);
                return;
            }
            InstantiatePanel(panelPrefab);
        }

        private void InstantiatePanel(GameObject panelPrefab)
        {
            _uiPanel = Object.Instantiate(panelPrefab);
            _uiPanel.transform.localPosition = Vector3.zero;
            
            _uiCanvas = _uiPanel.GetComponent<Canvas>();
            if (_uiCanvas == null)
            {
                LogModule.Error("UIWindow Load Error: Canvas is null. name: " + panelPrefab.name);
            }
            _uiCanvas.overrideSorting = true;
            _uiCanvas.sortingOrder = 0;
            _uiCanvas.sortingLayerName = "UI";
            
            _uiGraphicRaycaster = _uiPanel.GetComponent<GraphicRaycaster>();
            _uiChildCanvas = _uiPanel.GetComponentsInChildren<Canvas>();
            _uiChildGraphicRaycaster = _uiPanel.GetComponentsInChildren<GraphicRaycaster>(); 
            Prepared = true;
        }
        
        
        internal void Create()
        {
            if (_isCreate)
            {
                return;
            }
            OnCreate();
        }
        
        internal void Refresh(params System.Object[] userDatas)
        {
            if (!_isCreate)
            {
                return;
            }
            _userDatas = userDatas;
            OnRefresh();
        }
        
        internal void Update(float deltaTime)
        {
            if (!_isCreate)
            {
                return;
            }
            OnUpdate(deltaTime);
        }
        
        internal void Destroy()
        {
            if (!_isCreate)
            {
                return;
            }

            if (_uiPanel != null)
            {
                OnDestroy();
                Object.Destroy(_uiPanel);
                _uiPanel = null;
            }
        }
    }
}