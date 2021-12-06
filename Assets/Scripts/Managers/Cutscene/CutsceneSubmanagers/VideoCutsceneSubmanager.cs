using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCutsceneSubmanager : AbstractCSSubmanager<VideoCutscene>
{
    public override CutsceneType CutsceneType { get { return CutsceneType.Video; } }

    public override void Continue()
    {
        throw new NotImplementedException();
    }

    public override void FinishCutscene()
    {
        throw new NotImplementedException();
    }

    public override void LaunchCutscene(VideoCutscene cutscene, Action cutsceneFinishedCallback)
    {
        throw new NotImplementedException();
    }
}
