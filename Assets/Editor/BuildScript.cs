using UnityEditor;

public static class BuildScript
{
    static readonly string[] Scenes = { "Assets/Scenes/SampleScene.unity" };

    public static void BuildMac() =>
        BuildPipeline.BuildPlayer(Scenes, "Builds/macOS/Hexagonal Chess.app", BuildTarget.StandaloneOSX, BuildOptions.None);

    public static void BuildWebGL()
    {
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        BuildPipeline.BuildPlayer(Scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
    }
}
