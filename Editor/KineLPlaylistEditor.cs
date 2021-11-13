using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Kinel.VideoPlayer.Scripts.Playlist;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon;

namespace aki_lua87.Editor.KineLPlaylist
{
    public class KineLPlaylistEditor : EditorWindow
    {
        private readonly string logPrefix = "[aki_lua87] ";
        [SerializeField] private GameObject kinelPlayList;
        [SerializeField] private string channelID = "";
        [SerializeField] private string youtubeAPIKey = "";
        [SerializeField] private string searchWord = "";
        [SerializeField] private string playlistID = "";

        private KinelPlayListGenerator instance;

        // private KinelPlayListGeneratorInspector kinelPlayListGeneratorInspector; // = new();

        [MenuItem("aki_lua87/KineLPlaylistEditor")]
        private static void Init()
        {
            EditorWindow.GetWindow(typeof(KineLPlaylistEditor));
        }

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.Space();
                this.kinelPlayList = (GameObject)EditorGUILayout.ObjectField("KineL式 PlayList", this.kinelPlayList, typeof(GameObject), true);
                EditorGUILayout.Space();
                this.channelID = EditorGUILayout.TextField("YoutubeチャンネルID", this.channelID);
                if (GUILayout.Button("チャンネル最新15動画追加"))
                {
                    CreatePlaylistForYoutubeChannel();
                }
                if (GUILayout.Button("緋狐式動画システム(β)で追加"))
                {
                    CreatePlaylistForAkiSyatem();
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                this.youtubeAPIKey = EditorGUILayout.PasswordField("Youtube Data API Key", youtubeAPIKey);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                this.searchWord = EditorGUILayout.TextField("検索キーワード", this.searchWord);
                if (GUILayout.Button("検索結果追加(require API Key)"))
                {
                    CreatePlaylistForYoutubeSearchWord();
                }
                EditorGUILayout.Space();
                this.playlistID = EditorGUILayout.TextField("YoutubeプレイリストID", this.playlistID);
                if (GUILayout.Button("プレイリスト追加(require API Key)"))
                {
                    CreatePlaylistForYoutubePlaylist();
                }
            }
            if (this.kinelPlayList != null)
            {
                instance = this.kinelPlayList.GetComponent<KinelPlayListGenerator>();
            }
        }

        public void CreatePlaylist(string tagTitle, List<string> videoTitles, List<string> descriptions, List<string> urls, List<string> dummys, List<int> playMode)
        {
            var tabParent = instance.playlist.transform.Find("Canvas/Playlist/Tag/Viewport/Content").gameObject;
            var tabPrefab = tabParent.transform.Find("TabPrefab").gameObject;
            var listParent = instance.playlist.transform.Find("Canvas/Playlist/List").gameObject;
            var listPrefab = listParent.transform.Find("ListPrefab").gameObject;

            var index = instance.tags.Count;
            instance.tags.Add(tagTitle);

            var playlistObject = CreateTabPrefab(tagTitle, index, tabPrefab, listPrefab, tabParent, listParent, isActive: false);
            var playlist = playlistObject.GetUdonSharpComponent<KinelPlaylist>();
            playlist.titles = videoTitles.ToArray();
            playlist.descriptions = descriptions.ToArray();
            playlist.urlString = urls.ToArray();
            playlist.dummy = dummys.ToArray();
            playlist.playMode = playMode.ToArray();
            playlist.autoPlay = false;

            UdonSharpEditorUtility.CopyProxyToUdon(playlist);

            playlistObject.SetActive(true);
            playlistObject.SetActive(false);

            Debug.Log($"{logPrefix}prefabを作成しました");
        }

        private GameObject CreateTabPrefab(string tagName, int index, GameObject tabPrefab, GameObject listPrefab, GameObject tabParent, GameObject listParent, bool isActive)
        {
            Debug.Log($"{logPrefix}Call CreateTabPrefab");
            var tab = Instantiate(tabPrefab);
            var playlist = Instantiate(listPrefab);

            tab.transform.SetParent(tabParent.transform);
            playlist.transform.SetParent(listParent.transform);
            tab.transform.localPosition = Vector3.zero;
            tab.transform.localRotation = Quaternion.identity;
            tab.transform.localScale = Vector3.one;
            tab.name = $"Tag {tagName}";
            tab.SetActive(true);

            playlist.transform.localPosition = new Vector3(-325, -450, 0);
            playlist.transform.localRotation = Quaternion.identity;
            playlist.transform.localScale = Vector3.one;

            var toggleComponet = tab.GetComponent<Toggle>();
            toggleComponet.isOn = isActive;

            // var onValueChanged = toggleComponet.onValueChanged;

            // void OnToggle(bool value)
            // {
            //     listPrefab.SetActive(value);
            // }

            // onValueChanged.AddListener(OnToggle);

            var playlistComponent = playlist.GetComponent<KinelPlaylist>();

            var text = tab.transform.GetChild(1).GetComponent<Text>();
            text.text = $"{tagName}";
            playlist.name = $"List {tagName}";

            instance.playlistList.Add(playlistComponent);

            playlist.SetActive(isActive);
            return playlist;
        }



        public void CreatePlaylistForYoutubePlaylist()
        {
            Debug.Log($"{logPrefix}Call CreatePlaylistForYoutubePlaylist");
            try
            {
                var apiKey = this.youtubeAPIKey;
                var playlistID = this.playlistID;
                var maxResults = 50;

                // タイトル取得(少し雑にだけど)
                string tURL = $"https://www.googleapis.com/youtube/v3/playlists?id={playlistID}&key={apiKey}&part=snippet";
                string tjson = new HttpClient().GetStringAsync(tURL).Result;
                var tdata = JsonUtility.FromJson<YoutubeWebResponse>(tjson);
                var tagTitle = "";
                foreach (var item in tdata.items)
                {
                    tagTitle = item.snippet.title;
                    break;
                }

                // 内容取得
                string URL = $"https://www.googleapis.com/youtube/v3/playlistItems?playlistId={playlistID}&key={apiKey}&part=snippet&maxResults={maxResults}";
                string json = new HttpClient().GetStringAsync(URL).Result;

                Debug.Log("response json:" + json);

                var data = JsonUtility.FromJson<YoutubeWebResponse>(json);
                Debug.Log("data:" + JsonUtility.ToJson(data));

                var urlleft = "https://www.youtube.com/watch?v=";


                var videoTitles = new List<string>();
                var descriptions = new List<string>();
                var urls = new List<string>();
                var dummys = new List<string>();
                var playMode = new List<int>();
                foreach (var item in data.items)
                {
                    var targetURL = $"{urlleft}{item.snippet.resourceId.videoId}";
                    Debug.Log($"videoTitle:{item.snippet.title} description:{item.snippet.description} url:{targetURL}");
                    videoTitles.Add(item.snippet.title);
                    descriptions.Add(item.snippet.description);
                    urls.Add(targetURL);
                    dummys.Add("");
                    playMode.Add(0);
                }
                CreatePlaylist(tagTitle, videoTitles, descriptions, urls, dummys, playMode);
            }
            catch (Exception e)
            {
                Debug.Log($"{logPrefix}プレイリスト作成に失敗しました: {e.Message}");
            }
        }

        public void CreatePlaylistForYoutubeChannel()
        {
            Debug.Log($"{logPrefix}Call CreatePlaylistForYoutubeChannel");

            try
            {
                var channel = channelID;
                string URL = $"https://www.youtube.com/feeds/videos.xml?channel_id={channel}";

                var xml = XDocument.Load(URL);
                var root = xml.Root;

                var auther = root.Elements().Where(p => p.Name.LocalName == "title").FirstOrDefault();
                Debug.Log($"{logPrefix}auther => " + auther.Value);

                var entrys = root.Elements().Where(p => p.Name.LocalName == "entry");
                if (entrys != null)
                {
                    var videoTitles = new List<string>();
                    var descriptions = new List<string>();
                    var urls = new List<string>();
                    var dummys = new List<string>();
                    var playMode = new List<int>();
                    foreach (var entry in entrys)
                    {
                        var mediaGroup = entry.Elements().Where(p => p.Name.LocalName == "group").FirstOrDefault();
                        var videoTitle = mediaGroup.Elements().Where(p => p.Name.LocalName == "title").Select(p => p.Value).FirstOrDefault();
                        var description = mediaGroup.Elements().Where(p => p.Name.LocalName == "description").Select(p => p.Value).FirstOrDefault();
                        var url = entry.Elements().Where(p => p.Name.LocalName == "link").Select(p => p.Attribute("href").Value).FirstOrDefault();
                        Debug.Log($"videoTitle:{videoTitle} description:{description} url:{url}");
                        videoTitles.Add(videoTitle);
                        descriptions.Add(description);
                        urls.Add(url);
                        dummys.Add("");
                        playMode.Add(0);
                    }
                    CreatePlaylist(auther.Value, videoTitles, descriptions, urls, dummys, playMode);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"{logPrefix}プレイリスト作成に失敗しました: {e.Message}");
            }
            return;
        }

        public void CreatePlaylistForYoutubeSearchWord()
        {
            Debug.Log($"{logPrefix}Call CreatePlaylistForYoutubeSearchWord");
            if (instance == null)
            {
                Debug.Log($"{logPrefix}Nothing playlist object");
                return;
            }
            try
            {
                var apiKey = youtubeAPIKey;
                var word = searchWord;
                var maxResults = 15;
                string URL = $"https://www.googleapis.com/youtube/v3/search?q={word}&key={apiKey}&part=snippet&maxResults={maxResults}&type=video";
                string json = new HttpClient().GetStringAsync(URL).Result;

                Debug.Log($"{logPrefix}response json:" + json);

                var data = JsonUtility.FromJson<YoutubeWebResponse>(json);
                Debug.Log($"{logPrefix}data:" + JsonUtility.ToJson(data));

                var urlleft = "https://www.youtube.com/watch?v=";
                var tagTitle = "検索結果:" + word;

                var videoTitles = new List<string>();
                var descriptions = new List<string>();
                var urls = new List<string>();
                var dummys = new List<string>();
                var playMode = new List<int>();
                foreach (var item in data.items)
                {
                    var targetURL = $"{urlleft}{item.id.videoId}";
                    Debug.Log($"{logPrefix}videoTitle:{item.snippet.title} description:{item.snippet.description} url:{targetURL}");
                    videoTitles.Add(item.snippet.title);
                    descriptions.Add(item.snippet.description);
                    urls.Add(targetURL);
                    dummys.Add("");
                    playMode.Add(0);
                }
                CreatePlaylist(tagTitle, videoTitles, descriptions, urls, dummys, playMode);
            }
            catch (Exception e)
            {
                Debug.Log($"{logPrefix}プレイリスト作成に失敗しました: {e}");
            }
        }

        public async void CreatePlaylistForAkiSyatem()
        {
            Debug.Log($"{logPrefix}Call CreatePlaylistForAkiSyatem");
            if (instance == null)
            {
                Debug.Log($"{logPrefix}Nothing playlist object");
                return;
            }
            try
            {
                var channel = channelID;
                string URL = $"https://vrc.akakitune87.net/video/yt/channel/regist";
                // 手書きじゃないいい方法
                var reqJson = "{ \"channel_id\" : \"" + channel + "\"}";

                var content = new StringContent(reqJson, Encoding.UTF8, "application/json");
                var client = new System.Net.Http.HttpClient();
                var res = await client.PostAsync(URL, content);
                var resJson = await res.Content.ReadAsStringAsync();

                Debug.Log($"{logPrefix}response json:" + resJson);

                var resData = JsonUtility.FromJson<akiWebResponse>(resJson);
                Debug.Log($"{logPrefix}data:" + JsonUtility.ToJson(resData));

                if (!(resData.result == "OK"))
                {
                    Debug.Log($"{logPrefix}API実行に失敗しました");
                    return;
                }

                var indexURL = $"https://vrc.akakitune87.net/videos/yt/chlist/{channel}";
                var urlleft = $"https://vrc.akakitune87.net/videos/yt/ch/{channel}?n=";
                var tagTitle = "" + resData.auther;

                var videoTitles = new List<string>();
                var descriptions = new List<string>();
                var urls = new List<string>();
                var dummys = new List<string>();
                var playMode = new List<int>();

                videoTitles.Add("目次");
                descriptions.Add("");
                urls.Add(indexURL);
                dummys.Add("");
                playMode.Add(0);
                for (int i = 0; i < 15; i++)
                {
                    var targetURL = $"{urlleft}{i}";
                    videoTitles.Add($"{i + 1}");
                    descriptions.Add("");
                    urls.Add(targetURL);
                    dummys.Add("");
                    playMode.Add(0);
                }
                CreatePlaylist(tagTitle, videoTitles, descriptions, urls, dummys, playMode);
            }
            catch (Exception e)
            {
                Debug.Log($"{logPrefix}プレイリスト作成に失敗しました: {e}");
            }
        }

        [Serializable]
        public class akiWebResponse
        {
            public string result;
            public string auther;
        }

        [Serializable]
        public class YoutubeWebResponse
        {
            public string kind;
            public string etag;
            public string nextPageToken;
            public string regionCode;
            public YoutubeWebResponsePageInfo pageInfo;
            public YoutubeWebResponseItems[] items;

            [Serializable]
            public class YoutubeWebResponsePageInfo
            {
                public int totalResults;
                public int resultsPerPage;
            }

            [Serializable]
            public class YoutubeWebResponseItems
            {
                public string kind;
                public string etag;
                public YoutubeWebResponseItemsId id;
                public YoutubeWebResponseItemsSnippet snippet;

                [Serializable]
                public class YoutubeWebResponseItemsId
                {
                    public string kind;
                    public string videoId;
                }
                [Serializable]
                public class YoutubeWebResponseItemsResourceId
                {
                    public string kind;
                    public string videoId;
                }

                [Serializable]
                public class YoutubeWebResponseItemsSnippet
                {
                    public string publishedAt;
                    public string channelId;
                    public string title;
                    public string description;
                    public string channelTitle;
                    public string liveBroadcastContent;
                    public string publishTime;
                    public YoutubeWebResponseItemsResourceId resourceId;
                }
            }
        }
    }
}