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
        //variables necessary for the game.exe and its icon
        private string db_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"CurbicLauncher\db.txt";
        private string FileName = "";
        private string FilePath = "";
        private Image add_img = null;
        private string add_img_path = "";

        //list that contains every entered game
        private List<Games> games;
        //list that contains every created game button
        private List<Button> game_btns = new List<Button>();

        public Form1()
        {
            InitializeComponent();

       //if there arent any games in the database, show the button for that
             if (games==null)
             {
                 nogames.Visible = true;
                 nogames.Enabled = true;
             }
             else
             {
                 nogames.Visible = false;
                 nogames.Enabled = false;
             }
         
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

       //Create game list
            this.games = new List<Games>();
       //empty list slot
            Games tmp_stuff = new Games();

       //tests if there is anything in the database
            foreach (string line in file_contents)
            {
       //if it comes to the end of the database (its empty)
                if (line.Trim().Length == 0)
                {
                    //insert stuff
                    this.games.Add(tmp_stuff);
                    continue;
                }
                
       //if there is anything in the database, test what it is and fill the specified field in the Games-object with the tmp_stuff
                string[] splitted_line = line.Split(":".ToCharArray(), 2); //2 == split only 2 times
                switch (splitted_line.First())
                {
                    case "path":
                        tmp_stuff.path = splitted_line.Last();
                        continue;
                    case "name":
                        tmp_stuff.name = splitted_line.Last();
                        continue;
                    case "img":
                        try
                        {
                            tmp_stuff.img = Image.FromFile(splitted_line.Last());
                        }
                        catch
                        {
       //load default picture
                            
                        }
                        continue;
                }
            }

            //after this point, its pretty safe that everthing is loaded properly, so draw the buttons 
            //the buttons just have to start the startExecuteable Method for the Games-object

            this.drawButtons();
        }

        private void drawButtons()
        {
       //delete all buttons
            foreach (Button btn in this.game_btns)
            {
                btn.Dispose();
            }
       //create all buttons
            int y = 170;
            int x = 150;
            int gamecount = 0;
            foreach (Games stuff in this.games)
            {
                Button btn = new Button();
                btn.BackgroundImage = stuff.img;
                btn.Text = "";
                btn.Size = new System.Drawing.Size(200, 200);
                btn.Location = new Point(x,y);
                gamecount++;
                x = x + 250;
                btn.Click += new EventHandler(stuff.startExecutable);
                this.game_btns.Add(btn);
                this.Controls.Add(btn);
       //if the gamecount in one row exceeds 5, it will start a new row
                if (gamecount > 5)
                {
                    x = 150;
                    y = y + 250;
                }

            }
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

        private void writeListToFile(List<Games> stuffs, string db_path)
        {
            string full_file_text = "";
            foreach (Games stuff in stuffs)
            {
                string filestring =
                    "name:{0}" + Environment.NewLine +
                    "img:{1}" + Environment.NewLine +
                    "path:{2}" + Environment.NewLine +
                    Environment.NewLine;
                filestring = string.Format(filestring, stuff.name, stuff.img_path, stuff.path);
                full_file_text += filestring;
            }
            File.WriteAllText(this.db_path, full_file_text);
        }

        //Search a Game

        public void searchGame()
        {
            StartPosition:

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
                this.FileName = Path.GetFileName(FilePath);

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
                        MessageBox.Show("Unable to copy Icon: " + "\n" + ex, "Warning!");
                    }

       //confirmation
                    DialogResult msgbox;
                    msgbox = MessageBox.Show("Is everything correct?: " + "\n" + "Game.exe: " + select.FileName +
                                             "\n" + "Game icon: " + Path.GetFileName(img.FileName), "Confirmation", MessageBoxButtons.YesNo);

                    if (msgbox == DialogResult.Yes)
                    {
       //save Image file and path for later use
                        this.add_img = Image.FromFile(targetPath);
                        this.add_img_path = img.FileName;
                    }
                    else
                    {
       //confirmation if the image has to be deleted or not
                        msgbox = MessageBox.Show("Please add the game again with correct data." +
                                                 " Nevertheless, the image got copied. Do you want to delete it?", "Warning", MessageBoxButtons.YesNo);

                        if (msgbox == DialogResult.Yes)
                        {
                            File.Delete(targetPath);
                        }
                        else
                        {
                            msgbox = MessageBox.Show("Alright. Then do as you behave.", "Confirmation");
                            
                        }
                        goto StartPosition;
                    }
                }
            }

       //add the game
            if(this.add_img_path!="" && this.FilePath!="" && this.FileName!="")
            {
       //writes everything to the list and in the databse
                this.games.Add(new Games(this.FilePath, this.FileName,this.add_img_path));
                this.writeListToFile(this.games, this.db_path);
       //reset everything
                this.add_img_path = "";
                this.add_img = null;
                this.FileName = "";
                this.FilePath = "";
       //draw the buttons
                drawButtons();
            }
        }
    }

    class Games
    {
        public string path;
        public string name;
        public Image img;
        public string img_path;

        public Games()
        {

        }
        public Games(string path, string name, string imgPath)
        {
            this.path = path;
            this.name = name;
            this.img_path = imgPath;
            this.img = Image.FromFile(imgPath);
        }

        public void startExecutable(object sender, EventArgs e)
        {
            try
            {
                Process.Start(this.path);
            }
            catch(Exception ex)
            {
                
                MessageBox.Show("An error occured!" + "\n\n" + ex,"Error!");
            }

        }
    }
}
