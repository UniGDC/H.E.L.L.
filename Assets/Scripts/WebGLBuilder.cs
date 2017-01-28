using UnityEditor;

static class WebGLBuilder
{
    static void Build()
    {
        string[] scenes = { /* Put the list of scenes to build. */ };
        BuildPipeline.BuildPlayer(scenes, "Build/WebGL/", BuildTarget.WebGL, BuildOptions.None);
        return;
    }
}