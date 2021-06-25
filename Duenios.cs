using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConexionMYSQL;
using System.Windows.Forms;

namespace VeterDates {
    public partial class Duenios : Form {
        readonly BaseDatos @base = new BaseDatos("mysql-veterdates.alwaysdata.net", "237436", "Ma-^VdUyZeN$JyW", "veterdates_tijuana");
        readonly Form menuPrincipal;
        public Duenios( Form menu ) {
            InitializeComponent();
            this.menuPrincipal = menu;
            crearGrid();
            cargarDatos();
            this.bunifuImageButton4.Hide();
        }

        private void pictureBox3_Click( object sender, EventArgs e ) {
            Application.Exit();
        }

        private void bunifuImageButton5_Click( object sender, EventArgs e ) {
            this.menuPrincipal.Show();
            Dispose();
        }
        private void limpiar( ) {
            this.comboBox1.Items.Clear();
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
            cargarDatos();
        }
        void cargarDatos( ) {
            this.bunifuDataGridView1.Rows.Clear();
            foreach (List<string> dt in this.@base.Buscar("DUENIOS")) {
                this.comboBox1.Items.Add(dt[ 0 ]);
                this.bunifuDataGridView1.Rows.Add(dt[ 0 ], dt[ 1 ], dt[ 2 ], dt[ 3 ], dt[ 4 ]);
            }
        }
        void crearGrid( ) {
            this.bunifuDataGridView1.ColumnCount = 5;
            this.bunifuDataGridView1.Columns[ 0 ].Name = "ID";
            this.bunifuDataGridView1.Columns[ 1 ].Name = "NOMBRE";
            this.bunifuDataGridView1.Columns[ 2 ].Name = "APELLIDOS";
            this.bunifuDataGridView1.Columns[ 3 ].Name = "TELEFONO";
            this.bunifuDataGridView1.Columns[ 4 ].Name = "DIRECCION";
        }

        private void bunifuImageButton1_Click( object sender, EventArgs e ) {
            string[] datos = new string[] {
                $"('{this.comboBox1.Text}')",
                $"('{this.textBox1.Text}')",
                $"('{this.textBox2.Text}')",
                $"('{this.textBox3.Text}')",
                $"('{this.textBox4.Text}')"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Alta("DUENIOS", datos)) {
                MessageBox.Show("Registro completado");
                limpiar();
            }
            else if (MessageBox.Show("¿Desea sobreescribir los datos del ID?", "ID repetido", MessageBoxButtons.YesNo) == DialogResult.Yes)
                bunifuImageButton4_Click(null, null);
            else
                limpiar();
        }

        private void bunifuImageButton2_Click( object sender, EventArgs e ) {
            this.@base.Baja("DUENIOS", $"idDuenios='{this.comboBox1.Text}'");
            limpiar();
            MessageBox.Show("Registro Eliminado Correctamente");
        }

        private void bunifuImageButton3_Click( object sender, EventArgs e ) {
            List<List<string>> consulta = this.@base.Buscar("DUENIOS", $"idDuenios='{this.comboBox1.Text}'");
            this.textBox1.Text = consulta[ 0 ][ 1 ];
            this.textBox2.Text = consulta[ 0 ][ 2 ];
            this.textBox3.Text = consulta[ 0 ][ 3 ];
            this.textBox4.Text = consulta[ 0 ][ 4 ];

            if (!string.IsNullOrEmpty(this.comboBox1.Text)) {
                this.comboBox1.Enabled = false;
                this.bunifuImageButton1.Hide();
                this.bunifuImageButton2.Hide();
                this.bunifuImageButton3.Hide();
                this.bunifuImageButton4.Show();
            }
        }

        private void bunifuImageButton4_Click( object sender, EventArgs e ) {
            string[] datos = new string[] {
                $"nombreDuenio='{this.textBox1.Text}'",
                $"apellidosDuenio='{this.textBox2.Text}'",
                $"telefonoDuenio='{this.textBox3.Text}'",
                $"direccionDuenio='{this.textBox4.Text}'"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Actualizar("DUENIOS", datos, $"idDuenios='{this.comboBox1.Text}'")) {
                MessageBox.Show("Registro actualizado");
                limpiar();
            }
            else
                MessageBox.Show("Ocurrió un error, revisa tus datos");
        }
        private bool validos( ) => string.IsNullOrEmpty(this.comboBox1.Text) ||
            string.IsNullOrEmpty(this.textBox1.Text) ||
            string.IsNullOrEmpty(this.textBox2.Text) ||
            string.IsNullOrEmpty(this.textBox3.Text) ||
            string.IsNullOrEmpty(this.textBox4.Text);

        private void comboBox1_Validating( object sender, CancelEventArgs e ) {
            ToolTip advertencia = new ToolTip();
            if (!string.IsNullOrWhiteSpace(( sender as Control ).Text)) {
                if (( sender as Control ).Text.Contains(";") || ( sender as Control ).Text.Contains("=")) {
                    advertencia.ToolTipTitle = "Hey, te cacé";
                    advertencia.Show("No puedes usar los signos '=' o ';'", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
                else if (( sender as Control ).Text.Length > 8) {
                    advertencia.ToolTipTitle = "ID muy largo";
                    advertencia.Show("El ID no debe exceder de 8 caracteres", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
                else if (!int.TryParse(( sender as Control ).Text, out _)) {
                    advertencia.ToolTipTitle = "ID inválido";
                    advertencia.Show("El ID solo debe contener números", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
            }
        }
        private void textBox1_Validating( object sender, CancelEventArgs e ) {
            ToolTip advertencia = new ToolTip();
            if (!string.IsNullOrWhiteSpace(( sender as Control ).Text)) {
                if (( sender as Control ).Text.Contains(";") || ( sender as Control ).Text.Contains("=")) {
                    advertencia.ToolTipTitle = "Hey, te cacé";
                    advertencia.Show("No puedes usar los signos '=' o ';'", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
            }
        }

        private void textBox2_Validating( object sender, CancelEventArgs e ) {
            ToolTip advertencia = new ToolTip();
            if (!string.IsNullOrWhiteSpace(( sender as Control ).Text)) {
                if (( sender as Control ).Text.Contains(";") || ( sender as Control ).Text.Contains("=")) {
                    advertencia.ToolTipTitle = "Hey, te cacé";
                    advertencia.Show("No puedes usar los signos '=' o ';'", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
            }
        }

        private void textBox3_Validating( object sender, CancelEventArgs e ) {
            ToolTip advertencia = new ToolTip();
            if (!string.IsNullOrWhiteSpace(( sender as Control ).Text)) {
                if (( sender as Control ).Text.Contains(";") || ( sender as Control ).Text.Contains("=")) {
                    advertencia.ToolTipTitle = "Hey, te cacé";
                    advertencia.Show("No puedes usar los signos '=' o ';'", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
                else if (!long.TryParse(( sender as Control ).Text, out _)) {
                    advertencia.ToolTipTitle = "ID inválido";
                    advertencia.Show("El numero de telefono solo debe contener números", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
                else if ((sender as Control).Text.Length != 10) {
                    advertencia.ToolTipTitle = "Número de teléfono inválido";
                    advertencia.Show("El número de teléfono debe contener 10 dígitos números", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
            }
        }

        private void textBox4_Validating( object sender, CancelEventArgs e ) {
            ToolTip advertencia = new ToolTip();
            if (!string.IsNullOrWhiteSpace(( sender as Control ).Text)) {
                if (( sender as Control ).Text.Contains(";") || ( sender as Control ).Text.Contains("=")) {
                    advertencia.ToolTipTitle = "Hey, te cacé";
                    advertencia.Show("No puedes usar los signos '=' o ';'", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
            }
        }
    }
}
