using System;
using System.IO;
using Newtonsoft.Json;

namespace Diploma.Gateway.Common
{
    /// <summary>
    /// JSON парсер
    /// </summary>
    public static class JsonLoader
    {
        /// <summary>
        /// Загрузить из файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns></returns>
        public static T LoadFromFile<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        /// <summary>
        /// Десериализовать
        /// </summary>
        /// <param name="jsonObject">JSON объект</param>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns></returns>
        public static T Deserialize<T>(object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(Convert.ToString(jsonObject));
        }
    }
}