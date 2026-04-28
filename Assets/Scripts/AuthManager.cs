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
        
        // 이벤트 연결
        EventBinding();
        
        // 버튼 이벤트 연결
        _signInButton.onClick.AddListener(async () => 
        {
            try
            {
                // 익명 로그인 요청
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                
            }
            catch (AuthenticationException e)
            {
                Debug.LogError(e.Message);
            }
        });
    }
    
    // 인증 이벤트 연결
    private void EventBinding()
    {
        // 로그인
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"익명 사용자 로그인 성공 {AuthenticationService.Instance.PlayerId}");
        };
        // 로그 아웃
        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("로그 아웃");
        };
        
        // 세션 아웃
        AuthenticationService.Instance.Expired += () => Debug.Log("세션 타임 아웃");
    }
}
