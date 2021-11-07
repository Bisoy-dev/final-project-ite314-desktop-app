using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeatVibesApp
{
    public partial class Form1 : Form
    {
        private static string[] genre = { "","Classical","Pop","Jazz","R&B","Country Pop","Rock","Soul","Heavy","Metal","Hip-Hop"};
        private static string[] recordLabels = { "","Universal Music Group","Sony Music Entertainment",
            "Warner Music Group","BMG Rights Management", "Indie Labels"};
        private static string fileName = "";

        public static List<SongModel> songs = new List<SongModel>();
        public static int songId;
        public Form1()
        {
            InitializeComponent();
            BeatVibesApiHelper.InitializeConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbGenre.DataSource = genre;
            cmbRecordLabel.DataSource = recordLabels;

            LoadSongs();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "MP3 File | *.mp3";
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                }
            }
        } 

        private async void LoadSongs()
        {
            songs = await SongProcessor.GetAllSongs();
            bindingSourceSong.DataSource = songs;
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            //var json = JsonConvert.SerializeObject(song ,Formatting.Indented);
            //MessageBox.Show(string.Join(", ",json)); 
            if(txtSongTitle.Text.Length > 0 && txtArtist.Text.Length > 0 && fileName.Length > 0 &&
                recordLabels[cmbRecordLabel.SelectedIndex].Length > 0 && genre[cmbGenre.SelectedIndex].Length > 0)
            {
                var song = new SongModel()
                {
                    SongTitle = txtSongTitle.Text,
                    Artist = txtArtist.Text,
                    Album = txtAlbum.Text,
                    RecordLabel = recordLabels[cmbRecordLabel.SelectedIndex],
                    Genre = genre[cmbGenre.SelectedIndex],
                    ReleaseDate = dtpReleaseDate.Value,
                    TrackFile = fileName
                };
                string result = await SongProcessor.PostSong(song);
                if (result.Equals("1"))
                {
                    MessageBox.Show("Successfully added","Message");
                    LoadSongs();
                }
                else
                {
                    MessageBox.Show("Failed to Insert", "Message");
                }
            }
            else
            {
                MessageBox.Show("Invalid Input");
            }
        }

        private void dgvSongs_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var id = dgvSongs.Rows[e.RowIndex].Cells[1].Value.ToString();
            songId = Convert.ToInt32(id);
            SongDetailForm songDetailForm = new SongDetailForm();
            songDetailForm.Show();
        }
    }
}
