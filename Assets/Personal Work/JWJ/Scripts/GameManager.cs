using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : Singleton<GameManager>
{
    public int CurStage; //{ get; private set; }
    // public int Coin;
    private float _spawnDelayTime = 2;

    private GameObject _echo;
    private GameObject _player;
    //private GameObject _curBox;
    public Transform PlayerTransform;
    private Vector2 _spawnPoint = Vector2.zero;

    private GameObject _boxPrefab;
    private readonly List<Vector2> _boxPosList = new();
    private readonly List<BoxInteraction> _boxes = new();

    private GameObject _fireHPrefab;
    private GameObject _fireVPrefab;
    private Vector2 _fireHPos = Vector2.zero;
    private Vector2 _fireVPos = Vector2.zero;
    private readonly List<BoxInteraction> _firesH = new();
    private readonly List<BoxInteraction> _firesV = new();

    private List<EchoController> _echos = new List<EchoController>();
    private PlayerController _playerCon;

    public event Action OnPlayerDied;
    public event Action OnPlayerStart;
    public event Action OnTransitioning;

    private bool _isBox = false;
    private int _echoID = 0;

    //재영 추가
    public event Action<int> OnCoinCountChanged; 

    private int _coin;
    public int Coin 
    {
        get => _coin;
        set
        {
            _coin = value;
            OnCoinCountChanged?.Invoke(_coin); 
        }
    }
    protected override void Awake()
    {
        base.Awake();
        var handle = Addressables.LoadAssetAsync<GameObject>("Echo");
        handle.Completed += OnEchoLoaded;

        var handle_player = Addressables.LoadAssetAsync<GameObject>("Player");
        handle_player.Completed += OnPlayerLoaded;

        var handle_box = Addressables.LoadAssetAsync<GameObject>("Box");
        handle_box.Completed += OnBoxLoaded;

        var handle_fireH = Addressables.LoadAssetAsync<GameObject>("FireHo");
        handle_fireH.Completed += OnFireHLoaded;

        var handle_fireV = Addressables.LoadAssetAsync<GameObject>("FireVer");
        handle_fireV.Completed += OnFireVLoaded;

        CurStage = 0;
    }


    //어드레서블에서 잔상 로드하는 함수
    private void OnEchoLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _echo = handle.Result;
            Debug.Log($"Echo 로드 완료");
        }
        else
        {
            Debug.LogError($"Echo 로드 실패: {handle.OperationException}");
        }
    }
    private void OnPlayerLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _player = handle.Result;
            Debug.Log($"Player 로드 완료");
        }
        else
        {
            Debug.LogError($"Player 로드 실패: {handle.OperationException}");
        }
    }
    private void OnBoxLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _boxPrefab = handle.Result;
            Debug.Log($"Box 로드 완료");
        }
        else
        {
            Debug.LogError($"Box 로드 실패: {handle.OperationException}");
        }
    }
    private void OnFireHLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _fireHPrefab = handle.Result;
            Debug.Log($"FireH 로드 완료");
        }
        else
        {
            Debug.LogError($"FireH 로드 실패: {handle.OperationException}");
        }
    }
    private void OnFireVLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _fireVPrefab = handle.Result;
            Debug.Log($"FireV 로드 완료");
        }
        else
        {
            Debug.LogError($"FireV 로드 실패: {handle.OperationException}");
        }
    }

    //플레이어 사망시
    public void PlayerDieAndSave(List<InputRecord> records, GameObject player, int echoID, float recordStartTime)
    {
        player.SetActive(false);
        //Debug.Log("플레이어 사망");
        OnPlayerDied?.Invoke(); //플레이어 죽음 이벤트 발생

        StartCoroutine(RespawnRotine(records, player, echoID, recordStartTime));
    }

    private IEnumerator RespawnRotine(List<InputRecord> records, GameObject player, int id, float startTime)
    {
        _echoID = id;
        yield return new WaitForSeconds(_spawnDelayTime);

        foreach (var ech in _echos)
        {
            ech.ResetToSpawn(_spawnPoint);
        }

        var echo = Instantiate(_echo, _spawnPoint, Quaternion.identity);
        var controller = echo.GetComponent<EchoController>();
        controller.Init(records, _echoID, startTime);
        _echos.Add(controller);

        player.transform.position = _spawnPoint;
        player.transform.rotation = Quaternion.identity;
        player.SetActive(true);

        if (_isBox)
        {
            if (CurStage == 4 || CurStage == 8)
            {
                RepositionFires();
                SpawnFire(false);
                yield break;
            }
            RepositionBoxs();
            SpawnBoxes(false);

        }
        //Debug.Log("플레이어, 에코 프리팹 소환");
    }

    public void StartPlayer()
    {
        OnPlayerStart?.Invoke(); //플레이어 시작 이벤트 발생
    }

    public void LevelUp()
    {
        CurStage++;
        _echos.Clear();
    }
    public void InitLevel()
    {
        CurStage = 0;
        _echos.Clear();
    }

    public void SetRespawnPoint(Vector2 pos, Stage stage)
    {
        _echos.Clear();
        _spawnPoint = pos;
        _boxPosList.Clear();
        _isBox = stage.IsBox;

        if (_isBox && stage.BoxTransform != null)
        {
            if (CurStage == 7)
            {
                _fireHPos = new Vector2(stage.BoxTransform[0].transform.position.x, stage.BoxTransform[0].transform.position.y);
                _fireVPos = new Vector2(stage.BoxTransform[1].transform.position.x, stage.BoxTransform[1].transform.position.y);

                RepositionFires();
                SpawnFire(true);
                return;
            }
            foreach (var transform in stage.BoxTransform)
            {
                if (transform)
                {
                    _boxPosList.Add((Vector2)transform.position);
                }
            }
            RepositionBoxs();
            SpawnBoxes(true);
        }
    }
    public void SpawnPlayer()
    {
        var player = Instantiate(_player, _spawnPoint, Quaternion.identity);
        PlayerTransform = player.transform;
        _playerCon = player.GetComponent<PlayerController>();
        // audio 재영 추가
        AudioManager.Instance.PlaySFX(AudioManager.Instance.spawnSFX);
    }

    public void KillPlayer()
    {
        _playerCon.DiePlayer();
        // audio 재영 추가
        AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSFX);
    }

    private void SpawnBoxes(bool isForPlayer)
    {
        for (int i = 0; i < _boxPosList.Count; i++)
        {
            var go = Instantiate(_boxPrefab, _boxPosList[i], Quaternion.identity);
            var box = go.GetComponent<BoxInteraction>();
            box.SetBox(isForPlayer, _echoID, _boxPosList[i]);
            _boxes.Add(box);
        }
        BoxInteraction.InitCollisionAll();
    }

    private void RepositionBoxs()
    {
        for (int i = 0; i < _boxes.Count; i++)
        {
            var box = _boxes[i];
            box.transform.position = box.SpawnPos;
        }
    }

    public void StageClear()
    {
        OnTransitioning?.Invoke();
    }
    private void SpawnFire(bool isForPlayer)
    {
        var ver = Instantiate(_fireVPrefab, _fireVPos, Quaternion.Euler(0, 0, 90));
        var ho = Instantiate(_fireHPrefab, _fireHPos, Quaternion.identity);

        var fireV = ver.GetComponent<BoxInteraction>();
        var fireH = ho.GetComponent<BoxInteraction>();

        fireV.SetBox(isForPlayer, _echoID, _fireVPos);
        fireH.SetBox(isForPlayer, _echoID, _fireHPos);

        _firesV.Add(fireV);
        _firesH.Add(fireH);

        BoxInteraction.InitCollisionAll();
    }
    private void RepositionFires()
    {
        for (int i = 0; i < _firesH.Count; i++)
        {
            var fireH = _firesH[i];
            fireH.transform.position = fireH.SpawnPos;
            fireH.gameObject.GetComponent<WoodStick>().ResetFire();
        }
        for (int i = 0; i < _firesV.Count; i++)
        {
            var fireV = _firesV[i];
            fireV.transform.position = fireV.SpawnPos;
            fireV.gameObject.GetComponent<WoodStick>().ResetFire();
        }
    }


}
