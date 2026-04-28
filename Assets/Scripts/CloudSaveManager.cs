using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private Button cloudSaveButton;
    [SerializeField] private Button cloudLoadButton;

    private void OnEnable()
    {
        cloudSaveButton.onClick.AddListener(async () => await SaveSingleData());
        cloudLoadButton.onClick.AddListener(async () => await LoadSingleData());
    }
    
    // 싱글 데이터 저장
    private async Task SaveSingleData()
    {
        // 저장 데이터 (Json)
        var data = new Dictionary<string, object>
        {
            {"player_name", "zack"},
            {"level", 10},
            {"xp", 300},
            {"gold", 100}
        };

        // 저장 메서드 호출
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        // await CloudSaveService.Instance.Files.Player.SaveAsync(file);

        Debug.Log("단일 데이터 저장 완료");
    }
    
    // 싱글 데이터 로딩
    private async Task LoadSingleData()
    {
        var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{"player_name", "level", "xp", "gold"});
        if (data.TryGetValue("player_name", out var playerName))
        {
            Debug.Log("PlayerName: " + playerName.Value.GetAs<string>());
        }

        if (data.TryGetValue("level", out var level))
        {
            Debug.Log("Level: " + level.Value.GetAs<int>());
        }
        
        if (data.TryGetValue("xp", out var xp))
        {
            Debug.Log("XP: " + xp.Value.GetAs<int>());
        }

        if (data.TryGetValue("gold", out var gold))
        {
            Debug.Log("Gold: " + gold.Value.GetAs<int>());
        }
            
    }
}
