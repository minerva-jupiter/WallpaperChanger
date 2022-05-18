using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WallpaperChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //変数宣言
        string wherePictureFile;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Set_Click(object sender, RoutedEventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "Select pictures Folder",
                InitialDirectory = @"C:Users/",
                // フォルダ選択モードにする
                IsFolderPicker = true,
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                // FileNameで選択されたフォルダを取得する
                MessageBox.Show("you selected",$"{cofd.FileName}");

                //Textboxを選択されたフォルダーに変更
                wherePictureFile = cofd.FileName;
                WherePicturesFolder.Text = wherePictureFile;
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if(wherePictureFile == null)
            {
                MessageBox.Show("Folder wasn't selected!");
            }
            else
            {
                string command = "WallpaperChangerBackground.exe" + wherePictureFile;
                Process.Start("cmd.exe", command);
            }
            
        }

        private void WherePicturesFolder_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool directoryExists = System.IO.Directory.Exists(WherePicturesFolder.Text);
            if(directoryExists == false)
            {
                MessageBox.Show("Folder you selected is not exists");
            }
            else
            {
                wherePictureFile = WherePicturesFolder.Text;
            }
        }
    }
}
