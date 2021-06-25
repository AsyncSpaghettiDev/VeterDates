using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeterDates {
    public partial class Seleccion : Form {
        readonly string[] tablasGerente = {
            "MASCOTAS",
            "DUEÑOS",
            "VETERINARIOS",
            "CONSULTORIOS",
            "CONSULTAS",
            "DETALLE DE CONSULTAS"
        };
        readonly string[] tablasVeterinario = {
            "MASCOTAS"
        };
        readonly string[] tablasSecre = {
            "MASCOTAS",
            "DUEÑOS",
            "CONSULTAS",
            "DETALLE DE CONSULTAS"
        };
        public Seleccion( Usertype usuario ) {
            InitializeComponent();
            setTableContent(usuario);
        }
        void setTableContent( Usertype usuario ) {
            switch (usuario) {
                case Usertype.Gerente:
                    this.comboBox1.Items.AddRange(tablasGerente);
                    break;

                case Usertype.Veterinario:
                    this.comboBox1.Items.AddRange(tablasVeterinario);
                    break;

                case Usertype.Secretaria:
                    this.comboBox1.Items.AddRange(tablasSecre);
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e ) {
            switch (this.comboBox1.Text) {
                case "MASCOTAS":
                    new Mascotas(this).Show();
                    Hide();
                    break;

                case "DUEÑOS":
                    new Duenios().Show();
                    Hide();
                    break;

                case "VETERINARIOS":
                    new Veterinarios(this).Show();
                    Hide();
                    break;

                case "CONSULTORIOS":
                    new Consultorios().Show();
                    Hide();
                    break;

                case "CONSULTAS":
                    new Consultas(this).Show();
                    Hide();
                    break;

                case "DETALLE DE CONSULTAS":
                    new DetallesConsulta(this).Show();
                    Hide();
                    break;

                default:
                    MessageBox.Show("Por favor seleccione adecuadamente.");
                    break;
            }
        }

        private void pictureBox3_Click( object sender, EventArgs e ) {
            Application.Exit();
        }
    }
}
