using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Configuration;

namespace AnalyseForTime
{
    public partial class NNAnalyseForm : Form
    {
        public NNAnalyseForm()
        {
            InitializeComponent();

        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            //Analyse analyse = new Analyse();

            //analyse.analyse(int.Parse(txtSn.Text));

            Analyse2 analyse = new Analyse2();

            new ConfiguratedByForm(analyse);

            analyse.analyse();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}