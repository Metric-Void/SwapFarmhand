using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SwapFarmhand
{

    public partial class Import : Form
    {

        XmlDocument gameInfoDocument;
        XmlDocument gameDataDocument;
        string rawGameInfo = string.Empty;
        string rawGameData = string.Empty;

        string saveName = string.Empty;

        public Import()
        {
            InitializeComponent();
            this.gameDataDocument = new XmlDocument();
            this.gameInfoDocument = new XmlDocument();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void UpdateFormFromXml()
        {
            var saveGameInfoFarmer = gameInfoDocument.SelectSingleNode("//Farmer/name").InnerText;
            var saveGameDataFarmer = gameDataDocument.SelectSingleNode("//SaveGame/player/name").InnerText;

            if (saveGameInfoFarmer != saveGameDataFarmer)
            {
                MessageBox.Show($"SaveGameInfo does not match save game data.\nSaveGameInfo main player name:{saveGameInfoFarmer}\nSaveGame Data main player name:{saveGameDataFarmer}\nUsing Save Game Data.",
                    "SaveGame mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            lblMainFarmhand.Text = saveGameDataFarmer;

            lstFarmHands.Items.Clear();
            foreach(XmlNode cnode in gameDataDocument.SelectSingleNode("//SaveGame/farmhands").ChildNodes)
            {
                var handName = cnode.SelectSingleNode("name").InnerText;
                
                if(handName == "")
                {
                    lstFarmHands.Items.Add("<Empty>");
                } else
                {
                    lstFarmHands.Items.Add(handName);
                }
            }
        }

        private void Import_Load(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ZIP Archive (*.zip)|*.zip";
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.DefaultExt = "zip";

            if (ofd.ShowDialog() == DialogResult.OK) {
                var zipFName = ofd.FileName;

                using (var zipStreamIn = new FileStream(zipFName, FileMode.Open)) {
                    using (var archive = new ZipArchive(zipStreamIn, ZipArchiveMode.Read, true, System.Text.Encoding.UTF8))
                    {
                        string? gameInfoEntry = null;
                        string? gameDataEntry = null;

                        foreach(var entry in archive.Entries)
                        {
                            var path = entry.FullName.Split('/')[0];
                            if(saveName == String.Empty)
                            {
                                saveName = path;
                            }
                            if(entry.Name.EndsWith("SaveGameInfo"))
                            {
                                gameInfoEntry = entry.FullName;
                            } else if (entry.Name == path)
                            {
                                gameDataEntry = entry.FullName;
                            }
                        }

                        if (gameInfoEntry == null || gameDataEntry == null) {
                            MessageBox.Show("Not a valid export file.", "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                            return;
                        }

                        using (var gameInfoStream = archive.GetEntry(gameInfoEntry).Open())
                        {
                            using (StreamReader srInfo = new StreamReader(gameInfoStream))
                            {
                                rawGameInfo = srInfo.ReadToEnd();
                            }
                            this.gameInfoDocument.LoadXml(rawGameInfo);
                        }

                        using (var gameDataStream = archive.GetEntry(gameDataEntry).Open())
                        {
                            using (StreamReader srInfo = new StreamReader(gameDataStream))
                            {
                                rawGameData = srInfo.ReadToEnd();
                            }
                            this.gameDataDocument.LoadXml(rawGameData);
                        }

                        UpdateFormFromXml();
                        MessageBox.Show("Save File Loaded", "File Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            } else
            {
                this.Close();
            }
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            int selectedMainHand = lstFarmHands.SelectedIndex;
            if (selectedMainHand == -1)
            {
                MessageBox.Show("Please select a farmhand to swap with.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var prevMainPlayerNode = gameDataDocument.SelectSingleNode("//SaveGame/player").CloneNode(true);
            var newMainPlayerNode = gameDataDocument.SelectSingleNode("//SaveGame/farmhands").ChildNodes[selectedMainHand].CloneNode(true);
               
            if(newMainPlayerNode.SelectSingleNode("name").InnerText == "")
            {
                var dlgresult = MessageBox.Show("This slot is empty (no player)! Continue?", "Empty player", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dlgresult == DialogResult.Cancel)
                {
                    return;
                }
            }

            gameDataDocument.SelectSingleNode("//SaveGame/player").RemoveAll();
            foreach (XmlNode child in newMainPlayerNode.ChildNodes)
            {
                gameDataDocument.SelectSingleNode("//SaveGame/player").AppendChild(child.Clone());
            }

            gameDataDocument.SelectSingleNode($"//SaveGame/farmhands/Farmer[{selectedMainHand + 1}]").RemoveAll();
            foreach (XmlNode child in prevMainPlayerNode.ChildNodes)
            {
                gameDataDocument.SelectSingleNode($"//SaveGame/farmhands/Farmer[{selectedMainHand + 1}]").AppendChild(child.Clone());
            }

            // Manipulating SaveGameInfo XML is harder, as importing nodes directly destroys xsi info.
            int begin = Lib.NthIndexOf(rawGameData, "<Farmer>", selectedMainHand + 1);
            int end = Lib.NthIndexOf(rawGameData, "</Farmer>", selectedMainHand + 1) + "</Farmer>".Length;

            string mainPlayerSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + rawGameData.Substring(begin, end - begin);
            mainPlayerSection = mainPlayerSection.Replace("<Farmer>", "<Farmer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            gameInfoDocument.LoadXml(mainPlayerSection);

            UpdateFormFromXml();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string saveLoc = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "StardewValley", "Saves", saveName
            );

            if(Directory.Exists(saveLoc))
            {
                var dlgresult = MessageBox.Show(
                    $"There is already a save file named {saveName}. Continue to overwrite?",
                    "Overwrite?",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                );

                if(dlgresult == DialogResult.Cancel)
                {
                    return;
                }
            } else
            {
                Directory.CreateDirectory(saveLoc);
            }

            this.gameInfoDocument.Save(System.IO.Path.Combine(saveLoc, "SaveGameInfo"));
            this.gameDataDocument.Save(System.IO.Path.Combine(saveLoc, saveName));

            MessageBox.Show(
                $"Save File Imported to {saveLoc}",
                "Success"
            );
        }
    }
}
