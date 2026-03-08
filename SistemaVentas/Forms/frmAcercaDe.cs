using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CapaPresentacion.Forms
{
    public partial class frmAcercaDe : Form
    {
        public frmAcercaDe()
        {
            InitializeComponent();
        }

        private void frmAcercaDe_Load(object sender, EventArgs e)
        {
            string id = SeguridadHardware.ObtenerHardwareID();
            lbNumeroSerie.Text = id;

            // Si sigue fallando, es que el control se llama distinto o está siendo pisado
            Console.WriteLine("ID Detectado: " + id);
        }
    }
}
