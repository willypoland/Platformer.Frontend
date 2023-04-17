using System;
using System.IO;
using Game.Code.Data;
using Game.Code.Data.Settings;
using UnityEngine;


namespace Game.Code.Services
{
    public class GameSettingsService
    {
        private GameSettings _settings;
        private string _json;
        private DateTime _lastWrite;

        public event Action<GameSettings> Changed; 

        public GameSettings Read()
        {
            var path = GetPath();
            
            if (!File.Exists(path))
            {
                CreateSettingsFile(path);
            }
            else
            {
                ReadSettingFile(path);
            }

            return _settings;
        }

        private void ReadSettingFile(string path)
        {
            var last = File.GetLastWriteTime(path);

            if (last != _lastWrite)
            {
                _json = File.ReadAllText(path);
                _settings = JsonUtility.FromJson<GameSettings>(_json);
            }
            
            _lastWrite = last;
        }

        private void CreateSettingsFile(string path)
        {
            _settings = new GameSettings();
            _json = JsonUtility.ToJson(_settings, true);
            File.WriteAllText(path, _json);
            _lastWrite = File.GetLastWriteTime(path);
        }

        public void Write(GameSettings settings)
        {
            var newJson = JsonUtility.ToJson(settings);

            if (newJson != _json)
            {
                var path = GetPath();
                File.WriteAllText(path, newJson);
                _json = newJson;
                _settings = settings;
                Changed?.Invoke(_settings);
            }
        }
        
        private static string GetPath() => Path.Combine(Application.dataPath, Constants.ConfigFileName);
    }
    
}