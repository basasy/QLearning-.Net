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
    public partial class EpisodeViaStep : Form
    {
        List<int> episodeViaStep = new List<int>();
        public EpisodeViaStep(List<int> EpisodeViaStep)
        {
            this.episodeViaStep = EpisodeViaStep;
            InitializeComponent();
        }

        private void EpisodeViaStep_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < episodeViaStep.Count; i++) {
                this.chart1.Series["Episode Via Step"].Points.AddXY(i,episodeViaStep[i]);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
