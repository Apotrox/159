using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using System.Resources;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public  String FileName { get; set; }
        public  String FilePath { get; set; }
        public  Image add_img { get; set; }
        

        public Form1()
        {
            
            InitializeComponent();
            /* if (games.Components.Count == 0)
             {
                 nogames.Visible = true;
                 nogames.Enabled = true;
             }
             else
             {
                 nogames.Visible = false;
                 nogames.Enabled = false;
             }
         */
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            MyItem itm = new MyItem();

            int x = addbutton.Location.X + 60;
            int y = addbutton.Location.Y + 95;

        /*    foreach ()
            {
                Button btn = new Button();

                btn.Location = new Point(x + 250, y);
                btn.Name = "";
                btn.Text = "";

            }
        */
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }


        private void addbutton_Click(object sender, EventArgs e)
        {
            searchGame();
        }

        private void nogames_Click(object sender, EventArgs e)
        {
            searchGame();
        }


        //Search a Game

        public void searchGame()
        {
            OpenFileDialog select = new OpenFileDialog()
            {
                Filter = "exe files (*.exe)|*.exe",
                InitialDirectory = "C:\\",
                Title = "Select the Game.exe"
            };
            DialogResult result = select.ShowDialog();

            if (result == DialogResult.OK)
            {
                FilePath = select.FileName; 
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CubricLauncher\icons\";

                OpenFileDialog img = new OpenFileDialog()
                {
                    Filter = "png files (*.png)|*.png",
                    InitialDirectory = "C\\Users\\Desktop",
                    Title = "Select a Game Icon"
                };
                DialogResult res = img.ShowDialog();
                if (res == DialogResult.OK)
                {
                    //Copy image to Appdata
                    string targetPath = @Path.Combine(appdata, Path.GetFileName(img.FileName));
                    string sourcePath = @img.FileName;

                    try
                    {

                        if (!System.IO.Directory.Exists(targetPath))
                        {
                            System.IO.Directory.CreateDirectory(targetPath);
                        }

                        System.IO.File.Copy(sourcePath, targetPath, true);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to copy Icon: " + "\n" + ex);
                    }

                   // add_img = Image.FromFile(targetPath);
                }
            }
        }
    }

    class MyItem : Form1
    {

    }
}
