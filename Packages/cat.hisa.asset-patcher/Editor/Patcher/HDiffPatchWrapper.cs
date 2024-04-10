using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HisaCat.AssetPatcher.Patcher
{
    public static class HDiffPatchWrapper
    {
        private static void Launch(string arguments, bool logging = true)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe")
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                WorkingDirectory = PathUtility.GethdiffpatchDirPath(),
                Arguments = $"/C {arguments}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            string cmdMsg = null;
            using (var process = System.Diagnostics.Process.Start(startInfo))
            {
                process.WaitForExit();
                using (var reader = process.StandardOutput)
                {
                    cmdMsg = reader.ReadToEnd();
                }
            }

            if (logging)
                Debug.Log(cmdMsg);
        }

        public static void hdiffz(string oldPath, string newPath, string outputDiffFile)
            => Launch($"hdiffz -f \"{oldPath}\" \"{newPath}\" \"{outputDiffFile}\"");

        public static void hpatchz(string oldPath, string diffFile, string outNewPath)
            => Launch($"hpatchz -f \"{oldPath}\" \"{diffFile}\" \"{outNewPath}\"");
    }
}
