using System.Linq; // 마커 검색을 위해 필요
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline; // 타임라인 관련 클래스

public class EX_Actor_Timeline : MonoBehaviour
{
    public PlayableDirector director;
    int nextFrame = 0;
    string nextMarker = "";

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
    public void PlayTimeline()
    {
        director.Play();
        print("Play");
    }

    // 타임라인 일시정지
    public void PauseTimeline()
    {
        director.Pause();
        print("Pause");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeTimeline()
    {
        director.Resume();
        print("Resume");
    }

    // 타임라인 완전 정지
    public void StopTimeline()
    {
        director.Stop();
        print("Stop");
    }

    public void JumpToFrame(int frameIndex)
    {
        double targetTime = (double)frameIndex / 60.0; // 60fps 기준
        director.time = targetTime;

        // Evaluate()를 주석 처리하거나 삭제하고 Play()를 호출해 보세요.
        director.Play();
    }

    public void JumpToClipStart(string trackName, string clipName)
    {
        // 1. 타임라인 에셋에서 특정 트랙을 찾습니다.
        TimelineAsset timeline = director.playableAsset as TimelineAsset;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track.name == trackName)
            {
                // 2. 해당 트랙 안에 있는 클립들 중 이름을 확인합니다.
                foreach (var clip in track.GetClips())
                {
                    if (clip.displayName == clipName)
                    {
                        director.time = clip.start; // 클립의 시작 시간으로 점프!
                        director.Play();
                        return;
                    }
                }
            }
        }
    }

    public void SetNextMarker(string markerName)
    {
        nextMarker = markerName;
        print("nextMarker: " + nextMarker);
    }

    public void JumpToMarker(string markerName)
    {
        // playableAsset을 TimelineAsset으로 형변환
        TimelineAsset timeline = director.playableAsset as TimelineAsset;

        if (timeline == null)
        {
            Debug.LogError("타임라인 에셋을 찾을 수 없습니다.");
            return;
        }

        // GetOutputTracks()를 호출
        var markers = timeline.GetOutputTracks()
            .SelectMany(t => t.GetMarkers())
            .OfType<SignalEmitter>();

        foreach (var m in markers)
        {
            // 이미터 에셋 이름이 아닌, 타임라인 창에서 설정한 '이름'으로 비교하려면
            // 시그널 에셋 자체의 비교 혹은 특별한 네이밍 규칙이 필요
            if (m.asset != null && m.asset.name == markerName)
            {
                director.time = m.time;
                director.Play();
                return;
            }
        }
    }
}
