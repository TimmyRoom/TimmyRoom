using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayScene
{
    /// <summary>
    /// SceneRecorder 싱글톤을 사용해서 플레이를 기록하는 함수
    /// </summary>
    void Record();
}
