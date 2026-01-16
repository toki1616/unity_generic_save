using System.IO;
using UnityEngine;

namespace My.Save
{
    public static class SaveUtils
    {
        /// <summary>
        /// 保存フォルダの取得 (PC : exeと同階層、スマホ : 安全な領域)
        /// </summary>
        /// <returns></returns>
        private static string GetSaveFolder()
        {
    #if UNITY_STANDALONE
            //PC : 実行ファイルと同じ階層
            string root = Directory.GetParent(Application.dataPath).FullName;
            string folder = Path.Combine(root, "SaveData");
    #else
            //iOS / Android / その他
            string folder = Path.Combine(Application.persistentDataPath, "SaveData");
    #endif

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }

        /// <summary>
        /// フルパス生成
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetPath(string fileName)
        {
            return Path.Combine(GetSaveFolder(), fileName + ".json");
        }

        /// <summary>
        /// json形式で保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void Save<T>(string fileName, T data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetPath(fileName), json);
        }

        /// <summary>
        /// 保存したものをLoad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        // 汎用 Load
        // -----------------------------
        public static T Load<T>(string fileName) where T : new()
        {
            string path = GetPath(fileName);

            if (!File.Exists(path))
            {
                return new T(); //データが無い場合はデフォルト生成
            }

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
    }
}