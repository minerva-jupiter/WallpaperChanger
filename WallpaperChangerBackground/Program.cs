using System.Diagnostics;

string wherePicturesFolder = args[0];
string wherePictureFile;
string[] filePathes = Directory.GetFiles(wherePicturesFolder);
string[] fileNames = new string[filePathes.Length];
int a = 0;
while (filePathes.Length != a)
{
    fileNames[a] = Path.GetFileName(filePathes[a]);
    a++;
}
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
