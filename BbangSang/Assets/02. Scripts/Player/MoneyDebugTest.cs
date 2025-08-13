using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//플레이어 돈 저장 유무 테스트용 스크립트

//씬에 오브젝트 생성 후 스크립트 붙이고 사용 가능

// 1번 : 돈 추가
// 2번 : 돈 차감
// 3번 : 저장  (각각의 스크립트에서 자동 저장 필요, 테스트 시에 수동 저장 해야함)
// 4번 : 불러오기
// 5번 : 현재 금액 표시
// 9번 : 저장 데이터 초기화

// 사용 방법
// 1번 돈 추가(2번 돈 차감) 후 3번 저장, 게임 정지 후 재시작, 4번 불러오기, 5번 현재 금액 표시



public class MoneyDebugTest : MonoBehaviour
{
    [SerializeField] private long addAmount = 500_000;
    [SerializeField] private long spendAmount = 80_000;

    [Header("New Game Reset (key9)")]
    [SerializeField] private string newGameName = "NewPlayer";

    private Player player;

    void Start()
    {
        player = GameManager.Instance?.Player;
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        if (player == null)
        {
            Debug.LogWarning("[MoneyDebugTester] Player를 찾지 못했습니다.");
        }
        else
        {
            Debug.Log($"[MoneyDebugTester] Ready. Current Money: {player.Money:N0}");
        }
    }

    void Update()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) // + 돈
        {
            player.AddMoney(addAmount);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // - 돈
        {
            if (!player.TrySpendMoney(spendAmount))
                Debug.Log($"[MONEY] Not enough money to spend {spendAmount:N0}. Current {player.Money:N0}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // 저장
        {
            player.Save();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) // 로드
        {
            player.Load();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) // 현재 금액 출력
        {
            Debug.Log($"[MONEY] Now: {player.Money:N0}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ResetToNewGameSave();
        }
    }

    private void ResetToNewGameSave()
    {
        if (player == null)
        {
            player = GameManager.Instance?.Player ?? FindObjectOfType<Player>();
            if (player == null)
            {
                Debug.LogError("[RESET] Player가 없어 초기화 불가");
                return;
            }
        }

        string nameToUse = string.IsNullOrWhiteSpace(newGameName)
            ? (player.playerData?.name ?? "Player")
            : newGameName.Trim();

        player.CreateDataWithName(nameToUse);
        player.Save();

        Debug.Log($"[RESET] Player 데이터 초기화 완료 (씬 변경 없음, 재시작 필요)\n" +
                  $"- Name: {player.playerData.name}\n" +
                  $"- Money: {player.playerData.money:N0}\n" +
                  $"- CraftingTableLevel: {player.playerData.craftingTableLevel}");
    }
}

