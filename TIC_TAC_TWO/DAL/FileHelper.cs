namespace DAL;

public static class FileHelper
{
    public static readonly string BasePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "configurations");

    public static string ConfigExtension = ".config.json";
    public static string GameExtension = ".game.json";

    static FileHelper()
    {
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }
    }
}