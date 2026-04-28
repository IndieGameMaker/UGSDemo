using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _signOutButton;
    [SerializeField] private Button _playerNameSaveButton;

    [SerializeField] private TMP_InputField _playerNameIf;
    
    [Header("UserName & Password")]
    [SerializeField] private TMP_InputField _userNameIf, _passwordIf;
    [SerializeField] private Button _signUpButton, _signInUsernameButton;

    private async void Awake()
    {
        // UGS 초기화 콜백
        UnityServices.Initialized += () => Debug.Log("UGS 초기화 완료");
        
        // UGS 초기화
        await UnityServices.InitializeAsync();
        
        // 이벤트 연결
        EventBinding();
        
        // 로그인 버튼 이벤트 연결
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
        
        // 로그아웃 버튼 이벤트 연결
        _signOutButton.onClick.AddListener(() =>
        {
            AuthenticationService.Instance.SignOut();
        });
        
        // 플레이어 이름 저장 버튼 이벤트 연결
        _playerNameSaveButton.onClick.AddListener(async () =>
        {
            await SetPlayerName(_playerNameIf.text);
        });

        // AuthenticationService.Instance.GetPlayerNameAsync();
        
        // 회원가입 이벤트 연결
        /*
         * UserName : 대소문자 구별없음, 3 ~ 20자, [. - @] 사용가능
         * Passwrod : 대소문자 구별, 8자 ~ 30자, 1 영문 대문자, 1 영문 소문자, 숫자 1, 특수문자 1
         * Ab1234!!
         */
        _signUpButton.onClick.AddListener(async () =>
        {
            try
            {
                // 회원가입 (SignUp...)
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(_userNameIf.text,
                    _passwordIf.text);
                
                Debug.Log("회원가입 성공");
            }
            catch (AuthenticationException e)
            {
                Debug.LogError(e.Message);
            }
            catch (RequestFailedException e)
            {
                Debug.LogError(e.Message);
            }
        });
    }

    private void OnEnable()
    {
        // 로그인 이벤트 연결
        _signInUsernameButton.onClick.AddListener(async () => await SignIn(_userNameIf.text, _passwordIf.text));
    }

    private void OnDisable()
    {
        _signInButton.onClick.RemoveAllListeners();
        _signOutButton.onClick.RemoveAllListeners();
        _playerNameSaveButton.onClick.RemoveAllListeners();
        _signUpButton.onClick.RemoveAllListeners();
        _signInUsernameButton.onClick.RemoveAllListeners();
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
    
    // 플레이어 이름 저장 
    private async Task SetPlayerName(string playerName)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            Debug.Log($"{playerName} 변경 완료");
            /* 사용자 이름
             * 50자 허용, 공백 불가, 4자리 해시값  Zack => Zack#1234
             */
            Debug.Log(AuthenticationService.Instance.PlayerName);
        }
        catch (AuthenticationException e)   
        {
            Debug.LogError(e.Message);
        }
    }
    
    // Username & Password 로그인 요청
    private async Task SignIn(string userName, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(userName, password);
        }
        catch (AuthenticationException e)
        {
            Debug.LogError(e.Message);
        }
        catch (RequestFailedException e)
        {
            Debug.LogError(e.Message);
        }
    }
}
