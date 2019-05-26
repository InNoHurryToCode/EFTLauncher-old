using System;

namespace EFTLauncher.ClientData
{
    public struct ConfigData
    {
        public string BackendUrl;
        public string Version;
        public string BuildVersion;
        public bool LocalGame;
        public int AmmoPoolSize;
        public int WeaponsPoolSize;
        public int MagsPoolSize;
        public int ItemsPoolSize;
        public int PlayersPoolSize;
        public int ObservedFix;
        public int TargetFrameRate;
        public int BotsCount;
        public bool ResetSettings;
        public bool SaveResults;
        public int FixedFrameRate;

        public override string ToString()
        {
            return "BackendUrl: " + BackendUrl + Environment.NewLine
                 + "Version: " + Version + Environment.NewLine
                 + "BuildVersion: " + BuildVersion + Environment.NewLine
                 + "LocalGame: " + LocalGame + Environment.NewLine
                 + "AmmoPoolSize: " + AmmoPoolSize + Environment.NewLine
                 + "WeaponsPoolSize: " + WeaponsPoolSize + Environment.NewLine
                 + "MagsPoolSize: " + MagsPoolSize + Environment.NewLine
                 + "ItemsPoolSize: " + ItemsPoolSize + Environment.NewLine
                 + "PlayersPoolSize: " + PlayersPoolSize + Environment.NewLine
                 + "ObservedFix: " + ObservedFix + Environment.NewLine
                 + "TargetFrameRate: " + TargetFrameRate + Environment.NewLine
                 + "BotsCount: " + BotsCount + Environment.NewLine
                 + "ResetSettings: " + ResetSettings + Environment.NewLine
                 + "SaveResults: " + SaveResults + Environment.NewLine
                 + "FixedFrameRate: " + FixedFrameRate;
        }
    }
}
