using Godot;

namespace SnakeBiteSakiSfx;

public partial class CustomSfx : Node
{
    private static CustomSfx? _instance;
    private readonly List<AudioStreamPlayer> _sfxPool = new();
    private readonly Dictionary<string, AudioStream> _cache = new();

    private const string DefaultBus = "sfx";

    public static CustomSfx? Instance
    {
        get
        {
            if (_instance == null)
            {
                if (Engine.GetMainLoop() is not SceneTree root)
                {
                    return null;
                }
                _instance = new CustomSfx();
                root.Root.AddChild(_instance);
            }
            return _instance;
        }
    }

    private CustomSfx() { }

    public void Play(string path, float volume = 1f)
    {
        AudioStreamPlayer? player = _sfxPool.Find(static p => !p.Playing);

        if (player == null)
        {
            player = new AudioStreamPlayer
            {
                Bus = AudioServer.GetBusIndex(DefaultBus) >= 0 ? DefaultBus : "Master"
            };
            player.Finished += () => player.Stop();
            AddChild(player);
            _sfxPool.Add(player);
        }

        if (!_cache.TryGetValue(path, out var stream))
        {
            stream = GD.Load<AudioStream>(path);
            if (stream != null)
            {
                _cache[path] = stream;
            }
        }

        if (stream == null)
        {
            Console.WriteLine($"Failed to load audio stream at path: {path}");
            return;
        }

        player.VolumeDb = ToDb(volume);
        player.Stream = stream;

        player.Play();
    }

    private static float ToDb(float volume)
    {
        if (volume <= 0f)
            return -80f;
        return Mathf.LinearToDb(volume);
    }

}