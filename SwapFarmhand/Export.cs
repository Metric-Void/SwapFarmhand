using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SwapFarmhand
{

    public struct SaveFile
    {
        public string Name;
        public string SaveGameInfo_Path;
        public string GameFile_Path;

        public SaveFile(string name, string saveGameInfo_Path, string GameFile_Path)
        {
            this.Name = name;
            this.SaveGameInfo_Path = saveGameInfo_Path;
            this.GameFile_Path = GameFile_Path;
        }
    }

    public partial class Export : Form
    {

        public List<SaveFile> DiscoveredSaveFiles;

        public Export()
        {
            InitializeComponent();
            this.DiscoveredSaveFiles = new List<SaveFile>();
        }

        private void Export_Load(object sender, EventArgs e)
        {
            string saveLoc = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "StardewValley", "Saves"
            );

            string[] saveFolders = Directory.GetDirectories(saveLoc, "*", SearchOption.TopDirectoryOnly);

            foreach (string folder in saveFolders)
            {
                string[] files = Directory.GetFiles(folder);

                string savegameName = Path.GetFileName(folder);

                if (savegameName == null|| savegameName.Length == 0) { continue; }

                string? savegameinfoFile = null;
                string? savegameFile = null;

                foreach (string file in files)
                {
                    if(file.EndsWith("SaveGameInfo"))
                    {
                        savegameinfoFile = file;
                    } else if (file.EndsWith(savegameName))
                    {
                        savegameFile = file;
                    }
                }
                
                if (savegameinfoFile != null && savegameFile != null)
                {
                    DiscoveredSaveFiles.Add(new SaveFile(savegameName, savegameinfoFile, savegameFile));
                }
            }

            this.lstSaves.Items.Clear();
            foreach(SaveFile file in this.DiscoveredSaveFiles) {
                this.lstSaves.Items.Add(file.Name);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            int idx = lstSaves.SelectedIndex;

            if(idx == -1) {
                MessageBox.Show("Please select one of the save files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFile target = this.DiscoveredSaveFiles[idx];

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ZIP Archive|*.zip";
            saveFileDialog.FileName = target.Name + ".zip";
            saveFileDialog.DefaultExt = "zip";
            saveFileDialog.AddExtension = true;
            var result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var fname = saveFileDialog.FileName;

                using (var zipStreamOut = new FileStream(fname, FileMode.Create))
                {
                    using (var archive = new ZipArchive(zipStreamOut, ZipArchiveMode.Create, true, System.Text.Encoding.UTF8))
                    {
                        var gameFileEntry = archive.CreateEntry(target.Name + "/" + target.Name, CompressionLevel.Optimal);
                        var gameFileBytes = File.ReadAllBytes(target.GameFile_Path);
                        using (var zipGameFileStream = gameFileEntry.Open())
                        {
                            zipGameFileStream.Write(gameFileBytes);
                        }

                        var saveGameInfoEntry = archive.CreateEntry(target.Name + "/" + "SaveGameInfo", CompressionLevel.Optimal);
                        var saveGameInfoEntryBytes = File.ReadAllBytes(target.SaveGameInfo_Path);
                        using (var zipSaveInfoStream = saveGameInfoEntry.Open())
                        {
                            zipSaveInfoStream.Write(saveGameInfoEntryBytes);
                        }
                    }
                }

                MessageBox.Show("Save File Exported to " + fname, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
