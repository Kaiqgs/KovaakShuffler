using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KovaakShuffler
{
    public class UserMessages
    {
        
        public void ValidIO()
        {
            MessageBox.Show("Valid Input/Output operation", "IO Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void InvalidPlaylist()
        {
            MessageBox.Show("Invalid Playlist for this operation", "Playlist Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void InvalidIO()
        {
            MessageBox.Show("Invalid Input/Output for this operation", "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void InvalidOverwrite()
        {
            MessageBox.Show("Invalid Overwriting for this operation", "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

    }
}
