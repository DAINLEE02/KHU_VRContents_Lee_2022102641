using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Actor_Audio : MonoBehaviour
{
    // VideoPlayer 대신 AudioSource를 사용합니다.
    public AudioSource Player;

    // 오디오는 렌더러가 없으므로, 설정값이나 상태를 저장하는 용도로 변수를 구성할 수 있습니다.
    float defaultVolume;
    bool hasPlayer = false;

    void Start()
    {
        // AudioSource가 할당되어 있는지 확인합니다.
        if (Player != null)
        {
            hasPlayer = true;
            defaultVolume = Player.volume;
            Debug.Log("AudioSource detected.");
        }
        else
        {
            // 컴포넌트가 직접 할당되지 않았을 경우 오브젝트에서 찾아봅니다.
            Player = GetComponent<AudioSource>();
            if (Player != null)
            {
                hasPlayer = true;
                defaultVolume = Player.volume;
            }
            else
            {
                Debug.LogWarning("NO AudioSource found!");
            }
        }

        // 초기 상태는 정지
        Stop();
    }

    public void Play()
    {
        if (hasPlayer)
        {
            // 필요한 경우 재생 시 볼륨을 기본값으로 복구하는 등의 처리가 가능합니다.
            Player.volume = defaultVolume;
            Player.Play();
        }
    }

    public void Stop()
    {
        if (hasPlayer)
        {
            Player.Stop();
        }
    }

    public void Pause()
    {
        if (hasPlayer)
        {
            Player.Pause();
        }
    }

    /// <summary>
    /// 현재 재생 중이면 정지하고, 정지 중이면 재생합니다.
    /// </summary>
    public void PlayStop()
    {
        if (hasPlayer)
        {
            if (Player.isPlaying)
            {
                Stop();
            }
            else
            {
                Play();
            }
        }
    }
}