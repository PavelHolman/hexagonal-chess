using UnityEditor;

public static class BuildScript
{
    static readonly string[] Scenes = { "Assets/Scenes/SampleScene.unity" };

    public static void BuildMac() =>
        BuildPipeline.BuildPlayer(Scenes, "Builds/macOS/Hexagonal Chess.app", BuildTarget.StandaloneOSX, BuildOptions.None);

    public static void BuildWebGL()
    {
        // Gzip with a JS decompression fallback, so static hosts like GitHub Pages
        // can serve the build without Content-Encoding headers
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.template = "PROJECT:FullWindow";
        BuildPipeline.BuildPlayer(Scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
    }
}
