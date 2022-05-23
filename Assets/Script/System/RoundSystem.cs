using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 디버그를 위해 의도적으로 DisallowMultipleComponent는 넣지 않는다
// 여러 디버그용 Round를 제작해야할 수도 있기 때문
public class RoundSystem : Singleton<RoundSystem>
{
    private static readonly float SPAWN_CHECK_TIME = 0.1f;
    
    public List<Round> rounds = new List<Round>();
    private int currentRoundIndex = -1;
    public Round CurrentRound
    {
        get;
        private set;
    }

    private int remainEnemyCount;

    [Header("UI")]
    public GameObject GameClearUI;

    public void Start()
    {
        // 원래는 게임 시작 시 무기를 선택한 후, 라운드를 시작해야 함
        // 일단 간이 테스트로 시작하자마자 라운드 진행
        NextRound();
    }

    public void Update()
    {
        DebugExecute();
    }

    public void CheckRoundEnd()
    {
        // 몹이 음수 개만큼 존재해선 안 된다
        //Debug.Assert(remainEnemyCount >= 0 && current >= 0);

        remainEnemyCount--;
        Debug.Log("current enemy Count : " + remainEnemyCount);
        if (remainEnemyCount == 0 && CurrentRound.enemySpawns.Count == 0)
        {
            // 게임 끝났나 체크
            if (currentRoundIndex == rounds.Count)
            {
                // 게임 끝났으면 게임 완료 처리
                GameClearUI.SetActive(true);
            }
            // 아니면 업그레이드 띄우기
            else
            {
                // 라운드 끝 애니메이션
                // 업그레이드 할 수 있다 띄우고 윈도우 띄우기
                WindowSystem.Instance.OpenWindow(UpgradeWindow.Instance.gameObject, false);
                UpgradeWindow.Instance.SetUpgradeWindow(EnhanceSystem.Instance.GetRandomEnhances(3));
            }
        }
    }

    // 다음 라운드 시작
    public void NextRound()
    {
        // 라운드 시작 애니메이션


        currentRoundIndex++;
        // 리스트에서 라운드 정보를 읽고 시작한다
        CurrentRound = rounds[currentRoundIndex];

        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        var enemySpawns = CurrentRound.enemySpawns;
        // 처음 시작할 때 spawn Wait Time을 시작 시간으로 맞춘다
        foreach (var spawn in enemySpawns)
        {
            spawn.spawnWaitTime = spawn.startTime;
            spawn.remainCount = spawn.count;
        }

        // 
        while (CurrentRound.enemySpawns.Count > 0)
        {
            // 남은 몬스터가 없는 라운드는 삭제해야 한다
            // 정방향 순회는 Remove를 사용했을 때 오류가 생길 가능성이 있으므로 역방향 순회를 사용한다
            for (int i = CurrentRound.enemySpawns.Count - 1; i >= 0; i--)
            {
                var currEnemySpawn = enemySpawns[i];
                if (currEnemySpawn.spawnWaitTime <= 0)
                {
                    // 순차적으로 스폰
                    if (currEnemySpawn.spawnType == Round.EnemySpawn.SpawnType.Sequence)
                    {
                        // 스폰 코드
                        currEnemySpawn.spawnWaitTime = 
                            currEnemySpawn.periodScaleCurve.Evaluate((float)currEnemySpawn.remainCount / currEnemySpawn.count) * 
                            currEnemySpawn.period;
                    }
                    // 한 번에 스폰
                    else if (currEnemySpawn.spawnType == Round.EnemySpawn.SpawnType.Emission)
                    {
                        // for문 스폰 코드
                        currEnemySpawn.count = 0;
                    }
                }

                if (currEnemySpawn.remainCount == 0)
                {
                    enemySpawns.RemoveAt(i);
                }
                else
                {
                    currEnemySpawn.spawnWaitTime -= SPAWN_CHECK_TIME;
                }
            }

            // 몬스터를 처치하면 스폰 간격 좀 줄인다 - 보류
            yield return new WaitForSeconds(SPAWN_CHECK_TIME);
        }

        Debug.Log("create end");
    }

    //private void Spawn()
    //{
    //    // 중간 중간 설정한 몬스터가 나와야 한다
    //    Debug.Log("Monster Spawn!!!");
    //    int randNum = UnityEngine.Random.Range(0, enemyPrefabs.Length);
    //    var go = Instantiate(enemyPrefabs[randNum], spawnTr.position, Quaternion.identity);
    //    go.GetComponent<EnemyStatus>().PowerUp(currentEnemyPowerup);
    //    EnemyManager.Instance.SetEnemyTarget(go);
    //    currentEnemyCount++;
    //}

    #region Debug

    private void DebugExecute()
    {
        // Round Skip
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
        }
    }

    #endregion
}
