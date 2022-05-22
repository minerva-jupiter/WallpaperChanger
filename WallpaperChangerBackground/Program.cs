using Nancy.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

string wherePicturesFolder = args[0];
string wherePictureFile;
string[] filePathes = Directory.GetFiles(wherePicturesFolder);
string[] fileNames = new string[filePathes.Length];
int a = 0;
int year = DateTime.Now.Year;
int day = DateTime.Now.Day;
int month = DateTime.Now.Month;

while (filePathes.Length != a)
{
    fileNames[a] = Path.GetFileName(filePathes[a]);
    a++;
}

var apiUrl = "https://maps.googleapis.com/maps/api/geocode/json?";
var jsonParameter = new JavaScriptSerializer().Serialize(new
                    {
                        name = "Name",
                        email = "EmailAddress",
                        password = "Password",
                        detail_info = new
                        {
                            info1 = "info1",
                            info2 = "info2"
                        }
                    });

JObject response = GetAPI(apiUrl, jsonParameter);

//日昇日没時間取得
var apiUrl2 = "https://labs.bitmeister.jp/ohakon/json/";
var jsonParameter2 = new JavaScriptSerializer().Serialize(new
                    {
                        mode = "mode=sun_rise_set",
                        year = "year="+ year,
                        month = "month=" + month,
                        day = "day=" + day,
                        detail_info = new
                        {
                            info1 = "info1",
                            info2 = "info2"
                        }
                    });

JObject response = GetAPI(apiUrl2, jsonParameter2);
SetTimePicture();

SetPicture();

UpdateWallPaper();

void SetTimePicture()
{
    int time = DateTime.Now.Hour;
    //朝焼けに対応するメソッドを別途作成
    int whatPictureIndex = Array.IndexOf(fileNames,time);
    wherePictureFile = filePathes[whatPictureIndex];
}
void SetPicture()
{
    string command1 = "reg add ";
    string command2 = @"HKEY_CURRENT_USER\Control Panel\Desktop ";
    string command3 = "/v Wallpaper /t REG_SZ /d ";
    string command4 = " /f";
    string command = command1+command2+command3+wherePictureFile+command4;
    
    //
    Console.WriteLine(command);

    Process.Start("cmd.exe", command);
}
//背景の更新
void UpdateWallPaper()
{
    string command;
    command = "RUNDLL32.EXE user32.dll,UpdatePerUserSystemParameters";
    Process.Start("cmd.exe", command);
}

//日昇日没時刻の取得
JObject GetAPI(string apiUrl, string jsonParameter)
{
    JObject response = null;
    try
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
        request.Method = "POST";
        request.ContentType = "application/json;";
        // カスタムヘッダーが必要の場合(認証トークンとか)
        request.Headers.Add("custom-api-param", "value");

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonParameter);
        }

        var httpResponse = (HttpWebResponse)request.GetResponse();

        // HttpStatusCodeの判断が必要なら
        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("API呼び出しに失敗しました。");
        }

        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            response = JObject.Parse(streamReader.ReadToEnd());
        }

        // ex) response["status"]:"success"
    }
    catch (WebException wex)
    {
        // 200以外の場合
        if (wex.Response != null)
        {
            using (var errorResponse = (HttpWebResponse)wex.Response)
            {
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    response = JObject.Parse(reader.ReadToEnd());
                }
            }
        }
    }

    return response;
}