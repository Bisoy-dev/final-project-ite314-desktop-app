using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace BeatVibesApp
{
    public partial class SongDetailForm : Form
    {
        SongModel song = new SongModel();
        private WindowsMediaPlayer player = new WindowsMediaPlayer();
        string trackUrl = "http://localhost/beat-vibes/tracks/";
        string currentPlay = "";
        bool isPlay;
        int index = 0;
        public SongDetailForm()
        {
            InitializeComponent();
        }

        private void SongDetailForm_Load(object sender, EventArgs e)
        {
            LoadSong(Form1.songId);
        } 

        private async void LoadSong(int id)
        {
            song = await SongProcessor.GetSong(id);
            lblArtist.Text = song.Artist;
            lblTitle.Text = song.SongTitle;
            lblReleaseDate.Text = $"Release Date: {song.ReleaseDate.ToString("yyyy/MM/dd")}";
            lblRecLabel.Text = $"Record Label: {song.RecordLabel}";
            lblAlbum.Text = $"Album: {song.Album}";
            lblGenre.Text = $"Genre: {song.Genre}";
            index = Form1.songs.FindIndex(s => s.Id == id);
        }

        private void btnPrev_MouseHover(object sender, EventArgs e)
        {
            btnPrev.BackColor = Color.FromArgb(244, 212, 124);
         
            btnPrev.ForeColor = Color.Black;
        }
        private void Play(string fileName)
        {
            string url = $"{trackUrl}{fileName}";

            player.URL = url;
            player.controls.play();
        } 
        private void Stop(string fileName)
        {
            string url = $"{trackUrl}{fileName}";
            player.URL = url;
            player.controls.stop();
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            isPlay = !isPlay;
            if (isPlay)
            {
                btnPlay.Text = "Stop";
                Play(song.TrackFile);
            }
            else
            {
                btnPlay.Text = "Play";
                Stop(song.TrackFile);
            }
        }

        private void btnPrev_MouseHover_1(object sender, EventArgs e)
        {
            btnPrev.ForeColor = Color.Black;
        }

        private void btnPlay_MouseHover(object sender, EventArgs e)
        {
            btnPlay.ForeColor = Color.Black;
        }

        private void btnNext_MouseHover(object sender, EventArgs e)
        {
            btnNext.ForeColor = Color.Black;
        }

        private void btnPrev_MouseLeave(object sender, EventArgs e)
        {
            btnPrev.ForeColor = Color.FromArgb(190, 190, 190);
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            btnPlay.ForeColor = Color.FromArgb(190, 190, 190);
        }

        private void btnNext_MouseLeave(object sender, EventArgs e)
        {
            btnNext.ForeColor = Color.FromArgb(190, 190, 190);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if(index >= 1)
            {
                Stop(currentPlay);
                index -= 1;
                var song = Form1.songs[index];
                LoadSong(song.Id);
                if (isPlay)
                {
                    Play(song.TrackFile);
                }
                currentPlay = song.TrackFile;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(index <= Form1.songs.Count - 2)
            {
                Stop(currentPlay);

                index += 1;
                var song = Form1.songs[index];
                LoadSong(song.Id);
                if (isPlay)
                {
                    Play(song.TrackFile);
                }
                currentPlay = song.TrackFile;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Stop(song.TrackFile);
            this.Close();
        }
    }
}
