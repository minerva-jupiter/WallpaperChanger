// See https://aka.ms/new-console-template for more information
using System.Diagnostics;


//背景の更新
void UpdateWallPaper()
{
    string command;
    command = "RUNDLL32.EXE user32.dll,UpdatePerUserSystemParameters";
    Process.Start("cmd.exe", command);
}

