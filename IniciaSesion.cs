using System;
using ConexionMYSQL;
using System.Windows.Forms;

namespace VeterDates {
    public enum Usertype {
        Gerente,
        Veterinario,
        Secretaria
    }
    public partial class IniciaSesion : Form {
        readonly BaseDatos @base = new BaseDatos("mysql-veterdates.alwaysdata.net", "237436", "Ma-^VdUyZeN$JyW", "veterdates_tijuana");
        public IniciaSesion( ) {
            InitializeComponent();
        }

        private void bunifuButton1_Click( object sender, EventArgs e ) {
            try {
                Usertype user = ( Usertype ) int.Parse(this.@base.Login(this.bunifuTextBox1.Text, this.bunifuTextBox2.Text));
                new Seleccion(user).Show();
                Dispose(false);
            }
            catch (Exception) {
                MessageBox.Show("Credenciales Inválidas, prueba de nuevo.");
            }
        }

        private void pictureBox3_Click( object sender, EventArgs e ) {
            Application.Exit();
        }
    }
}
