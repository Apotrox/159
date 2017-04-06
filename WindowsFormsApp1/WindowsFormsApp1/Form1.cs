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
        private string db_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"CurbicLauncher\db.txt";
        public string FileName = "";
        public string FilePath = "";
        public Image add_img = null;
        public string add_img_path = "";
        

        public Form1()
        {
            InitializeComponent();

            //if there arent any games in the database, show the button for that
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
       //if the database doesnt exist, create it
            if (!File.Exists(this.db_path))
            {
                FileStream fh = File.Create(this.db_path);
                fh.Close();
            }

       //Copies content of the database into an array
            string[] file_contents = File.ReadAllLines(this.db_path);
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

       //select a game exe
            OpenFileDialog select = new OpenFileDialog()
            {
                Filter = "exe files (*.exe)|*.exe",
                InitialDirectory = "C:\\",
                Title = "Select the Game.exe"
            };
            DialogResult result = select.ShowDialog();


            if (result == DialogResult.OK)
            {
       //save game exe path for later use
                this.FilePath = select.FileName; 

       //directory for the icon copies
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CubricLauncher\icons\";

       //select a game icon
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
       //if directory for the copies does not exists, create it
                        if (!System.IO.Directory.Exists(appdata))
                        {
                            System.IO.Directory.CreateDirectory(appdata);
                        }

       //copy image
                        System.IO.File.Copy(sourcePath, targetPath, true);
                    }

       //pukes out exception if copying failed
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to copy Icon: " + "\n" + ex);
                    }

       //save Image file and path for later use
                   this.add_img = Image.FromFile(targetPath);
                   this.add_img_path = img.FileName;

                
                }
            }
        }
    }
}
