using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private Button _signInButton;

    private async void Awake()
    {
        // UGS 초기화 콜백
        UnityServices.Initialized += () => Debug.Log("UGS 초기화 완료");
        
        // UGS 초기화
        await UnityServices.InitializeAsync();
        
        // 버튼 이벤트 연결
        _signInButton.onClick.AddListener(async () => 
        {
            try
            {
                // 익명 로그인 요청
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("익명 사용자 로그인 성공");
            }
            catch (AuthenticationException e)
            {
                Debug.LogError(e.Message);
            }
        });
    }
}
