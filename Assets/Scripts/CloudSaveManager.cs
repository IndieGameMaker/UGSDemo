using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private Button cloudSaveButton;

    private void OnEnable()
    {
        cloudSaveButton.onClick.AddListener(async () => await SaveSingleData());
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

        Debug.Log("단일 데이터 저장 완료");
    }
}
