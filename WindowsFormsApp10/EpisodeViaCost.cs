using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp10
{
    public partial class EpisodeViaCost : Form
    {
        List<int> episodeViaCost = new List<int>();
        public EpisodeViaCost(List<int> EpisodeViaCost)
        {
            this.episodeViaCost = EpisodeViaCost;
            InitializeComponent();
        }

        private void EpisodeViaCost_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < episodeViaCost.Count; i++)
            {
                this.chart1.Series["Episode Via Cost"].Points.AddXY(i, episodeViaCost[i]);
            }
        }
    }
}
