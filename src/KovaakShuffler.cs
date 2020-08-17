using GvasFormat;
using GvasFormat.Serialization;
using GvasFormat.Serialization.UETypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KovaakShuffler
{
    public partial class KovaakShufflerMain : Form
    {
        public const string KOVAAK_PLAYLIST_EXTENSION = ".plo";

        private Gvas _playlist;
        private string _playlistPath;
        private List<string> _importedListPlaylist;
        private List<Tuple<string, int>> _shuffledPlaylist;
        private UEArrayProperty PlaylistScenarios => ((UEArrayProperty)(((UEGenericStructProperty)_playlist.Properties[0]).Properties[1]));
        private UEStringProperty PlaylistName => ((UEStringProperty)(((UEGenericStructProperty)_playlist.Properties[0]).Properties[0]));
        private bool ValidShuffledPlaylist => _shuffledPlaylist != null;


        public KovaakShufflerMain()
        {
            InitializeComponent();


            //Gvas save;



            //Playlist Name: ((UEStringProperty)(((UEGenericStructProperty)save.Properties[0]).Properties[0]))
            //Playlist Scenarios: ((UEArrayProperty)(((UEGenericStructProperty)save.Properties[0]).Properties[1]))

            //((UEStringProperty)(((UEGenericStructProperty)save.Properties[0]).Properties[0])).Value = "Fota - Shuffled";
            //var UEscenarios = ((UEArrayProperty)(((UEGenericStructProperty)save.Properties[0]).Properties[1]));

            //if(UEscenarios.Items.Count() > 0) { 

            //var scenarios = ImportScenarios(UEscenarios.Items);

            //var shuffled = ShuffleScenarios(scenarios);
            //var sameshuffled = GroupBySame(shuffled);

            //var arr = ExportScenarios(shuffled, (UEGenericStructProperty)UEscenarios.Items[0]);

            //Console.WriteLine(((UEArrayProperty)(((UEGenericStructProperty)save.Properties[0]).Properties[1])).Items.ToList().Format());

            //((UEArrayProperty)(((UEGenericStructProperty)save.Properties[0]).Properties[1])).Items = arr;

            //Console.WriteLine(((UEArrayProperty)(((UEGenericStructProperty)save.Properties[0]).Properties[1])).Items.ToList().Format());
            //}










        }





        public List<string> ShuffleScenarios(List<string> scenarios)
        {
            return scenarios.OrderBy(x => Guid.NewGuid()).ToList();
        }



        private void UpdateProgramState()
        {
            groupBox1.Enabled = ValidShuffledPlaylist;
            if (ValidShuffledPlaylist)
            {
                UpdatePlaylistListbox(_shuffledPlaylist);
            }
        }
        private void UpdatePlaylistListbox(List<Tuple<string, int>> playlist)
        {
            listBox1.Items.Clear();
            foreach (var val in playlist) listBox1.Items.Add($"{val.Item1} {val.Item2}x");
        }

        #region Operations


        private void ShufflePlaylist(object sender, EventArgs e)
        {
            var res = ShuffleScenarios(_importedListPlaylist);
            _shuffledPlaylist = new GroupUtil().GroupBySame(res);
            UpdateProgramState();
        }
        private void LoadPlaylist(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.DefaultExt = "plo";
            fd.AddExtension = true;
            fd.Filter = "Playlist (*.plo) | *.plo";
            var res = fd.ShowDialog();
            if (res.Accept())
            {
                try
                {
                    using (var stream = File.Open(fd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        _playlist = UESerializer.Read(stream);
                }
                catch
                {
                    _playlist = null;
                    new UserMessages().InvalidIO();
                }
                if (_playlist != null && PlaylistScenarios.Items.Count() > 0)
                {
                    _playlistPath = fd.FileName;
                    _shuffledPlaylist = new KovaakHandler().ImportPlaylist(PlaylistScenarios.Items);
                    _importedListPlaylist = new GroupUtil().UngroupBySame(_shuffledPlaylist);
                    _playlistNameTextbox.Text = $"{PlaylistName.Value} - Shuffled";
                    UpdateProgramState();
                }


            }

            else new UserMessages().InvalidIO();
        }
        private void SavePlaylist(object sender, EventArgs e)
        {
            var dir = Path.GetDirectoryName(_playlistPath);
            var pname = _playlistNameTextbox.Text;
            var path = Path.Combine(dir, pname + KOVAAK_PLAYLIST_EXTENSION);
            var fexists = File.Exists(path);
            if (!fexists || overwriteCheckbox.Checked)
            {
                PlaylistName.Value = pname;
                PlaylistScenarios.Items = new KovaakHandler().ExportPlaylist(_shuffledPlaylist, (UEGenericStructProperty)PlaylistScenarios.Items[0]);

                File.WriteAllText(path, string.Empty);
                using (var stream = File.Open(path, FileMode.Append))
                    UESerializer.WriteKovaakSpecific(_playlist, stream);
                new UserMessages().ValidIO();

            }else if(fexists && !overwriteCheckbox.Checked)
            {
                new UserMessages().InvalidOverwrite();
            }
        }

        #endregion


    }
}
