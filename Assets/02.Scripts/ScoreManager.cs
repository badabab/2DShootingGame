using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

// 역할: 점수를 관리하는 점수 관리자
public class ScoreManager : MonoBehaviour
{
    // 목표: 적을 잡을 때마다 점수를 올리고, 현재 점수를 UI에 표시하고 싶다.
    // 필요 속성
    // - 현재 점수를 표시할 UI
    public Text ScoreTextUI;
    // - 현재 점수를 기억할 변수
    private int _score = 0;
    public int Score
    {
        get { return _score; }
        set 
        {
            if (value < 0)
            {
                return;
            }
            _score = value;

            // 목표: 스코어를 화면에 표시한다.
            ScoreTextUI.text = $"점수 : {Score}";
            if (Score > BestScore)
            {
                BestScore = Score;
                PlayerPrefs.SetInt("BestScore", BestScore);
                BestScoreTextUI.text = $"최고 점수 : {BestScore}";
            }
        }
    }

    // 최고 점수 관련 속성
    public Text BestScoreTextUI;
    public int BestScore = 0;


    // ScoreManager가 점수를 관리하는 유일한 매니저(관리자)이므로 싱글톤을 적용하는 게 편하다.
    public static ScoreManager instance { get; private set; } // ScoreManager 객체

    private void Awake()
    {
        Debug.Log("ScoreManager 객체의 Awake 호출!");
        // 싱글톤 패턴: 오직 한 개의 클래스 인스턴스를 갖도록 보장하고,
        //              전역적인 접근점을 제공한다. (아무데서나 쉽게 이 하나의 객체에 접근 가능)
        // 사용 이유:
        // 개임 개발에서 매니저 / 관리자 클래스에 싱글톤 패턴을 사용하는 것은 일반적인 관행
        // - 전역 접근, 코드 단순화 (해당 관리자를 찾기 위한 복잡한 로직이 필요 없다.)
        // - 중복 생성 방지(메모리 및 리소스 관리 능력 Up)

        if (instance == null)
        {
            Debug.Log("생성된 것");
            instance = this;
        }
        else
        {
            Debug.Log("이미 있는 것");
            Destroy(gameObject);
        }
    }


    // 목표: 게임을 시작할 때 최고 점수를 불러오고, UI에 표시하고 싶다.
    // 구현 순서:
    // 1. 게임을 시작할 때
    private void Start()
    {
        // 2. 최고 점수를 불러온다.
        BestScore = PlayerPrefs.GetInt("BestScore", 0); // default value = 0
        // 3. UI에 표시한다.
        BestScoreTextUI.text = $"최고 점수 : {BestScore}";
    }

    /*
    // 목표: _score 속성에 대한 캡슐화 (get/set)
    public int GetScore()
    {
        return _score;
    }

    public void AddScore()
    {
        SetScore(_score + 1);
    }

    public void SetScore(int score)
    {
        // 유효성 검사
        if (score < 0)
        {
            return;
        }
        _score = score;

        // 목표: 스코어를 화면에 표시한다.
        ScoreTextUI.text = $"점수 : {_score}";

        // 목표: 최고 점수를 갱신하고 UI에 표시하고 싶다.
        // 1. 만약에 현재 점수가 최고 점수보다 크다면
        if (_score > BestScore)
        {
            // 2. 최고 점수를 갱신하고
            BestScore = _score;

            // 목표: 최고 점수를 저장
            // 'PlayerPrefs' 클래스를 사용
            // -> 값을 '키(Key)'와 '값(Value)' 형태로 저장하는 클래스입니다.
            // 저장할 수 있는 데이터타입: int, float, string
            // 타입별로 저장/로드가 가능한 Set/Get 메서드가 있다.
            PlayerPrefs.SetInt("BestScore", BestScore);

            // 3. UI에 표시한다.
            BestScoreTextUI.text = $"최고 점수 : {BestScore}";
        }
    }
    */
}
