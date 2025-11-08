using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int CurStage {  get; private set; }
    [SerializeField] private List<Stage> _stages;
    public Vector2 SpawnPos;

    private void Awake()
    {
        int index = Manager.Game.CurStage;
        _stages[index].gameObject.SetActive(true);
        Debug.Log($"지금 스테이지 {index}");
        Debug.Log($"x {_stages[index].respawnPoint.position.x}");
        Debug.Log($"y {_stages[index].respawnPoint.position.y}");
        SpawnPos = new Vector2(_stages[index].respawnPoint.position.x, _stages[index].respawnPoint.position.y);
        
        Manager.Game.SetRespawnPoint(SpawnPos);
        Manager.Game.SpawnPlayer();
    }
}
