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
    public partial class Consultorios : Form {
        readonly BaseDatos @base = new BaseDatos("mysql-veterdates.alwaysdata.net", "237436", "Ma-^VdUyZeN$JyW", "veterdates_tijuana");
        readonly Form menuPrincipal;
        public Consultorios( Form menu ) {
            InitializeComponent();
            this.menuPrincipal = menu;
            crearGrid();
            cargarDatos();
            this.bunifuImageButton4.Hide();
        }

        private void pictureBox3_Click( object sender, EventArgs e ) {
            Application.Exit();
        }

        private void limpiar( ) {
            this.comboBox1.Items.Clear();
            this.comboBox2.Items.Clear();
            this.bunifuTextBox1.Clear();
            cargarDatos();
        }
        void cargarDatos( ) {
            this.bunifuDataGridView1.Rows.Clear();
            foreach (List<string> dt in this.@base.Buscar("CONSULTORIOS")) {
                this.comboBox1.Items.Add(dt[ 0 ]);
                this.comboBox2.Items.Add(dt[ 1 ]);
                this.bunifuDataGridView1.Rows.Add(dt[ 0 ], dt[ 1 ], dt[ 2 ]);
            }
        }
        void crearGrid( ) {
            this.bunifuDataGridView1.ColumnCount = 3;
            this.bunifuDataGridView1.Columns[ 0 ].Name = "ID";
            this.bunifuDataGridView1.Columns[ 1 ].Name = "MEDICO";
            this.bunifuDataGridView1.Columns[ 2 ].Name = "DESCRIPCION";
        }

        private void bunifuImageButton5_Click( object sender, EventArgs e ) {
            this.menuPrincipal.Show();
            Dispose();
        }

        private void bunifuTextBox1_Validating( object sender, CancelEventArgs e ) {
            ToolTip advertencia = new ToolTip();
            if (!string.IsNullOrWhiteSpace(( sender as Control ).Text)) {
                if (( sender as Control ).Text.Contains(";") || ( sender as Control ).Text.Contains("=")) {
                    advertencia.ToolTipTitle = "Hey, te cacé";
                    advertencia.Show("No puedes usar los signos '=' o ';'", sender as IWin32Window, 2000);
                    e.Cancel = true;
                }
            }
        }

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

        private void bunifuImageButton1_Click( object sender, EventArgs e ) {
            string[] datos = new string[] {
                $"('{this.comboBox1.Text}')",
                $"('{this.comboBox2.Text}')",
                $"('{this.bunifuTextBox1.Text}')"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Alta("CONSULTORIOS", datos)) {
                MessageBox.Show("Registro completado");
                limpiar();
            }
            else if (MessageBox.Show("¿Desea sobreescribir los datos del ID?", "ID repetido", MessageBoxButtons.YesNo) == DialogResult.Yes)
                bunifuImageButton4_Click(null, null);
            else
                limpiar();
        }

        private void bunifuImageButton2_Click( object sender, EventArgs e ) {
            this.@base.Baja("CONSULTORIOS", $"idConsultorio='{this.comboBox1.Text}'");
            limpiar();
        }

        private void bunifuImageButton3_Click( object sender, EventArgs e ) {
            List<List<string>> consulta = this.@base.Buscar("CONSULTORIOS", $"idConsultorio='{this.comboBox1.Text}'");
            this.comboBox2.SelectedItem = consulta[ 0 ][ 1 ];
            this.bunifuTextBox1.Text = consulta[ 0 ][ 2 ];

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
                $"idMedico='{this.comboBox2.Text}'",
                $"descripcionConsultorio='{this.bunifuTextBox1.Text}'"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Actualizar("CONSULTORIOS", datos, $"idConsultorio='{this.comboBox1.Text}'")) {
                MessageBox.Show("Registro actualizado");
                limpiar();
            }
            else
                MessageBox.Show("Ocurrió un error, revisa tus datos");
        }
        private bool validos( ) => string.IsNullOrEmpty(this.comboBox1.Text) ||
            string.IsNullOrEmpty(this.comboBox2.Text) ||
            string.IsNullOrEmpty(this.bunifuTextBox1.Text);
    }
}
