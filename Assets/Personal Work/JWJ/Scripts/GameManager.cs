using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : Singleton<GameManager>
{
    public int CurStage {  get; private set; }

    private float _spawnDelayTime = 2;

    private GameObject _echo;
    private GameObject _player;
    private Vector2 _spawnPoint = Vector2.zero;

    private List<EchoController> _echos = new List<EchoController>();
    private PlayerController _playerCon;

    public event Action OnPlayerDied;
    public event Action OnPlayerStart;
    protected override void Awake()
    {
        base.Awake();
        var handle = Addressables.LoadAssetAsync<GameObject>("Echo");
        handle.Completed += OnEchoLoaded;
        var handle_player = Addressables.LoadAssetAsync<GameObject>("Player");
        handle_player.Completed += OnPlayerLoaded;
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
        yield return new WaitForSeconds(_spawnDelayTime);

        foreach (var ech in _echos)
        {
            ech.ResetToSpawn(_spawnPoint);
        }

        var echo = Instantiate(_echo, _spawnPoint, Quaternion.identity);
        var controller = echo.GetComponent<EchoController>();
        controller.Init(records, id, startTime);
        _echos.Add(controller);

        player.transform.position = _spawnPoint;
        player.transform.rotation = Quaternion.identity;
        player.SetActive(true);
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

    public void SetRespawnPoint(Vector2 pos)
    {
        _spawnPoint = pos;
    }
    public void SpawnPlayer()
    {
        var player = Instantiate(_player, _spawnPoint, Quaternion.identity);
        _playerCon = player.GetComponent<PlayerController>();
    }

    public void KillPlayer()
    {
        _playerCon.DiePlayer();
    }
}
