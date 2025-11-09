using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int CurStage { get; private set; }
    [SerializeField] private List<Stage> _stages;
    public Vector2 SpawnPos;
    [SerializeField] private CinemachineCamera cinemachine;
    [SerializeField] TMP_Text stageTitle;

    public List<Stage> Stages => _stages;
    private void Awake()
    {
        int index = Manager.Game.CurStage - 1;
        _stages[index].gameObject.SetActive(true);
        //Debug.Log($"지금 스테이지 {index}");
        //Debug.Log($"x {_stages[index].respawnPoint.position.x}");
        //Debug.Log($"y {_stages[index].respawnPoint.position.y}");
        SpawnPos = new Vector2(_stages[index].respawnPoint.position.x, _stages[index].respawnPoint.position.y);
        stageTitle.text = _stages[index].stageName;
        ShowStageTitle();
        Manager.Game.SetRespawnPoint(SpawnPos, _stages[index]);
        Manager.Game.SpawnPlayer();
        var camTarget = cinemachine.Target;
        camTarget.TrackingTarget = Manager.Game.PlayerTransform;
        Debug.Log(Manager.Game.PlayerTransform.gameObject.name);
        //camTarget.CustomLookAtTarget = false;   // (필요하면) TrackingTarget으로 회전까지 하게
        cinemachine.Target = camTarget;
    }

    private void ShowStageTitle()
    {
        stageTitle.alpha = 1f;
        stageTitle.DOFade(0f, 2.5f) // 점점 투명해짐
            .SetEase(Ease.InOutQuad);
    }
}
