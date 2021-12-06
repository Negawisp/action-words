using System;

public interface ICutscene
{
    CutsceneType CutsceneType { get; }
    void Load();
    void Unload();
}
