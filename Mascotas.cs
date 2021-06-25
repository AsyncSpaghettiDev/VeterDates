using System;
using System.Collections.Generic;
using System.ComponentModel;
using ConexionMYSQL;
using System.Windows.Forms;

namespace VeterDates {
    public partial class Mascotas : Form {
        readonly BaseDatos @base = new BaseDatos("mysql-veterdates.alwaysdata.net", "237436", "Ma-^VdUyZeN$JyW", "veterdates_tijuana");
        readonly Form menuPrincipal;
        public Mascotas( Form menu ) {
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
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.bunifuTextBox1.Clear();
            this.bunifuTextBox2.Clear();
            cargarDatos();
        }
        void cargarDatos( ) {
            this.bunifuDataGridView1.Rows.Clear();
            this.comboBox1.Items.Clear();
            this.comboBox2.Items.Clear();
            foreach (List<string> dt in this.@base.Buscar("MASCOTAS")) {
                this.comboBox1.Items.Add(dt[ 0 ]);
                this.comboBox2.Items.Add(dt[ 1 ]);
                this.bunifuDataGridView1.Rows.Add(dt[ 0 ], dt[ 1 ], dt[ 2 ], dt[ 3 ]);
            }
        }
        void crearGrid( ) {
            this.bunifuDataGridView1.ColumnCount = 4;
            this.bunifuDataGridView1.Columns[ 0 ].Name = "ID";
            this.bunifuDataGridView1.Columns[ 0 ].Width = 120;
            this.bunifuDataGridView1.Columns[ 1 ].Name = "DUEÑO";
            this.bunifuDataGridView1.Columns[ 1 ].Width = 120;
            this.bunifuDataGridView1.Columns[ 2 ].Name = "TIPO";
            this.bunifuDataGridView1.Columns[ 3 ].Name = "NOMBRE";
        }

        private void bunifuImageButton1_Click( object sender, EventArgs e ) {
            string[] datos = new string[] {
                $"('{this.comboBox1.Text}')",
                $"('{this.comboBox2.Text}')",
                $"('{this.bunifuTextBox1.Text}')",
                $"('{this.bunifuTextBox2.Text}')"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Alta("MASCOTAS", datos)) {
                MessageBox.Show("Registro completado");
                limpiar();
            }
            else if (MessageBox.Show("¿Desea sobreescribir los datos del ID?", "ID repetido", MessageBoxButtons.YesNo) == DialogResult.Yes)
                bunifuImageButton4_Click(null, null);
            else
                limpiar();
        }

        private void bunifuImageButton2_Click( object sender, EventArgs e ) {
            this.@base.Baja("MASCOTAS", $"idMascota='{this.comboBox1.Text}'");
            limpiar();
        }

        private void bunifuImageButton3_Click( object sender, EventArgs e ) {
            List<List<string>> consulta = this.@base.Buscar("MASCOTAS", $"idMascota='{this.comboBox1.Text}'");
            this.comboBox2.Text = consulta[ 0 ][ 1 ];
            this.bunifuTextBox1.Text = consulta[ 0 ][ 2 ];
            this.bunifuTextBox2.Text = consulta[ 0 ][ 3 ];

            if (!string.IsNullOrEmpty(this.comboBox1.Text)) {
                this.comboBox1.Enabled = false;
                this.comboBox2.Enabled = false;
                this.bunifuImageButton1.Hide();
                this.bunifuImageButton2.Hide();
                this.bunifuImageButton3.Hide();
                this.bunifuImageButton4.Show();
            }
        }

        private void bunifuImageButton4_Click( object sender, EventArgs e ) {
            string[] datos = new string[] {
                $"idDuenio='{this.comboBox2.Text}'",
                $"tipoMascota='{this.bunifuTextBox1.Text}'",
                $"nombreMascota='{this.bunifuTextBox2.Text}'"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Actualizar("MASCOTAS", datos, $"idMascota='{this.comboBox1.Text}'")) {
                MessageBox.Show("Registro actualizado");
                limpiar();
            }
            else
                MessageBox.Show("Ocurrió un error, revisa tus datos");
            this.comboBox1.Enabled = true;
            this.comboBox2.Enabled = true;
            this.bunifuImageButton1.Show();
            this.bunifuImageButton2.Show();
            this.bunifuImageButton3.Show();
            this.bunifuImageButton4.Hide();
        }
        private bool validos( ) => 
            string.IsNullOrEmpty(this.comboBox1.Text) || 
            string.IsNullOrEmpty(this.comboBox2.Text) || 
            string.IsNullOrEmpty(this.bunifuTextBox1.Text) || 
            string.IsNullOrEmpty(this.bunifuTextBox2.Text);

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
            }
        }

        private void bunifuTextBox1_Validating( object sender, CancelEventArgs e ) {
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
            }
        }

        private void bunifuTextBox2_Validating( object sender, CancelEventArgs e ) {
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
            }
        }
    }
}
