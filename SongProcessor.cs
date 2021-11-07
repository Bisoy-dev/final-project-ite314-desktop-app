using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BeatVibesApp
{
    public static class SongProcessor
    { 
        
        public async static Task<List<SongModel>> GetAllSongs()
        {
            List<SongModel> songs = new List<SongModel>();
            string url = "http://localhost/beat-vibes/api/retrieve.php";

            using (HttpResponseMessage response = await BeatVibesApiHelper.HttpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    songs = JsonConvert.DeserializeObject<List<SongModel>>(content);
                    return songs;
                }

                return songs;
            }
        } 

        public async static Task<SongModel> GetSong(int id)
        {
            SongModel song = new SongModel();
            string url = $"http://localhost/beat-vibes/api/retrieve_one.php?id={id}";

            using (HttpResponseMessage response = await BeatVibesApiHelper.HttpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    song = JsonConvert.DeserializeObject<SongModel>(content);
                    return song;
                }

                return song;
            }
        }
        public async static Task<string> PostSong(SongModel songModel)
        {
            string url = "http://localhost/beat-vibes/api/insert.php";
            string returnValue = "";

            var fileName = Path.GetFileName(songModel.TrackFile);
            using (var multipartData = new MultipartFormDataContent())
            {
                using (var stream = File.OpenRead(songModel.TrackFile))
                {
                    using (var streamContent = new StreamContent(stream))
                    {
                        using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                        {
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                            var dateFormat = DateTime.Parse(songModel.ReleaseDate.ToString()).ToString("yyyy/MM/dd");

                            multipartData.Add(fileContent, "trackFile", fileName);
                            multipartData.Add(new StringContent(songModel.Artist),"artistName");
                            multipartData.Add(new StringContent(songModel.SongTitle), "songTitle");
                            multipartData.Add(new StringContent(songModel.RecordLabel), "recordLabel");
                            multipartData.Add(new StringContent(songModel.Genre), "genre");
                            multipartData.Add(new StringContent(songModel.Album), "album");
                            multipartData.Add(new StringContent(dateFormat),"releaseDate");

                            using (HttpResponseMessage response = await BeatVibesApiHelper.HttpClient.PostAsync(url, multipartData))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    returnValue = "1";
                                    return returnValue;
                                }

                                returnValue = response.ReasonPhrase;
                                return returnValue;
                            }
                        }
                    }
                }
            }
        }
    }
}
